using FluentValidation;
using PetProject.Application.Pets.UploadPhotos;
using PetProject.Domain.Shared;

namespace PetProject.API.Volunteers
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
