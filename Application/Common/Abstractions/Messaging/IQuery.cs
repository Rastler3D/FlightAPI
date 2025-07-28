using Domain.Common;

namespace Application.Common.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
        where TResponse : notnull
    { }

    public interface IQuery : IRequest<Result>
    { }
}
