using FluentValidation;
using PetProject.Application.Volunteers.Update;
using PetProject.Domain.Shared;

namespace PetProject.API.Volunteers
{
    public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
    {
        public DeleteVolunteerCommandValidator() 
        {
            RuleFor(i => i.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("id"));
        }
    }
}
