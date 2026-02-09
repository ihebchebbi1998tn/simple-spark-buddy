namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230825100200)]
	public class ChangeLookupsToGuidId : Migration
	{
		public override void Up()
		{
			Database.ChangeTableFromIntToGuidId("LU", "CostCenter", "CostCenterId");
			Database.ChangeTableFromIntToGuidId("LU", "Country", "CountryId");
			Database.ChangeTableFromIntToGuidId("LU", "Currency", "ProjectCategoriyId");
			Database.ChangeTableFromIntToGuidId("LU", "DocumentCategory", "DocumentCategoryId");
			Database.ChangeTableFromIntToGuidId("LU", "InvoicingType", "InvoicingTypeId");
			Database.ChangeTableFromIntToGuidId("LU", "Language", "LangId");
			Database.ChangeTableFromIntToGuidId("LU", "LengthUnit", "LengthUnitId");
			Database.ChangeTableFromIntToGuidId("LU", "PaymentCondition", "PaymentConditionId");
			Database.ChangeTableFromIntToGuidId("LU", "PaymentInterval", "PaymentIntervalId");
			Database.ChangeTableFromIntToGuidId("LU", "PaymentType", "PaymentTypeId");
			Database.ChangeTableFromIntToGuidId("LU", "Region", "RegionId");
			Database.ChangeTableFromIntToGuidId("SMS", "Skill", "SkillId");
			Database.ChangeTableFromIntToGuidId("LU", "TimeUnit", "TimeUnitId");
			Database.ChangeTableFromIntToGuidId("LU", "UserStatus", "UserStatusId");
			Database.ChangeTableFromIntToGuidId("LU", "ZipCodeFilter", "Id");
		}
	}
}
