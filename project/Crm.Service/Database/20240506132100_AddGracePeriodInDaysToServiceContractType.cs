namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240506132100)]
	public class AddGracePeriodInDaysToServiceContractType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceContractType]", new Column("GracePeriodInDays")
			{
				Type = DbType.Int32,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = 0
			});
		}
	}
}
