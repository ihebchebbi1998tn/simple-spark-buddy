namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200617150700)]
	public class AddPKToSmsServiceObjects : Migration
	{
		public override void Up()
		{
			var query = @"
				IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [type] = 'PK' AND [parent_object_id] = OBJECT_ID('SMS.ServiceObject'))
				BEGIN
					ALTER TABLE [SMS].[ServiceObject] ADD  CONSTRAINT [PK_ServiceObject] PRIMARY KEY CLUSTERED
					([ContactKey] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				END;";

			Database.ExecuteNonQuery(query);
		}
	}
}