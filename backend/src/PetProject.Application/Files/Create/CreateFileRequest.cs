using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Create
{
    public record CreateFileRequest(string BucketName, string ObjectName, Stream Stream);
}
