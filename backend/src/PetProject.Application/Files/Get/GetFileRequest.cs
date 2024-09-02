using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Get
{
    public record GetFileRequest(string BucketName, string ObjectName);
}
