using System.Data;

namespace PetProject.Core.Application.Abstractions
{
    public interface ISqlConnectionFactory
    {
        public IDbConnection Create();
    }
}
