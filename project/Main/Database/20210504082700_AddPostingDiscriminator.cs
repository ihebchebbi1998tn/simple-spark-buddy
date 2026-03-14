namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using log4net;

	[Migration(20210504082700, UseTransaction = false)]
	public class AddPostingDiscriminator : Migration
	{
		private readonly ILog logger = LogManager.GetLogger(typeof(AddPostingDiscriminator));
		public override void Up()
		{
			if (!Database.ColumnExists("CRM.Posting", "Category"))
			{
				using (var command = Database.GetCommand())
				{
					command.Transaction = null;
					command.CommandText = @"
					ALTER TABLE [CRM].[Posting] ADD Category nvarchar(30) NOT NULL DEFAULT 'Posting';";
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

			if (Database.ColumnExists("CRM.Posting", "Category"))
			{
				var statement = "UPDATE [CRM].[Posting] SET [Category] = 'Posting';";
				Database.ExecuteNonQuery(statement);
			}
		}
	}
}
