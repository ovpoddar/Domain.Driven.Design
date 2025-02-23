namespace DDD.Application.Exceptions;

/// <summary>
/// this validation is not for Application use. it's Application internal exception
/// which will trigger if model validation failed. and it will change the current 
/// response to <see cref="HttpStatusCode.BadRequest"/>
/// </summary>
public sealed class BaseValidationException : Exception
{
    public IReadOnlyCollection<ValidationError> Errors { get; }
    public BaseValidationException(List<ValidationError> errors)
        : base("Validation Failed Exception.")
    {
        Errors = errors;
    }
}

public record ValidationError(string ProprietyName, string ErrorMessage);
