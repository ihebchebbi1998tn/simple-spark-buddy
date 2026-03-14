namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20240820095600)]
	public class AddParentIdToServiceOrderTimePostings : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderTimePostings", "ParentServiceOrderTimePostingId"))
			{
				Database.AddColumn("SMS.ServiceOrderTimePostings",
					new Column("ParentServiceOrderTimePostingId",
						DbType.Guid,
						ColumnProperty.Null));

				Database.AddForeignKey("FK_ServiceOrderTimePostings_ParentServiceOrderTimePosting",
					"[SMS].[ServiceOrderTimePostings]",
					"ParentServiceOrderTimePostingId",
					"[SMS].[ServiceOrderTimePostings]",
					"id",
					ForeignKeyConstraint.NoAction);
			}
		}
	}
}
