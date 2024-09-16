using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Delete
{
    public record DeleteFileCommand(string BucketName, string ObjectName);
}
