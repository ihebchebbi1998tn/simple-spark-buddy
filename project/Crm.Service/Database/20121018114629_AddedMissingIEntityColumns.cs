namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20121018114629)]
	public class AddedMissingIEntityColumns : Migration
	{
		private const string IsActiveColumnName = "IsActive";
		private readonly string[] tables =
			{
				"SMS.ServiceOrderMaterial",
				"SMS.ServiceOrderTimes",
				"SMS.ServiceOrderTimePostings"
			};

		public override void Up()
		{
			var isActiveColumn = new Column(IsActiveColumnName, DbType.Boolean, ColumnProperty.NotNull, true);

			foreach (string table in tables)
			{
				Database.AddColumnIfNotExisting(table, isActiveColumn);
			}
		}
		public override void Down()
		{
			foreach (string table in tables)
			{
				Database.RemoveColumnIfExisting(table, IsActiveColumnName);
			}
		}
	}
}