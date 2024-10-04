using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Files.Commands.Create
{
    public record FileData(Stream Stream, string ObjectName);
}
