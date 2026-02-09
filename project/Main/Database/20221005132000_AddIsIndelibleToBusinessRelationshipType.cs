namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20221005132000)]
	public class AddIsIndelibleToBusinessRelationshipType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[LU].[BusinessRelationshipType]",
				new Column("IsIndelible",
					DbType.Boolean,
					ColumnProperty.NotNull,
					false));
		}
	}
}
