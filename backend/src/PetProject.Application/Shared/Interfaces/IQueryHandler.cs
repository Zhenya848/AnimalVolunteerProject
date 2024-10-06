using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Shared.Interfaces
{
    public interface IQueryHandler<TQuery, TResult>
    {
        public Task<TResult> Get(TQuery query, CancellationToken cancellationToken = default);
    }
}
