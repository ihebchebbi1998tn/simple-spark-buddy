using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

namespace Crm.Service.Database
{
	[Migration(20230809115500)]
	public class RemoveServiceOrderTimePostingsDate : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.ServiceOrderTimePostings", "Date");
		}
	}
}
