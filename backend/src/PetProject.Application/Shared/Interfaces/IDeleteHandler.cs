using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Shared.Interfaces
{
    public interface IDeleteHandler<TCommand, TResult>
    {
        public Task<TResult> Delete(TCommand command, CancellationToken cancellationToken = default);
    }
}
