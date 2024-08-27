using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Response;
using PetProject.Application.Volunteers.Create;
using PetProject.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.API
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder, Func<TElement, Result<TValueObject, Error>> factoryMethod)
        {
            return ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error.Serialize());
            });
        }

        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, Error error)
        {
            return rule.WithMessage(error.Serialize());
        }
    }
}