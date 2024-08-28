using FluentValidation;
using PetProject.Application.Volunteers.Update;
using PetProject.Domain.Shared;

namespace PetProject.API.Volunteers
{
    public class DeleteVolunteerRequestValidator : AbstractValidator<DeleteVolunteerRequest>
    {
        public DeleteVolunteerRequestValidator() 
        {
            RuleFor(i => i.VolunteerId).NotEmpty().WithError(Errors.General.Failure("id"));
        }
    }
}
