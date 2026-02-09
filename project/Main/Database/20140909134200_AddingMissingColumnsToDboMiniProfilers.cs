namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140909134200)]
	public class AddingMissingColumnsToDboMiniProfilers : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[dbo].[MiniProfilers]", "CustomLinksJson"))
			{
				Database.AddColumn("[dbo].[MiniProfilers]", new Column("CustomLinksJson", DbType.String, Int32.MaxValue , ColumnProperty.Null));
			}

			if (!Database.ColumnExists("[dbo].[MiniProfilers]", "ClientTimingsRedirectCount"))
			{
				Database.AddColumn("[dbo].[MiniProfilers]", new Column("ClientTimingsRedirectCount", DbType.Int32, ColumnProperty.Null));
			}

			if (!Database.ColumnExists("[dbo].[MiniProfilerTimings]", "CustomTimingsJson"))
			{
				Database.AddColumn("[dbo].[MiniProfilerTimings]", new Column("CustomTimingsJson", DbType.String, Int32.MaxValue, ColumnProperty.Null));
			}
		}
	}
}