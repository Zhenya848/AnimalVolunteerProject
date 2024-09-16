﻿using PetProject.Domain.Shared.ValueObjects.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Pets.UploadPhotos
{
    public record UploadFilesToPetCommand(
        Guid VolunteerId, 
        Guid PetId, 
        IEnumerable<UploadFileDto> Files);
}
