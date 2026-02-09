namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20200618134000)]
	public class CrmMessageAttachments : Migration
	{
		public override void Up()
		{
			Database.AddTable("CRM.MessageAttachment", 
				new Column("MessageKey", DbType.Guid, ColumnProperty.NotNull),
				new Column("FileResourceKey", DbType.Guid, ColumnProperty.NotNull));
			Database.AddForeignKey("FK_MessageAttachment_Message", "CRM.MessageAttachment", "MessageKey", "CRM.Message", "MessageId", ForeignKeyConstraint.Cascade);
			Database.AddForeignKey("FK_MessageAttachment_FileResource", "CRM.MessageAttachment", "FileResourceKey", "CRM.FileResource", "Id", ForeignKeyConstraint.Cascade);
		}
	}
}
