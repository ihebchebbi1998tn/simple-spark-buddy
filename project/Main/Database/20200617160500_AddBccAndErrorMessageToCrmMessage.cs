namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200617113500)]
	public class AddBccAndErrorMessageToCrmMessage : Migration
	{
		public override void Up()
		{
			Database.AddColumn("CRM.Message", "Bcc", DbType.String, Int32.MaxValue, ColumnProperty.Null);
			Database.AddColumn("CRM.Message", "ErrorMessage", DbType.String, Int32.MaxValue, ColumnProperty.Null);
		}
	}
}
