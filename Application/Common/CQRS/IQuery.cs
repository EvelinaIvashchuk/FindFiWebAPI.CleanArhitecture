using MediatR;

namespace FindFi.CL.Application.Common.CQRS;

/// <summary>
/// Маркерний інтерфейс для запитів з відповіддю.
/// </summary>
public interface IQuery<out TResponse> : IRequest<TResponse> { }
