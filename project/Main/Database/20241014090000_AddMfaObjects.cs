namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	using Main.Model.Lookups;

	[Migration(20241014090000)]
	public class AddMfaObjects : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[User]", new Column("TotpAuthorizationKey", DbType.String, 128, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("dbo.Domain", new Column("MultiFactorAuthenticationModeKey", DbType.String, 64, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[CRM].[Usergroup]", new Column("MultiFactorAuthenticationMandatory", DbType.Boolean, ColumnProperty.NotNull, false));

			Database.ExecuteNonQuery(@$"UPDATE dbo.Domain 
				SET MultiFactorAuthenticationModeKey = '{MultiFactorAuthenticationMode.MandatoryForSpecificUserGroupsKey}' 
				WHERE UId = '00000000-0000-0000-0000-000000000000'");

			if (!Database.TableExists("[LU].[MultiFactorAuthenticationMode]"))
			{
				Database.AddTable(
					"[LU].[MultiFactorAuthenticationMode]",
					new Column($"MultiFactorAuthenticationModeId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
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

				AddMultiFactorAuthenticationMode(MultiFactorAuthenticationMode.MandatoryForSpecificUserGroupsKey,
					"Mandatory for specific user groups",
					"Obligatorisch für bestimmte Benutzergruppen",
					"Obligatoire pour certains groupes d''utilisateurs",
					"Obligatorio para grupos de usuarios específicos",
					"Kötelező bizonyos felhasználói csoportok számára",
					"0");

				AddMultiFactorAuthenticationMode(MultiFactorAuthenticationMode.MandatoryForAllUsersKey,
					"Mandatory for all users",
					"Für alle Benutzer verpflichtend",
					"Obligatoire pour tous les utilisateurs",
					"Obligatorio para todos los usuarios",
					"Kötelező minden felhasználó számára",
					"1");
			}
		}

		private void AddMultiFactorAuthenticationMode(string value, string enText, string deText, string frText, string esText, string huText, string sortOrder)
		{
			Database.ExecuteNonQuery($"INSERT INTO [LU].[MultiFactorAuthenticationMode] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{enText}', 'en', 0, '{sortOrder}', 'Migration_20241014090000', 'Migration_20241014090000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[MultiFactorAuthenticationMode] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{deText}', 'de', 0, '{sortOrder}', 'Migration_20241014090000', 'Migration_20241014090000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[MultiFactorAuthenticationMode] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{frText}', 'fr', 0, '{sortOrder}', 'Migration_20241014090000', 'Migration_20241014090000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[MultiFactorAuthenticationMode] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{esText}', 'es', 0, '{sortOrder}', 'Migration_20241014090000', 'Migration_20241014090000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[MultiFactorAuthenticationMode] (Value, Name, Language, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{value}', '{huText}', 'hu', 0, '{sortOrder}', 'Migration_20241014090000', 'Migration_20241014090000')");
		}
	}
}
