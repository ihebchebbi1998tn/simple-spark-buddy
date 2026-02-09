namespace Main.Services.Interfaces
{
	using Crm.Library.AutoFac;

	using MailKit.Net.Smtp;

	using MimeKit;

	public interface ISmtpClientProvider : ISingletonDependency
	{
		public MimeMessage CreateMailMessage();
		public SmtpClient CreateSmtpClient();
	}
}
