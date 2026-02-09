namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211206113100)]
	public class SetLanguageDefaultLocale : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE [LU].[Language] SET [DefaultLocale] = 'de' WHERE [Value] = 'de' AND [DefaultLocale] IS NULL");
			Database.ExecuteNonQuery("UPDATE [LU].[Language] SET [DefaultLocale] = 'en-GB' WHERE [Value] = 'en' AND [DefaultLocale] IS NULL");
			Database.ExecuteNonQuery("UPDATE [LU].[Language] SET [DefaultLocale] = 'es' WHERE [Value] = 'es' AND [DefaultLocale] IS NULL");
			Database.ExecuteNonQuery("UPDATE [LU].[Language] SET [DefaultLocale] = 'fr' WHERE [Value] = 'fr' AND [DefaultLocale] IS NULL");
			Database.ExecuteNonQuery("UPDATE [LU].[Language] SET [DefaultLocale] = 'hu' WHERE [Value] = 'hu' AND [DefaultLocale] IS NULL");
		}
	}
}
