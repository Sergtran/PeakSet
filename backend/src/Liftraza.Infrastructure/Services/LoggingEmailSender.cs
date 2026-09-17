using Liftraza.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Liftraza.Infrastructure.Services;

/// <summary>
/// Placeholder delivery: writes the message to the application log instead of
/// sending it. Replace this registration with a real provider (Azure
/// Communication Services, SendGrid, SMTP) before opening the app to other users.
/// </summary>
public sealed class LoggingEmailSender : IEmailSender
{
	private readonly ILogger<LoggingEmailSender> _logger;

	public LoggingEmailSender(ILogger<LoggingEmailSender> logger)
		=> _logger = logger;

	public Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
	{
		_logger.LogWarning(
			"No email provider is configured. Message for {To} with subject {Subject}:\n{Body}",
			to, subject, body);

		return Task.CompletedTask;
	}
}
