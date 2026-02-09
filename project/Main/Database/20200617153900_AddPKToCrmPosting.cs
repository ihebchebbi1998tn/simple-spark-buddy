namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using log4net;

	[Migration(20200617153900, UseTransaction = false)]
	public class AddPKToCrmPosting : Migration
	{
		private readonly ILog logger = LogManager.GetLogger(typeof(AddPKToCrmPosting));
		public override void Up()
		{
			using (var command = Database.GetCommand())
			{
				command.Transaction = null;
				command.CommandText = @"
					IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [type] = 'PK' AND [parent_object_id] = OBJECT_ID('CRM.Posting'))
					BEGIN
						ALTER TABLE CRM.Posting add CONSTRAINT [PK_Posting] PRIMARY KEY CLUSTERED 
						([PostingId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
					END;";
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
