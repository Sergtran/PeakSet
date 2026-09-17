namespace Liftraza.Application.Exceptions;

public sealed record ValidationError(string Code, string Message);
