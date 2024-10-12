using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery
{
    public record BreedDto(Guid Id, Guid SpeciesId, string Title);
}
