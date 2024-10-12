using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Species;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Repositories.Write
{
    public interface ISpeciesRepository
    {
        Guid Save(Species species);
        Guid Delete(Species species);
        Task<Result<Species, Error>> GetById(
            SpeciesId speciesId,
            CancellationToken cancellationToken = default);
    }
}
