namespace Application.Common.Abstractions.Messaging;


public interface ICachedQuery<TResponse> : ICachedQuery, IQuery<TResponse> where TResponse : notnull;

public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? CacheExpiration { get; }
}