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
				(StatusCodes.Status401Unauthorized, "Invalid credentials", null),
			DomainException =>
				(StatusCodes.Status400BadRequest, "Business rule violation", null),
			NotFoundException =>
				(StatusCodes.Status404NotFound, "Not found", null),
			_ =>
				(StatusCodes.Status500InternalServerError, "An unexpected error occurred", null)
		};

		if (statusCode == StatusCodes.Status500InternalServerError)
			_logger.LogError(exception, "Unhandled exception");

		context.Response.StatusCode = statusCode;
		context.Response.ContentType = "application/problem+json";

		var problem = new ProblemDetails
		{
			Status = statusCode,
			Title = title,
			Detail = exception.Message
		};

		if (errors is not null)
			problem.Extensions["errors"] = errors;

		await context.Response.WriteAsJsonAsync(problem, context.RequestAborted);
	}
}