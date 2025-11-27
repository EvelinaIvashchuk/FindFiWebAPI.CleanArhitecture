namespace FindFi.CL.Application.Common.Exceptions;

/// <summary>
/// Виняток для ситуацій, коли ресурс не знайдено (HTTP 404).
/// </summary>
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
