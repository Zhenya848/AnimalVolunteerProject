﻿using PetProject.Domain.Shared.ValueObjects.Dtos;

namespace PetProject.Application.Volunteers.Pets.Commands.Create
{
    public record CreatePetCommand(
        Guid VolunteerId,
        string Name,
        string Description,
        string Color,
        string HealthInfo,
        AddresDto Addres,
        string TelephoneNumber,
        float Weight,
        float Height,
        bool IsCastrated,
        bool IsVaccinated,
        DateTime BirthdayTime,
        DateTime DateOfCreation,
        IEnumerable<RequisiteDto> Requisites,
        HelpStatus HelpStatus);
}