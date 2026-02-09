namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160819154100)]
	public class DropNextLowFromNumberingSequence : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("[dbo].[NumberingSequence]", "NextLow");
		}
	}
}