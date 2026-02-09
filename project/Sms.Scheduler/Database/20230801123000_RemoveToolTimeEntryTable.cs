using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;

namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230801123000)]
	public class RemoveToolTimeEntryTable : Migration
	{
		public override void Up()
		{
			Database.RemoveTableIfExisting("[SMS].[ToolTimeEntry]");
		}
	}
}
