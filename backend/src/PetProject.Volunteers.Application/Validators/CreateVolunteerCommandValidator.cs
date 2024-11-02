using FluentValidation;
using PetProject.Core.Application.Validation;
using PetProject.Core.ValueObjects;
using PetProject.Volunteers.Application.Volunteers.Commands.Create;
using PetProject.Volunteers.Domain.ValueObjects;

namespace PetProject.Volunteers.Application.Validators
{
    public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
    {
        public CreateVolunteerCommandValidator()
        {
            RuleFor(n => n.Name).MustBeValueObject(n =>
            FullName.Create(n.firstName, n.lastName, n.patronymic ?? ""));

            RuleFor(d => d.Description).MustBeValueObject(Description.Create);
            RuleFor(pn => pn.PhoneNumber).MustBeValueObject(TelephoneNumber.Create);
            RuleFor(e => e.Experience).MustBeValueObject(Experience.Create);

            RuleForEach(r => r.Requisites).MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
            RuleForEach(sn => sn.SocialNetworks).MustBeValueObject(sn => SocialNetwork.Create(sn.Name, sn.Reference));
        }
    }
}