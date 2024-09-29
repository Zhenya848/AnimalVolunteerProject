using PetProject.Domain.Volunteers.ValueObjects.Collections;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Volunteers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery
{
    public record VolunteerDto
    {
        public Guid Id { get; }

        public string FirstName { get; } = default!;
        public string LastName { get; } = default!;
        public string Patronymic { get; } = default!;

        public string Description { get; } = default!;
        public string PhoneNumber { get; } = default!;

        public RequisiteDto[] RequisitesDto { get; } = default!;
        public SocialNetworkDto[] SocialNetworksDto { get; } = default!;

        public int Experience { get; } = default!;
    }
}
