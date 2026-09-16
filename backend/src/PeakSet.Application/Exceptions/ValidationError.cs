namespace PeakSet.Application.Exceptions;

public sealed record ValidationError(string Code, string Message);
