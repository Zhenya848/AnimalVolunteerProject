using FluentValidation;
using PetProject.Application.Volunteers.Pets.Commands.Update;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.API.Volunteers
{
    public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
    {
        public UpdatePetCommandValidator() 
        {
            RuleFor(i => i.PetId).NotEmpty().WithError(Errors.General.Failure("id"));

            RuleFor(n => n.Name).NotEmpty().WithError(Errors.General.ValueIsRequired("Name"));
            RuleFor(c => c.Color).NotEmpty().WithError(Errors.General.ValueIsRequired("Color"));
            RuleFor(c => c.HealthInfo).NotEmpty().WithError(Errors.General.ValueIsRequired("Health info"));

            RuleFor(w => w.Weight).Must(w => w > 0).WithError(Errors.General.ValueIsInvalid("Weight"));
            RuleFor(w => w.Height).Must(h => h > 0).WithError(Errors.General.ValueIsInvalid("Height"));

            RuleFor(bt => bt.BirthdayTime).NotEmpty().NotNull().WithError(Errors.General.ValueIsRequired("Birthday time"));
            RuleFor(bt => bt.DateOfCreation).NotEmpty().NotNull().WithError(Errors.General.ValueIsRequired("Date of creation"));

            RuleFor(d => d.Description).MustBeValueObject(Description.Create);
            RuleFor(t => t.TelephoneNumber).MustBeValueObject(TelephoneNumber.Create);

            RuleFor(a => a.Addres).MustBeValueObject(a => Address.Create(a.Street, a.City, a.State, a.ZipCode));

            RuleForEach(r => r.Requisites).MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
        }
    }
}
