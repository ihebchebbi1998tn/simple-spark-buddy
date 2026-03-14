namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using log4net;

	[Migration(20200415100000, UseTransaction = false)]
	public class AddIndexToPostingState : Migration
	{
		private readonly ILog logger = LogManager.GetLogger(typeof(AddIndexToPostingState));
		public override void Up()
		{
			using (var command = Database.GetCommand())
			{
				command.Transaction = null;
				command.CommandText = @"
					IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('[CRM].[Posting]') AND name = 'IX_Posting_State')
					DROP INDEX [IX_Posting_State] ON [CRM].[Posting]
					CREATE NONCLUSTERED INDEX [IX_Posting_State] ON [CRM].[Posting] ([State] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)";
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
