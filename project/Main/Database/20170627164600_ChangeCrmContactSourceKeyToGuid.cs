namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170627164600)]
	public class ChangeCrmContactSourceKeyToGuid : Migration
	{

		public override void Up()
		{
			if((int)Database.ExecuteScalar(@"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Contact' AND COLUMN_NAME='SourceKey' AND DATA_TYPE = 'int'") > 0)
			{
				Database.ExecuteNonQuery(@"EXEC sp_rename '[CRM].[Contact].[SourceKey]', 'SourceKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[Contact] ADD [SourceKey] uniqueidentifier NULL");
			}
		}
	}
}