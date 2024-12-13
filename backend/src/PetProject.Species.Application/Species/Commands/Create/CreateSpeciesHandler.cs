using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Core;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Repositories;
using PetProject.Species.Application.Repositories;

namespace PetProject.Species.Application.Species.Commands.Create;

public class CreateSpeciesHandler : ICommandHandler<CreateSpeciesCommand, Result<Guid, ErrorList>>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpeciesHandler(
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        [FromKeyedServices(Modules.Species)]IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSpeciesCommand command, 
        CancellationToken cancellationToken = default)
    {
        var speciesExist = await _readDbContext.Species
            .FirstOrDefaultAsync(s => s.Name == command.SpeciesName, cancellationToken);
        
        if (speciesExist != null)
            return (ErrorList)Error.Validation("value.already.exist", "Species already exists");
        
        if (string.IsNullOrWhiteSpace(command.SpeciesName))
            return (ErrorList)Error.Validation("value.name.empty", "Species name is required");

        var speciesResult = Domain.Species.Create(command.SpeciesName);
        
        if (speciesResult.IsFailure)
            return (ErrorList)speciesResult.Error;
        
        var result = _speciesRepository.Create(speciesResult.Value);
        await _unitOfWork.SaveChanges(cancellationToken);

        return result;
    }
}