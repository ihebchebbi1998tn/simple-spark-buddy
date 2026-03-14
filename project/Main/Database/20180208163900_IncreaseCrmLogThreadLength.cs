namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180208163900)]
	public class IncreaseCrmLogThreadLength : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[CRM].[Log]", new Column("Thread", DbType.String, 512, ColumnProperty.NotNull));
		}
	}
}