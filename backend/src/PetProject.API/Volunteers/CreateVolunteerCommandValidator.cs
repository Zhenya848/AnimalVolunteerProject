using FluentValidation;
using PetProject.Application.Volunteers.Create;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.API.Volunteers
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

            RuleForEach(r => r.Requisites).MustBeValueObject(r => Requisite.Create(r.title, r.description));
            RuleForEach(sn => sn.SocialNetworks).MustBeValueObject(sn => SocialNetwork.Create(sn.name, sn.reference));
        }
    }
}