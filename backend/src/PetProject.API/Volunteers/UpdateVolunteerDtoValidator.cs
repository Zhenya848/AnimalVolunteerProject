using FluentValidation;
using PetProject.Application.Volunteers.Update;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.API.Volunteers
{
    public class UpdateVolunteerDtoValidator : AbstractValidator<UpdateVolunteerDto>
    {
        public UpdateVolunteerDtoValidator() 
        {
            RuleFor(n => n.name).MustBeValueObject(n =>
            FullName.Create(n.firstName, n.lastName, n.patronymic ?? ""));

            RuleFor(d => d.description).MustBeValueObject(Description.Create);
            RuleFor(pn => pn.phoneNumber).MustBeValueObject(TelephoneNumber.Create);
            RuleFor(e => e.experience).MustBeValueObject(Experience.Create);

            RuleForEach(r => r.requisites).MustBeValueObject(r => Requisite.Create(r.title, r.description));
            RuleForEach(sn => sn.sotialNetworks).MustBeValueObject(sn => SocialNetwork.Create(sn.name, sn.reference));
        }    
    }
}
