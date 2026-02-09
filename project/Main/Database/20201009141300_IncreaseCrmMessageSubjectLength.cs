namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20201009141300)]
	public class IncreaseCrmMessageSubjectLength : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("CRM.Message", "Subject"))
			{
				Database.ChangeColumn("CRM.Message", new Column("Subject", DbType.String, 255, ColumnProperty.NotNull));
			}
		}
	}
}
