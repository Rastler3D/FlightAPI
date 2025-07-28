using Domain.Common;

namespace Application.Common.Abstractions.Messaging
{
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
        where TRequest : ICommand<TResponse>
        where TResponse : notnull
    { }

    public interface ICommandHandler<TRequest> : IRequestHandler<TRequest, Result>
        where TRequest : ICommand
    { }
}
