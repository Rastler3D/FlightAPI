namespace Application.Common.Abstractions
{
    public interface IRequestBase { }
    public interface IQuery<TResponse> : IRequestBase, IRequest<Result<TResponse,IDomainError>>
        where TResponse : notnull
    { }

    public interface IQuery : IRequestBase, IRequest<Result>
    { }
}
