namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Main.Model.Lookups;

	[Migration(20230825100300)]
	public class AddAuthDataIdToLookups : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<CostCenter>("LU", "CostCenter");
			helper.AddOrUpdateEntityAuthDataColumn<Country>("LU", "Country");
			helper.AddOrUpdateEntityAuthDataColumn<Currency>("LU", "Currency", "ProjectCategoriyId");
			helper.AddOrUpdateEntityAuthDataColumn<DocumentCategory>("LU", "DocumentCategory");
			helper.AddOrUpdateEntityAuthDataColumn<InvoicingType>("LU", "InvoicingType");
			helper.AddOrUpdateEntityAuthDataColumn<Language>("LU", "Language", "LangId");
			helper.AddOrUpdateEntityAuthDataColumn<LengthUnit>("LU", "LengthUnit");
			helper.AddOrUpdateEntityAuthDataColumn<PaymentCondition>("LU", "PaymentCondition");
			helper.AddOrUpdateEntityAuthDataColumn<PaymentInterval>("LU", "PaymentInterval");
			helper.AddOrUpdateEntityAuthDataColumn<PaymentType>("LU", "PaymentType");
			helper.AddOrUpdateEntityAuthDataColumn<Region>("LU", "Region");
			helper.AddOrUpdateEntityAuthDataColumn<Skill>("SMS", "Skill");
			helper.AddOrUpdateEntityAuthDataColumn<TimeUnit>("LU", "TimeUnit");
			helper.AddOrUpdateEntityAuthDataColumn<UserStatus>("LU", "UserStatus");
		}
	}
}
