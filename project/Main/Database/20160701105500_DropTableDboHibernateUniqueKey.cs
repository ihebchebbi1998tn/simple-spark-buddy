namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160701105500)]
	public class DropTableDboHibernateUniqueKey : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hibernate_unique_key]')) 
																BEGIN
																	SELECT * INTO [dbo].[hibernate_unique_key_old] FROM [dbo].[hibernate_unique_key]
																	DROP TABLE [dbo].[hibernate_unique_key]
																END");
		}
	}
}