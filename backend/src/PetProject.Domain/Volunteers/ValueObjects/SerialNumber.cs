using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Domain.Volunteers.ValueObjects
{
    public record SerialNumber
    {
        public static SerialNumber First = new SerialNumber(1);
        public int Value { get; }

        private SerialNumber(int value)
        {
            Value = value;
        }

        public static Result<SerialNumber, Error> Create(int number)
        {
            if (number <= 0)
                return Errors.General.ValueIsInvalid("Serial number");

            return new SerialNumber(number);
        }

        public static implicit operator int(SerialNumber serialNumber) =>
            serialNumber.Value;
    }
}
