namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20150413180900)]
	public class AddFileResourceKeyFkToCrmDocumentAttributes : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_DocumentAttributes_FileResource'") == 0)
			{
				Database.ChangeColumn("[CRM].[DocumentAttributes]", new Column("FileResourceKey", DbType.Int64, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE da SET da.[FileResourceKey] = NULL FROM [CRM].[DocumentAttributes] da LEFT OUTER JOIN [CRM].[FileResource] fr ON da.[FileResourceKey] = fr.[Id] WHERE fr.[Id] IS NULL");
				Database.AddForeignKey("FK_DocumentAttributes_FileResource", "[CRM].[DocumentAttributes]", "FileResourceKey", "[CRM].[FileResource]", "Id", ForeignKeyConstraint.SetNull);
			}
		}
	}
}