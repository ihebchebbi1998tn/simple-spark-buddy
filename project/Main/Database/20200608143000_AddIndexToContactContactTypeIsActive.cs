namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using log4net;

	[Migration(20200608143000, UseTransaction = false)]
	public class AddIndexToContactContactTypeIsActive : Migration
	{
		private readonly ILog logger = LogManager.GetLogger(typeof(AddIndexToContactContactTypeIsActive));
		public override void Up()
		{
			using (var command = Database.GetCommand())
			{
				command.Transaction = null;
				command.CommandText = @"
					IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('[CRM].[Contact]') AND name = 'IX_Contact_ContactType_IsActive')
					DROP INDEX [IX_Contact_ContactType_IsActive] ON [CRM].[Contact]
					CREATE NONCLUSTERED INDEX IX_Contact_ContactType_IsActive ON [CRM].[Contact] ([ContactType], [IsActive]) INCLUDE ([ModifyDate], [ContactId])";
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
