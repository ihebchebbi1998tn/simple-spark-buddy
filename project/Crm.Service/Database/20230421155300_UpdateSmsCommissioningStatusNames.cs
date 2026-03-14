namespace Crm.Service.Database
{
	using System.Collections.Generic;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230421155300)]
	public class UpdateSmsCommissioningStatusNames : Migration
	{
		public override void Up()
		{
			var rows = new List<string[]>
			{
				new[] { "0", "de", "Keine Kommissionierung erforderlich" },
				new[] { "0", "en", "No picking required" },
				new[] { "0", "fr", "Pas de prélèvement nécessaire" },
				new[] { "0", "es", "No precisa preparación" },
				new[] { "0", "hu", "Nincs szükség komissiózásra" },

				new[] { "1", "de", "Kommissionierung ausstehend" },
				new[] { "1", "en", "Picking pending" },
				new[] { "1", "fr", "Prélèvement en attente" },
				new[] { "1", "es", "Preparación pendiente" },
				new[] { "1", "hu", "Komissiózás függőben" },

				new[] { "2", "de", "Teilweise kommissioniert" },
				new[] { "2", "en", "Partially picked" },
				new[] { "2", "fr", "Partiellement prélevé" },
				new[] { "2", "es", "Preparado parcialmente" },
				new[] { "2", "hu", "Részben komissiózva" },

				new[] { "3", "de", "Kommissioniert" },
				new[] { "3", "en", "Picked" },
				new[] { "3", "fr", "Prélevé" },
				new[] { "3", "es", "Preparado" },
				new[] { "3", "hu", "Komissiózva" }
			};

			foreach (var row in rows)
			{
				UpdateOrInsertSmsCommissioningStatus(row[0],row[1], row[2]);
			}
		}

		private void UpdateOrInsertSmsCommissioningStatus(string value, string language, string name)
		{
			if ((int)Database.ExecuteScalar($"SELECT COUNT(*) FROM [SMS].[CommissioningStatus] WHERE [Value] = {value} and [Language] = '{language}'") == 1)
			{
				var query = $@"UPDATE [SMS].[CommissioningStatus]
				SET [Name] = '{name}', [ModifyDate] = GETUTCDATE(), [ModifyUser] = 'Migration_20230421155300'
				WHERE [Language] = '{language}' and [Value] = {value}";

				Database.ExecuteNonQuery(query);
			}
			else
			{
				var query = $@"INSERT INTO [SMS].[CommissioningStatus] ([Name],[Language],[Value],[Favorite],[SortOrder],[CreateUser],[ModifyUser],[IsActive])
				VALUES ('{name}','{language}',{value},0,0,'Migration_20230421155300','Migration_20230421155300',1)";

				Database.ExecuteNonQuery(query);
			}
		}
	}
}
