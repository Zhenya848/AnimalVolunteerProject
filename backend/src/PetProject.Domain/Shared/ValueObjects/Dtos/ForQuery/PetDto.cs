using PetProject.Domain.Species;
using PetProject.Domain.Volunteers.ValueObjects.Collections;
using PetProject.Domain.Volunteers.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery
{
    public record PetDto
    {
        public Guid Id { get; }
        public Guid VolunteerId { get; }

        public string Name { get; } = default!;
        public string Description { get; } = default!;
        public string Color { get; } = default!;
        public string HealthInfo { get; } = default!;
        public string Street { get; } = default!;
        public string City { get; } = default!;
        public string State { get; } = default!;
        public string ZipCode { get; } = default!;
        public string TelephoneNumber { get; } = default!;

        public Guid BreedId { get; }
        public Guid SpeciesId { get; }

        public int SerialNumber { get; } = default!;

        public float Weight { get; }
        public float Height { get; }

        public bool IsCastrated { get; }
        public bool IsVaccinated { get; }

        public DateTime BirthdayTime { get; }
        public DateTime DateOfCreation { get; }

        public RequisiteDto[] RequisitesDto { get; } = default!;
        public string[] PhotoPaths { get; } = default!;

        public HelpStatus HelpStatus { get; }
    }
}
