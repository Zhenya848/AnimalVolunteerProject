using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Domain.Shared.ValueObjects.Dtos
{
    public record UploadFileDto(string FileName, string ContentType, Stream Stream);
}
