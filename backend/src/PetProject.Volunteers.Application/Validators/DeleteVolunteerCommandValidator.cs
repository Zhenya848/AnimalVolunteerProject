using FluentValidation;
using PetProject.Core;
using PetProject.Core.Application.Validation;
using PetProject.Volunteers.Application.Volunteers.Commands.Delete;

namespace PetProject.Volunteers.Application.Validators
{
    public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
    {
        public DeleteVolunteerCommandValidator() 
        {
            RuleFor(i => i.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("id"));
        }
    }
}
