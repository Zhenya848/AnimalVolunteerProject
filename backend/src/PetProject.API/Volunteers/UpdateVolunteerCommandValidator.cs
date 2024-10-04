using FluentValidation;
using PetProject.Application.Volunteers.UseCases.Update;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.API.Volunteers
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

            RuleForEach(r => r.Requisites).MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
            RuleForEach(sn => sn.SocialNetworks).MustBeValueObject(sn => SocialNetwork.Create(sn.Name, sn.Reference));
        }
    }
}
