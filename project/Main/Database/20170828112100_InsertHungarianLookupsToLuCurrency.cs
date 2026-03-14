namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170828112100)]
	public class InsertHungarianLookupsToLuCurrency : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("select COUNT(*) from [LU].[Currency] where [Language] = 'hu'") > 0)
			{
				Database.ExecuteNonQuery(@"UPDATE [LU].[Currency]
																	SET Name = CASE Value 
																		WHEN 'EUR' THEN (SELECT TOP 1 Name FROM [LU].[Currency] WHERE Value = 'EUR' AND [Language] = 'en')
																		WHEN 'USD' THEN (SELECT TOP 1 Name FROM [LU].[Currency] WHERE Value = 'USD' AND [Language] = 'en') 
																		WHEN 'GBP' THEN (SELECT TOP 1 Name FROM [LU].[Currency] WHERE Value = 'GBP' AND [Language] = 'en') 
																	END, ModifyUser = 'Migration_201708281121', ModifyDate = GETUTCDATE() where [Language] = 'hu'");
			}
			else
			{
				InsertLookupValue("EUR", "€", "hu", "0", "0");
				InsertLookupValue("USD", "$", "hu", "0", "1");
				InsertLookupValue("GBP", "£", "hu", "0", "2");
			}
		}

		private void InsertLookupValue(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.ExecuteNonQuery($"INSERT INTO [LU].[Currency] ([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [CreateUser], [ModifyDate], [ModifyUser]) VALUES ('{value}', '{name}', '{language}', '{favorite}', '{sortOrder}', GETUTCDATE(), 'Migration_201708281121', GETUTCDATE(), 'Migration_201708281121')");
		}
	}
}