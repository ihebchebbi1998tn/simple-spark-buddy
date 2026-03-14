namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210809160000)]
	public class Unicore_Migrations : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[dbo].[_MigrationInfo]"))
			{
				return;
			}
			Database.ExecuteNonQuery(@"
				CREATE TABLE [dbo].[_MigrationInfo] (
				[Version] [bigint] NOT NULL,
				[InstalledAt] [datetime] NULL,
				[Description] [nvarchar](1024) NULL
			)");
			Database.ExecuteNonQuery(@"
				INSERT INTO [dbo].[_MigrationInfo] ([Version])
				VALUES (20180108173600), (20180112113600), (20190807173600)"); //do not run these
		}
	}
}
