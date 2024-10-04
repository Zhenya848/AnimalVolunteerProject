using PetProject.Application.Shared.Interfaces.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Commands.Delete
{
    public record DeleteFileCommand(string BucketName, string ObjectName) : IDeleteCommand;
}
