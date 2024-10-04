using PetProject.Application.Shared.Interfaces.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Commands.Update
{
    public record UpdateFileCommand() : IUpdateCommand;
}
