using System.Data;
using System.Text.Json;
using Dapper;

namespace PetProject.Core.Infrastructure.Dapper;

public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = JsonSerializer.Serialize(value);
    }

    public override T? Parse(object value)
    {
        if (value is string str)
            return JsonSerializer.Deserialize<T>(str, JsonSerializerOptions.Default);

        return default;
    }
}