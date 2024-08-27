using FluentValidation;
using PetProject.Application.Volunteers.Update;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.API.Volunteers
{
    public class UpdateVolunteerRequestValidator : AbstractValidator<UpdateVolunteerRequest>
    {
        public UpdateVolunteerRequestValidator()
        {
            RuleFor(i => i.volunteerId).NotEmpty().WithError(Errors.General.Failure("id"));
        }
    }
}
