namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20221005134500)]
	public class AddIsSyncableToNoteType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[LU].[NoteType]",
				new Column("IsSyncable",
					DbType.Boolean,
					ColumnProperty.NotNull,
					true));
		}
	}
}
