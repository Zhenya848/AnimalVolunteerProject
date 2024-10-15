using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.Database;
using PetProject.Application.Messaging;
using PetProject.Application.Repositories.Read;
using PetProject.Application.Repositories.Write;
using PetProject.Application.Shared.Interfaces;
using PetProject.Application.Volunteers.Pets.Commands.Create;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace PetProject.Application.Volunteers.Pets.Commands.Get
{
    public class GetPetHandler(IReadDbContext _readDbContext) : IQueryHandler<Guid, Result<PetDto, ErrorList>>
    {
        public async Task<Result<PetDto, ErrorList>> Get(
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
