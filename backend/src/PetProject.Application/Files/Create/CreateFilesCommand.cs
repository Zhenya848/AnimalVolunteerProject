using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Create
{
    public record CreateFilesCommand(IEnumerable<FileData> Files, string BucketName);
}
