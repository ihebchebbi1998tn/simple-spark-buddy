namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170523101900)]
	public class UpdateLookupsToEntityLookup : Migration
	{
		public override void Up()
		{
			const string createUser = "Migration_20170523101900";
			AddEntityLookupColumns("Lu", "Language", createUser);
			AddEntityLookupColumns("Lu", "UserStatus", createUser);
			AddEntityLookupColumns("Sms", "Skill", createUser);
			AddEntityLookupColumns("Lu", "LengthUnit", createUser);
			AddEntityLookupColumns("Lu", "SalutationLetter", createUser);
			AddEntityLookupColumns("Lu", "NumberOfEmployees", createUser);
			AddEntityLookupColumns("Lu", "Turnover", createUser);
			AddEntityLookupColumns("Lu", "PaymentType", createUser);
			AddEntityLookupColumns("Lu", "PaymentInterval", createUser);
			AddEntityLookupColumns("Lu", "PaymentCondition", createUser);
			AddEntityLookupColumns("Lu", "InvoicingType", createUser);
			AddEntityLookupColumns("Lu", "ZipCodeFilter", createUser);
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
	}
}