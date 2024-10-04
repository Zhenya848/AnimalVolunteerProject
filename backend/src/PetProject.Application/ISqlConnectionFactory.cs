using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application
{
    public interface ISqlConnectionFactory
    {
        public IDbConnection Create();
    }
}
