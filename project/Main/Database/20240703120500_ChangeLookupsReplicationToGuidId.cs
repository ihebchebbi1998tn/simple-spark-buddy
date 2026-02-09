namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240703120500)]
	public class ChangeLookupsReplicationToGuidId : Migration
	{
		public override void Up()
		{
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "AddressType", "AddressTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Branch1", "Branch1Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Branch2", "Branch2Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Branch3", "Branch3Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Branch4", "Branch4Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "BravoCategory", "BravoCategoryTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "BusinessRelationshipType", "BusinessRelationshipTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "BusinessTitle", "BusinessTitleId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "CompanyGroupFlag1", "CompanyGroupFlag1Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "CompanyGroupFlag2", "CompanyGroupFlag2Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "CompanyGroupFlag3", "CompanyGroupFlag3Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "CompanyGroupFlag4", "CompanyGroupFlag4Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "CompanyGroupFlag5", "CompanyGroupFlag5Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "CompanyPersonRelationshipType", "CompanyPersonRelationshipTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "CompanyType", "CompanyTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "CostCenter", "CostCenterId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Country", "CountryId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Currency", "ProjectCategoriyId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "DepartmentType", "DepartmentTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "DocumentCategory", "DocumentCategoryId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "EmailType", "EmailTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "FaxType", "FaxTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "InvoicingType", "InvoicingTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Language", "LangId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "LengthUnit", "LengthUnitId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "NoteType", "NoteTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "NumberOfEmployees", "NumberOfEmployeesId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "PaymentCondition", "PaymentConditionId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "PaymentInterval", "PaymentIntervalId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "PaymentType", "PaymentTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "PhoneType", "PhoneTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Region", "RegionId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Salutation", "SalutationTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "SalutationLetter", "LetterSalutationTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("SMS", "Skill", "SkillId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "SourceType", "Id");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "TaskType", "TaskTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "TimeUnit", "TimeUnitId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Title", "TitleTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "Turnover", "TurnoverId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "UserStatus", "UserStatusId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "WebsiteType", "WebsiteTypeId");
			Database.ChangeReplicatedEntityFromIntToGuidId("LU", "ZipCodeFilter", "Id");
		}
	}
}
