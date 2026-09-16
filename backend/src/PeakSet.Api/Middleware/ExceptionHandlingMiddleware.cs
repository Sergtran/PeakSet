using PeakSet.Application.Exceptions;
using PeakSet.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace PeakSet.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception exception)
		{
			await HandleExceptionAsync(context, exception);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var (statusCode, title, errors) = exception switch
		{
			ValidationException validation =>
				(StatusCodes.Status400BadRequest, "Validation failed", validation.Errors),
			InvalidCredentialsException =>
				(StatusCodes.Status401Unauthorized, "Invalid credentials",
					Single("InvalidCredentials", "Incorrect email or password.")),
			DomainException domain =>
				(StatusCodes.Status400BadRequest, "Business rule violation",
					Single("DomainRule", domain.Message)),
			NotFoundException notFound =>
				(StatusCodes.Status404NotFound, "Not found",
					Single("NotFound", notFound.Message)),
			_ =>
				(StatusCodes.Status500InternalServerError, "An unexpected error occurred",
					Single("Unexpected", "An unexpected error occurred."))
		};

		if (statusCode == StatusCodes.Status500InternalServerError)
			_logger.LogError(exception, "Unhandled exception");

		context.Response.StatusCode = statusCode;
		context.Response.ContentType = "application/problem+json";

		var problem = new ProblemDetails
		{
			Status = statusCode,
			Title = title,
			Detail = string.Join(" ", errors.Select(error => error.Message))
		};

		problem.Extensions["errors"] = errors;

		await context.Response.WriteAsJsonAsync(problem, context.RequestAborted);
	}

	private static IReadOnlyCollection<ValidationError> Single(string code, string message)
		=> new[] { new ValidationError(code, message) };
}
