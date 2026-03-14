namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using log4net;

	[Migration(20200715200000, UseTransaction = false)]
	public class AddIndexToPostingTransaction : Migration
	{
		private readonly ILog logger = LogManager.GetLogger(typeof(AddIndexToPostingTransaction));
		public override void Up()
		{
			using (var command = Database.GetCommand())
			{
				command.Transaction = null;
				command.CommandText = @"
					IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('[CRM].[Posting]') AND name = 'IX_Posting_Transaction')
					DROP INDEX [IX_Posting_Transaction] ON [CRM].[Posting]
					CREATE NONCLUSTERED INDEX [IX_Posting_Transaction] ON [CRM].[Posting] ([TransactionId] ASC) INCLUDE ([State])";
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
