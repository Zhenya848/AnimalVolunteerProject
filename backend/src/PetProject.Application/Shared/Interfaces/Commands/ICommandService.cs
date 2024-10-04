using CSharpFunctionalExtensions;
using PetProject.Application.Volunteers.UseCases.Create;
using PetProject.Application.Volunteers.UseCases.Delete;
using PetProject.Application.Volunteers.UseCases.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Shared.Interfaces.Commands
{
    public interface ICommandService<TResponse, TCreateCommand, TDeleteCommand, TUpdateCommand>
        where TCreateCommand : ICreateCommand
        where TDeleteCommand : IDeleteCommand
        where TUpdateCommand : IUpdateCommand
    {
        Task<TResponse> Create(
            TCreateCommand request,
            CancellationToken cancellationToken = default);

        Task<TResponse> Delete(
            TDeleteCommand request,
            CancellationToken cancellationToken = default);
        Task<TResponse> Update(
            TUpdateCommand request,
            CancellationToken cancellationToken = default);
    }
}
