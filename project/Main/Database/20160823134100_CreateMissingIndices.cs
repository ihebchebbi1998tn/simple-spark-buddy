namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160823134100)]
	public class CreateMissingIndices : Migration
	{
		public override void Up()
		{
			if (!Database.IndexExists("[CRM].[User]", "IX_User_Discharged"))
			{
				Database.ExecuteNonQuery(@"
					CREATE NONCLUSTERED INDEX [IX_User_Discharged] ON [CRM].[User]([Discharged] ASC)
				");
			}
		}
	}
}