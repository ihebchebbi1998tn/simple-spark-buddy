namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240829153000)]
	public class AddPasswordStrengthTable : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[PasswordStrength]"))
			{
				Database.AddTable(
					"[LU].[PasswordStrength]",
					new Column($"PasswordStrengthId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.StringFixedLength, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.Null),
					new Column("Value", DbType.String, ColumnProperty.NotNull),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, 1)
				);
			}

			AddPasswordStrength("0",
				"Too easy to guess",
				"Zu leicht zu erraten",
				"Trop facile à deviner",
				"Demasiado fácil de adivinar",
				"Túl könnyű kitalálni");
			AddPasswordStrength("1",
				"Easy to guess",
				"Leicht zu erraten",
				"Facile à deviner",
				"Fácil de adivinar",
				"Könnyű kitalálni");
			AddPasswordStrength("2",
				"Moderately hard to guess",
				"Mäßig schwer zu erraten",
				"Moyennement difficile à deviner",
				"Moderadamente difícil de adivinar",
				"Közepesen nehéz kitalálni");
			AddPasswordStrength("3",
				"Difficult to guess",
				"Schwierig zu erraten",
				"Difficile à deviner",
				"Difícil de adivinar",
				"Nehéz kitalálni");
			AddPasswordStrength("4",
				"Very difficult to guess",
				"Sehr schwer zu erraten",
				"Très difficile à deviner",
				"Muy difícil de adivinar",
				"Nagyon nehéz kitalálni");
		}
		private void AddPasswordStrength(string value, string enText, string deText, string frText, string esText, string huText)
		{
			Database.ExecuteNonQuery($"INSERT INTO [LU].[PasswordStrength] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{enText}', 'en', 0, '{value}', 'Migration_20240829153000', 'Migration_20240829153000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[PasswordStrength] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{deText}', 'de', 0, '{value}', 'Migration_20240829153000', 'Migration_20240829153000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[PasswordStrength] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{frText}', 'fr', 0, '{value}', 'Migration_20240829153000', 'Migration_20240829153000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[PasswordStrength] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{esText}', 'es', 0, '{value}', 'Migration_20240829153000', 'Migration_20240829153000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[PasswordStrength] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{huText}', 'hu', 0, '{value}', 'Migration_20240829153000', 'Migration_20240829153000')");
		}
	}
}
