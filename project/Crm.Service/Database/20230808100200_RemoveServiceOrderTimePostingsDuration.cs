using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

namespace Crm.Service.Database
{
	[Migration(20230808100200)]
	public class RemoveServiceOrderTimePostingsDuration : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.ServiceOrderTimePostings", "DurationInMinutes");
		}
	}
}
