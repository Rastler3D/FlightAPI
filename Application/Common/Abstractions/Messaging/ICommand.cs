using Domain.Common;

namespace Application.Common.Abstractions.Messaging
{
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
        where TResponse : notnull
    { }

    public interface ICommand : IRequest<Result> { }
}
