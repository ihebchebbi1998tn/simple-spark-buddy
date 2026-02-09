namespace Crm.Service.Database
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240802100000)]
	public class AddEndDateAndNetWorkMinutesToDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumn("[SMS].[ServiceOrderDispatch]",
				"NetWorkMinutes",
				DbType.Int32,
				ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceOrderDispatch]",
				"EndDate",
				DbType.DateTime,
				ColumnProperty.Null);

			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderDispatch] SET [EndDate] = DATEADD(minute,[DurationInMinutes],[Date])");

			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] ALTER COLUMN [EndDate] DATETIME NOT NULL");
			Database.RemoveColumn("[SMS].[ServiceOrderDispatch]", "DurationInMinutes");
		}
	}
}
