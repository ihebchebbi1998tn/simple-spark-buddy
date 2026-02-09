using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

namespace Sms.Scheduler.Database
{
	[Migration(20240313121400)]
	public class AddUniqueConstraintToRplProfileResource : Migration
	{
		public override void Up()
		{
			Database.AddUniqueConstraint("UC_ProfileResource", "RPL.ProfileResource", "ProfileKey", "ResourceKey");
		}
	}
}
