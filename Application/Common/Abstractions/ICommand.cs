﻿namespace Application.Common.Abstractions
{
    public interface ICommand<TResponse> : IRequestBase, IRequest<Result<TResponse,IDomainError>>
        where TResponse : notnull
    { }

    public interface ICommand : IRequestBase, IRequest<Result<Unit>> { }
}
