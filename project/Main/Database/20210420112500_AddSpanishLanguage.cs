namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210420112500)]
	public class AddSpanishLanguage : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[Language]"))
			{
				InsertLookupValue("Español", "es", "es", "0");
				
				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[Language] WHERE [Value] = 'en' AND [IsActive] = 1 ") > 0)
				{
					InsertLookupValue("Spanish", "en", "es", "0");
					InsertLookupValue("Inglés", "es", "en", "(CASE WHEN EXISTS (SELECT TOP 1 NULL FROM [LU].[Language] WHERE [Value] = 'en' AND IsSystemLanguage = 1) THEN 1 ELSE 0 END)");
				}

				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[Language] WHERE [Value] = 'de' AND [IsActive] = 1 ") > 0)
				{
					InsertLookupValue("Spanisch", "de", "es", "0");
					InsertLookupValue("Alemán", "es", "de", "(CASE WHEN EXISTS (SELECT TOP 1 NULL FROM [LU].[Language] WHERE [Value] = 'de' AND IsSystemLanguage = 1) THEN 1 ELSE 0 END)");
				}

				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[Language] WHERE [Value] = 'fr' AND [IsActive] = 1 ") > 0)
				{
					InsertLookupValue("Espagnol", "fr", "es", "0");
					InsertLookupValue("Francés", "es", "fr", "(CASE WHEN EXISTS (SELECT TOP 1 NULL FROM [LU].[Language] WHERE [Value] = 'fr' AND IsSystemLanguage = 1) THEN 1 ELSE 0 END)");
				}

				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[Language] WHERE [Value] = 'hu' AND [IsActive] = 1 ") > 0)
				{
					InsertLookupValue("Spanyol", "hu", "es", "0");
					InsertLookupValue("Húngaro", "es", "hu", "(CASE WHEN EXISTS (SELECT TOP 1 NULL FROM [LU].[Language] WHERE [Value] = 'hu' AND IsSystemLanguage = 1) THEN 1 ELSE 0 END)");
				}
			}
		}
		private void InsertLookupValue(string name, string language, string value, string isSystemLanguage)
		{
			Database.ExecuteNonQuery($"INSERT INTO [LU].[Language] (Name, Language, Value, Favorite, SortOrder, IsSystemLanguage) VALUES ('{name}', '{language}', '{value}', 0, 0, {isSystemLanguage})");
		}
	}
}
