using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Species.Application.Repositories;
using PetProject.Species.Domain;

namespace PetProject.Species.Application.Breeds.Commands.Create;

public class CreateBreedHandler : ICommandHandler<CreateBreedCommand, Result<Guid, ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBreedHandler(
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        [FromKeyedServices(Modules.Species)]IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateBreedCommand command, 
        CancellationToken cancellationToken = default)
    {
        var speciesExist = await _speciesRepository.GetById(
            SpeciesId.Create(command.SpeciesId),
            cancellationToken);

        if (speciesExist.IsFailure)
            return (ErrorList)speciesExist.Error;

        var breedExist = await _readDbContext.Breeds
            .FirstOrDefaultAsync(t=> t.Title == command.BreedName, cancellationToken);
        
        if (breedExist != null)
            return (ErrorList)Error.Validation("value.already.exist", "Breed already exists");
        
        var breedResult = Breed.Create(command.BreedName);
        
        if (breedResult.IsFailure)
            return (ErrorList)breedResult.Error;
        
        speciesExist.Value.Breeds.Add(breedResult.Value);
        
        _speciesRepository.Save(speciesExist.Value);
        await _unitOfWork.SaveChanges(cancellationToken);

        return (Guid)breedResult.Value.Id;
    }
}