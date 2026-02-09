namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20231020150100)]
	public class AddErrorTypesToLuStatisticsKeyCause : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[LU].[StatisticsKeyCause]", "ErrorTypes"))
			{
				Database.AddColumn("[LU].[StatisticsKeyCause]", "ErrorTypes", DbType.String, 500, ColumnProperty.Null);
			}
		}
	}
}
