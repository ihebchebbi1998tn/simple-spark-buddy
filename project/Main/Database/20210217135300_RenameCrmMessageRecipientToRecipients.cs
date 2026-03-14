namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210217135300)]
	public class RenameCrmMessageRecipientToRecipients : Migration
	{
		public override void Up()
		{
			Database.RenameColumn("CRM.Message", "Recipient", "Recipients");
			Database.ChangeColumn("CRM.Message", new Column("Recipients", DbType.String, Int32.MaxValue, ColumnProperty.NotNull));
		}
	}
}
