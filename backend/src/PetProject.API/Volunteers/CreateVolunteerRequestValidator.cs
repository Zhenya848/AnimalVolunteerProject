using FluentValidation;
using PetProject.Application.Volunteers.Create;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.API.Volunteers
{
    public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
    {
        public CreateVolunteerRequestValidator()
        {
            RuleFor(n => new { n.firstname, n.lastName, n.patronymic }).MustBeValueObject(n =>
            FullName.Create(n.firstname, n.lastName, n.patronymic ?? ""));

            RuleFor(d => d.description).MustBeValueObject(Description.Create);
            RuleFor(pn => pn.phoneNumber).MustBeValueObject(TelephoneNumber.Create);
            RuleFor(e => e.exp).MustBeValueObject(Experience.Create);

            RuleForEach(r => r.requisites).MustBeValueObject(r => Requisite.Create(r.title, r.description));
            RuleForEach(sn => sn.sotialNetworks).MustBeValueObject(sn => SocialNetwork.Create(sn.name, sn.reference));
        }
    }
}
