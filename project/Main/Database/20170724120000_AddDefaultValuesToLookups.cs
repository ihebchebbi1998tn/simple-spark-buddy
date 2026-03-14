namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Library.Extensions;

	[Migration(20170724120000)]
	public class AddDefaultValuesToLookups : Migration
	{
		public override void Up()
		{
			new[]
			{
				new { Schema = "LU", Table = "AddressType" },
				new { Schema = "LU", Table = "BravoCategory" },
				new { Schema = "LU", Table = "BusinessRelationshipType" },
				new { Schema = "LU", Table = "BusinessTitle" },
				new { Schema = "LU", Table = "CompanyGroupFlag1" },
				new { Schema = "LU", Table = "CompanyGroupFlag2" },
				new { Schema = "LU", Table = "CompanyGroupFlag3" },
				new { Schema = "LU", Table = "CompanyGroupFlag4" },
				new { Schema = "LU", Table = "CompanyGroupFlag5" },
				new { Schema = "LU", Table = "CompanyType" },
				new { Schema = "LU", Table = "Country" },
				new { Schema = "LU", Table = "Currency" },
				new { Schema = "LU", Table = "DepartmentType" },
				new { Schema = "LU", Table = "EmailType" },
				new { Schema = "LU", Table = "FaxType" },
				new { Schema = "LU", Table = "InvoicingType" },
				new { Schema = "LU", Table = "Language" },
				new { Schema = "LU", Table = "LengthUnit" },
				new { Schema = "LU", Table = "NoteType" },
				new { Schema = "LU", Table = "NumberOfEmployees" },
				new { Schema = "LU", Table = "PaymentCondition" },
				new { Schema = "LU", Table = "PaymentInterval" },
				new { Schema = "LU", Table = "PaymentType" },
				new { Schema = "LU", Table = "PhoneType" },
				new { Schema = "LU", Table = "Region" },
				new { Schema = "LU", Table = "Salutation" },
				new { Schema = "LU", Table = "SalutationLetter" },
				new { Schema = "SMS", Table = "Skill" },
				new { Schema = "LU", Table = "SourceType" },
				new { Schema = "LU", Table = "TaskType" },
				new { Schema = "LU", Table = "TimeUnit" },
				new { Schema = "LU", Table = "Title" },
				new { Schema = "LU", Table = "Turnover" },
				new { Schema = "LU", Table = "UserStatus" },
				new { Schema = "LU", Table = "WebsiteType" },
				new { Schema = "LU", Table = "ZipCodeFilter" }
			}
			.ForEach(x => Database.AddEntityBaseDefaultContraints(x.Schema, x.Table));
		}
	}
}