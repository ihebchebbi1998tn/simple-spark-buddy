namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191119113300)]
	public class RemoveHungarianAsSystemLanguage : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE LU.[Language] SET IsSystemLanguage = 0 WHERE [Value] = 'hu'");
		}
	}
}
