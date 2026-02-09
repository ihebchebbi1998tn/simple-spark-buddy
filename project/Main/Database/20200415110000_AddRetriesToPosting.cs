namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using log4net;

	[Migration(20200415110000, UseTransaction = false)]
	public class AddRetriesToPosting : Migration
	{
		private readonly ILog logger = LogManager.GetLogger(typeof(AddRetriesToPosting));
		public override void Up()
		{
			using (var command = Database.GetCommand())
			{
				command.Transaction = null;
				command.CommandText = @"
					ALTER TABLE [CRM].[Posting] ADD RetryAfter datetime NULL;
					ALTER TABLE [CRM].[Posting] ADD Retries int NOT NULL DEFAULT 0;";
				command.CommandType = CommandType.Text;
				command.CommandTimeout = 300;
				try
				{
					command.ExecuteScalar();
				}
				catch (Exception e)
				{
					logger.Warn($"Query failed: {command.CommandText}", e);
					throw;
				}
			}
		}
	}
}
