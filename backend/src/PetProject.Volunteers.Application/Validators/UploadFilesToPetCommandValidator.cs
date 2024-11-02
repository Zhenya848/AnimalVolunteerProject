using FluentValidation;
using PetProject.Core;
using PetProject.Core.Application.Validation;
using PetProject.Volunteers.Application.Pets.Commands.UploadPhotos;

namespace PetProject.Volunteers.Application.Validators
{
    public class UploadFilesToPetCommandValidator : AbstractValidator<UploadFilesToPetCommand>
    {
        public UploadFilesToPetCommandValidator()
        {
            RuleFor(i => i.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired("Volunteer id"));
            RuleFor(i => i.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired("Pet id"));

            RuleFor(f => f.Files).NotEmpty().WithError(Errors.General.ValueIsRequired("File list"));
            RuleForEach(f => f.Files).SetValidator(new UploadFileDtoValidator());
        }
    }
}
