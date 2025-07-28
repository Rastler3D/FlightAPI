using Domain.Common;

namespace Application.Common.Abstractions.Messaging
{
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
       where TRequest : IQuery<TResponse>
       where TResponse : notnull
    { }
}
