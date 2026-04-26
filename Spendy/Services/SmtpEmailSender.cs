using System.Net;
using System.Net.Mail;

namespace Spendy.Services;

/// <summary>
/// Minimal SMTP sender. Configure via environment variables:
/// SPENDY_SMTP_HOST, SPENDY_SMTP_PORT, SPENDY_SMTP_USER, SPENDY_SMTP_PASS, SPENDY_SMTP_FROM.
/// </summary>
public sealed class SmtpEmailSender : IEmailSender
{
	static string GetRequired(string key) =>
		Environment.GetEnvironmentVariable(key)
		?? throw new InvalidOperationException($"Missing environment variable: {key}");

	public async Task SendAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
	{
		// System.Net.Mail doesn't accept CancellationToken, but we keep the interface consistent.
		_ = cancellationToken;

		var host = GetRequired("SPENDY_SMTP_HOST");
		var portRaw = Environment.GetEnvironmentVariable("SPENDY_SMTP_PORT") ?? "587";
		if (!int.TryParse(portRaw, out var port))
			port = 587;
		var user = GetRequired("SPENDY_SMTP_USER");
		var pass = GetRequired("SPENDY_SMTP_PASS");
		var from = GetRequired("SPENDY_SMTP_FROM");

		using var msg = new MailMessage(from, toEmail, subject, body);
		using var client = new SmtpClient(host, port)
		{
			EnableSsl = true,
			Credentials = new NetworkCredential(user, pass)
		};

		await client.SendMailAsync(msg);
	}
}

