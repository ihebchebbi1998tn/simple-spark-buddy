namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191221142100)]
	public class AddPrimaryKeyToLog : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [type] = 'PK' AND [parent_object_id] = OBJECT_ID('CRM.Log')) BEGIN
					ALTER TABLE[CRM].[Log] ADD CONSTRAINT[PK_Log] PRIMARY KEY CLUSTERED
					([Id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON[PRIMARY]
				END");
		}
	}
}
