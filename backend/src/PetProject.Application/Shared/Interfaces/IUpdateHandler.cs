using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Shared.Interfaces
{
    public interface IUpdateHandler<TCommand, TResult>
    {
        public Task<TResult> Update(TCommand command, CancellationToken cancellationToken = default);
    }
}
