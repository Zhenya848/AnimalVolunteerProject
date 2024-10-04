using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Cryptography.Xml;

namespace PetProject.Infastructure.Extensions
{
    public static class EfCoreApiExtensions
    {
        public static PropertyBuilder<IReadOnlyList<TValueObject>> ValueToDtoConversion<TValueObject, TDto>(
            this PropertyBuilder<IReadOnlyList<TValueObject>> builder,
            Func<TValueObject, TDto> convertor, 
            Func<TDto, TValueObject> selector)
        {
            return builder.HasConversion(

                valueObjects => SerializeToDto(valueObjects, convertor),
                json => DeserializeToValueObject(json, selector),

                new ValueComparer<IReadOnlyList<TValueObject>>(
                    (c1, c2) => c1!.Equals(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                    c => c.ToList()))

            .HasColumnType("jsonb");
        }

        private static string SerializeToDto<TValueObject, TDto>(
            IReadOnlyList<TValueObject> valueObjects,
            Func<TValueObject, TDto> convertor)
        {
            return JsonSerializer.Serialize(valueObjects.Select(convertor), JsonSerializerOptions.Default);
        }

        private static IReadOnlyList<TValueObject> DeserializeToValueObject<TValueObject, TDto>(
            string expression,
            Func<TDto, TValueObject> selector)
        {
            var dtos = JsonSerializer.Deserialize<IEnumerable<TDto>>(expression, JsonSerializerOptions.Default) ?? [];

            return dtos.Select(selector).ToList();
        }
    }
}
