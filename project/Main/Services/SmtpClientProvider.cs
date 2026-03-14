namespace Main.Services
{
	extern alias SystemConfigurationConfigurationManager;
	using System.IO;
	using System.Xml;

	using ConfigurationManager = SystemConfigurationConfigurationManager::System.Configuration.ConfigurationManager;
	using ConfigurationUserLevel = SystemConfigurationConfigurationManager::System.Configuration.ConfigurationUserLevel;
	using ExeConfigurationFileMap = SystemConfigurationConfigurationManager::System.Configuration.ExeConfigurationFileMap;

	using MailKit.Net.Smtp;
	using MailKit.Security;

	using Main.Services.Interfaces;

	using MimeKit;

	using SystemConfigurationConfigurationManager::System.Configuration;

	public class SmtpClientProvider : ISmtpClientProvider
	{
		private readonly string deliveryMethod;
		private readonly string pickupDirectoryLocation;
		private readonly string from;
		private readonly string host;
		private readonly int? port;
		private readonly string userName;
		private readonly string password;
		private readonly bool? enableSsl;

		public SmtpClientProvider()
		{
			var mailSettings = GetMailSettings();
			deliveryMethod = GetValueFromXml(mailSettings,
				"smtp",
				"deliveryMethod");
			pickupDirectoryLocation = GetValueFromXml(mailSettings,
				"specifiedPickupDirectory",
				"pickupDirectoryLocation");
			from = GetValueFromXml(mailSettings,
				"smtp",
				"from");
			host = GetValueFromXml(mailSettings,
				"network",
				"host");
			var portString = GetValueFromXml(mailSettings,
				"network",
				"port");
			port = portString != null ? int.Parse(portString) : null;
			userName = GetValueFromXml(mailSettings,
				"network",
				"userName");
			password = GetValueFromXml(mailSettings,
				"network",
				"password");
			var enableSslString = GetValueFromXml(mailSettings,
				"network",
				"enableSsl");
			enableSsl = enableSslString != null ? bool.Parse(enableSslString) : null;
		}

		protected virtual XmlDocument GetMailSettings()
		{
			var configMap = new ExeConfigurationFileMap { ExeConfigFilename = "web.config" };
			var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
			var systemNet = config.GetSection("system.net").SectionInformation.GetRawXml();
			var mailSettings = new XmlDocument();
			using var xmlReader = XmlReader.Create(new StringReader(systemNet));
			mailSettings.Load(xmlReader);
			return mailSettings;
		}

		protected virtual string GetValueFromXml(XmlDocument document, string tagName, string attributeName)
		{
			var elements = document.GetElementsByTagName(tagName);
			if (elements.Count == 0)
			{
				return null;
			}

			return elements[0]?.Attributes?[attributeName]?.Value;
		}

		public virtual MimeMessage CreateMailMessage()
		{
			var mailMessage = new MimeMessage();

			if (from != null)
			{
				mailMessage.From.Clear();
				mailMessage.From.Add(MailboxAddress.Parse(from));
			}

			return mailMessage;
		}

		public virtual SmtpClient CreateSmtpClient()
		{
			if (deliveryMethod == "SpecifiedPickupDirectory")
			{
				return new PickupDirectorySmtpClient(pickupDirectoryLocation);
			}
			
			var smtpClient = new SmtpClient();

			smtpClient.Connect(host,
				port ?? 0,
				enableSsl == null || enableSsl == true ? SecureSocketOptions.Auto : SecureSocketOptions.None);

			if (userName != null || password != null)
			{
				smtpClient.Authenticate(userName, password);
			}

			return smtpClient;
		}
	}
}
