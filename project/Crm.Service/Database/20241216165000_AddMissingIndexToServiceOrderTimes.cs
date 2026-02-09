namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20241216165000)]
	public class AddMissingIndexToServiceOrderTimes : Migration
	{
		public override void Up()
		{
			if (!Database.IndexExists("[SMS].[ServiceOrderTimes]", "IX_ServiceOrderTimes_IsActive_InstallationId"))
			{
				Database.ExecuteNonQuery(@"
CREATE NONCLUSTERED INDEX IX_ServiceOrderTimes_IsActive_InstallationId 
ON [SMS].[ServiceOrderTimes] ([IsActive],[InstallationId]) 
INCLUDE ([OrderId])
");
			}
		}
	}
}
