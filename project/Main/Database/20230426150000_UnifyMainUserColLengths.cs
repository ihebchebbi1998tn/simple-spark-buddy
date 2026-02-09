namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230426150000)]
	public class UnifyMainUserColLengths : Migration
	{
		public override void Up()
		{
			(string, string)[] tables = { ("CRM", "Address"), ("CRM", "Bravo"), ("CRM", "BusinessRelationship"), ("CRM", "Communication"), ("CRM", "Contact"), ("CRM", "DocumentAttributes"), ("CRM", "FileResource"), ("CRM", "Group"), ("CRM", "LinkResource"), ("CRM", "Message"), ("CRM", "Note"), ("CRM", "PersonPosition"), ("CRM", "Station"), ("CRM", "Task"), ("CRM", "User"), ("LU", "AddressType"), ("LU", "BusinessRelationshipType"), ("LU", "CompanyGroupFlag1"), ("LU", "CompanyGroupFlag2"), ("LU", "CompanyGroupFlag3"), ("LU", "CompanyGroupFlag4"), ("LU", "CompanyGroupFlag5"), ("LU", "CompanyType"), ("LU", "Country"), ("LU", "DepartmentType"), ("LU", "DocumentCategory"), ("LU", "EmailType"), ("LU", "FaxType"), ("LU", "InvoicingType"), ("LU", "Language"), ("LU", "LengthUnit"), ("LU", "NoCausingItemPreviousSerialNoReason"), ("LU", "NoCausingItemSerialNoReason"), ("LU", "NoPreviousSerialNoReason"), ("LU", "NoteType"), ("LU", "NumberOfEmployees"), ("LU", "PaymentCondition"), ("LU", "PaymentInterval"), ("LU", "PaymentType"), ("LU", "PhoneType"), ("LU", "ProjectCategoryGroups"), ("LU", "Region"), ("LU", "SalutationLetter"), ("LU", "TaskType"), ("LU", "Turnover"), ("LU", "UserStatus"), ("LU", "WebsiteType"), ("LU", "ZipCodeFilter"), ("SMS", "Skill"), ("SMS", "UserSkill") };
			foreach (var table in tables)
			{
				var tableName = $"[{table.Item1}].[{table.Item2}]";
				if (Database.TableExists($"{tableName}"))
				{
					if (Database.ColumnExists($"{tableName}", "ModifyUser"))
					{
						var modifyUserLength = (int)Database.ExecuteScalar(
							$"SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{table.Item1}' AND TABLE_NAME = '{table.Item2}' AND COLUMN_NAME = 'ModifyUser'");
						if (modifyUserLength < 255)
							Database.ExecuteNonQuery($"ALTER TABLE {tableName} ALTER COLUMN ModifyUser NVARCHAR(255) NOT NULL");
					}

					if (Database.ColumnExists($"{tableName}", "CreateUser"))
					{
						var createUserLength = (int)Database.ExecuteScalar(
							$"SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{table.Item1}' AND TABLE_NAME = '{table.Item2}' AND COLUMN_NAME = 'CreateUser'");
						if (createUserLength < 255)
							Database.ExecuteNonQuery($"ALTER TABLE {tableName} ALTER COLUMN CreateUser NVARCHAR(255) NOT NULL");
					}
				}
			}
		}
	}
}
