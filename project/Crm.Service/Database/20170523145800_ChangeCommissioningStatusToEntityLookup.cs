namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170523145800)]
	public class ChangeCommissioningStatusToEntityLookup : Migration
	{
		public override void Up()
		{
			const string createUser = "Migration_20170523145800";
			AddEntityLookupColumns("Sms", "CommissioningStatus", createUser);

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderHead' AND COLUMN_NAME='CommissioningStatus')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderHead] ADD [CommissioningStatusKey] nvarchar(10) NULL
					EXEC('UPDATE [SMS].[ServiceOrderHead] SET [CommissioningStatusKey] = [CommissioningStatus]')

					DECLARE @ConstraintName nvarchar(200)
					SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS WHERE PARENT_OBJECT_ID = OBJECT_ID('[Sms].[ServiceOrderHead]') AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns WHERE NAME = N'CommissioningStatus' AND object_id = OBJECT_ID(N'[Sms].[ServiceOrderHead]'))
					IF @ConstraintName IS NOT NULL
						EXEC('ALTER TABLE [Sms].[ServiceOrderHead] DROP CONSTRAINT ' + @ConstraintName)
					IF EXISTS (SELECT * FROM syscolumns WHERE id=object_id('[Sms].[ServiceOrderHead]') AND name='CommissioningStatus')
						EXEC('ALTER TABLE [Sms].[ServiceOrderHead] DROP COLUMN CommissioningStatus')

					ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT DF_ServiceOrder_CommissionStatusKey DEFAULT N'0' FOR CommissioningStatusKey;
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ServiceOrderMaterial' AND COLUMN_NAME='CommissioningStatus')
				BEGIN
					ALTER TABLE [SMS].[ServiceOrderMaterial] ADD [CommissioningStatusKey] nvarchar(10) NULL
					EXEC('UPDATE [SMS].[ServiceOrderMaterial] SET [CommissioningStatusKey] = [CommissioningStatus]')

					DECLARE @ConstraintName nvarchar(200)
					SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS WHERE PARENT_OBJECT_ID = OBJECT_ID('[Sms].[ServiceOrderMaterial]') AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns WHERE NAME = N'CommissioningStatus' AND object_id = OBJECT_ID(N'[Sms].[ServiceOrderMaterial]'))
					IF @ConstraintName IS NOT NULL
						EXEC('ALTER TABLE [Sms].[ServiceOrderMaterial] DROP CONSTRAINT ' + @ConstraintName)
					IF EXISTS (SELECT * FROM syscolumns WHERE id=object_id('[Sms].[ServiceOrderMaterial]') AND name='CommissioningStatus')
						EXEC('ALTER TABLE [Sms].[ServiceOrderMaterial] DROP COLUMN CommissioningStatus')

					ALTER TABLE [SMS].[ServiceOrderMaterial] ADD CONSTRAINT DF_ServiceMaterial_CommissionStatusKey DEFAULT N'0' FOR CommissioningStatusKey;
				END");

			InsertLookup("0", "NoCommissioning", "en", false, 0);
			InsertLookup("0", "NoCommissioning", "de", false, 0);
			InsertLookup("0", "NoCommissioning", "fr", false, 0);
			InsertLookup("0", "NoCommissioning", "hu", false, 0);

			InsertLookup("1", "ToBeCommissioned", "en", false, 0);
			InsertLookup("1", "ToBeCommissioned", "de", false, 0);
			InsertLookup("1", "ToBeCommissioned", "fr", false, 0);
			InsertLookup("1", "ToBeCommissioned", "hu", false, 0);

			InsertLookup("2", "PartlyCommissioned", "en", false, 0);
			InsertLookup("2", "PartlyCommissioned", "de", false, 0);
			InsertLookup("2", "PartlyCommissioned", "fr", false, 0);
			InsertLookup("2", "PartlyCommissioned", "hu", false, 0);

			InsertLookup("3", "Commissioned", "en", false, 0);
			InsertLookup("3", "Commissioned", "de", false, 0);
			InsertLookup("3", "Commissioned", "fr", false, 0);
			InsertLookup("3", "Commissioned", "hu", false, 0);
		}

		private void AddEntityLookupColumns(string schema, string table, string createUser)
		{
			Database.ExecuteNonQuery($@"
				IF COL_LENGTH('[{schema}].[{table}]','CreateDate') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [CreateDate] datetime NOT NULL CONSTRAINT DF_{schema}{table}_CreateDate DEFAULT GETUTCDATE()
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_CreateDate
				END
				IF COL_LENGTH('[{schema}].[{table}]','ModifyDate') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [ModifyDate] datetime NOT NULL CONSTRAINT DF_{schema}{table}_ModifyDate DEFAULT GETUTCDATE()
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_ModifyDate
				END
				IF COL_LENGTH('[{schema}].[{table}]','CreateUser') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [CreateUser] nvarchar(100) NOT NULL CONSTRAINT DF_{schema}{table}_CreateUser DEFAULT '{createUser}'
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_CreateUser
				END
				IF COL_LENGTH('[{schema}].[{table}]','ModifyUser') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [ModifyUser] nvarchar(100) NOT NULL CONSTRAINT DF_{schema}{table}_ModifyUser DEFAULT '{createUser}'
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_ModifyUser
				END
				IF COL_LENGTH('[{schema}].[{table}]','IsActive') IS NULL 
				BEGIN
					ALTER TABLE [{schema}].[{table}] ADD [IsActive] bit NOT NULL CONSTRAINT DF_{schema}{table}_IsActive DEFAULT 1
					ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT DF_{schema}{table}_IsActive
				END
			");
		}
		private void InsertLookup(string value, string name, string language, bool favorite, int sortorder)
		{
			Database.ExecuteNonQuery($"INSERT INTO [SMS].[CommissioningStatus] ([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) VALUES ('{value}', '{name}', '{language}', {(favorite ? 1 : 0)}, {sortorder}, GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', 1)");
		}
	}
}