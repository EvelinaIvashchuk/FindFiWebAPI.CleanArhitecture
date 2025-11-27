namespace FindFi.CL.Application.Common.Exceptions;

/// <summary>
/// Конфлікт стану/даних (HTTP 409). Наприклад, дубль ключа або конкуренція оновлень.
/// </summary>
public sealed class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
