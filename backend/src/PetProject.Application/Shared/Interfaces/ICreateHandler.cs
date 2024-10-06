using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Shared.Interfaces
{
    public interface ICreateHandler<TCommand, TResult>
    {
        public Task<TResult> Create(TCommand command, CancellationToken cancellationToken = default);
    }
}
