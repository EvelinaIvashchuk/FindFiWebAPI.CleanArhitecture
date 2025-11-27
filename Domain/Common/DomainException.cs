namespace FindFi.CL.Domain.Common;

/// <summary>
/// Базове доменне виключення для порушень бізнес-правил.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
