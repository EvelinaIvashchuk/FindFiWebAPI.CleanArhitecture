using MediatR;

namespace FindFi.CL.Application.Common.CQRS;

/// <summary>
/// Маркерний інтерфейс для команд без відповіді.
/// </summary>
public interface ICommand : IRequest { }

/// <summary>
/// Маркерний інтерфейс для команд з відповіддю.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse> { }
