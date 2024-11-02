using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetProject.Core;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;
using PetProject.Core.Application.Repositories;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Volunteers.Application.Pets.Commands.Get
{
    public class GetPetHandler(IReadDbContext _readDbContext) : ICommandHandler<Guid, Result<PetDto, ErrorList>>
    {
        public async Task<Result<PetDto, ErrorList>> Handle(
            Guid petId,
            CancellationToken cancellationToken = default)
        {
            var pet = await _readDbContext.Pets.Where(i => i.Id == petId)
                .FirstOrDefaultAsync(cancellationToken);

            if (pet == null)
                return (ErrorList)Errors.General.NotFound(petId);

            return pet;
        }
    }
}
