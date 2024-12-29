using FluentValidation;
using PetProject.Core;
using PetProject.Core.Application.Validation;
using PetProject.Core.ValueObjects;
using PetProject.Volunteers.Application.Volunteers.Commands.Update;
using PetProject.Volunteers.Domain.ValueObjects;

namespace PetProject.Volunteers.Application.Validators
{
    public class UpdateVolunteerCommandValidator : AbstractValidator<UpdateVolunteerCommand>
    {
        public UpdateVolunteerCommandValidator()
        {
            RuleFor(i => i.VolunteerId).NotEmpty().WithError(Errors.General.Failure("id"));

            RuleFor(n => n.Name).MustBeValueObject(n =>
            FullName.Create(n.firstName, n.lastName, n.patronymic ?? ""));

            RuleFor(d => d.Description).MustBeValueObject(Description.Create);
            RuleFor(pn => pn.PhoneNumber).MustBeValueObject(TelephoneNumber.Create);
            RuleFor(e => e.Experience).MustBeValueObject(Experience.Create);
        }
    }
}
