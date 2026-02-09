namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20240919102200)]
	public class AddParentIdToServiceOrderErrorCauses : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderErrorCauses", "ParentServiceOrderErrorCauseId"))
			{
				Database.AddColumn("SMS.ServiceOrderErrorCauses",
					new Column("ParentServiceOrderErrorCauseId",
						DbType.Guid,
						ColumnProperty.Null));

				Database.AddForeignKey("FK_ServiceOrderErrorCauses_ParentServiceOrderErrorCause",
					"[SMS].[ServiceOrderErrorCauses]",
					"ParentServiceOrderErrorCauseId",
					"[SMS].[ServiceOrderErrorCauses]",
					"ServiceOrderErrorCauseId",
					ForeignKeyConstraint.NoAction);
			}
		}
	}
}
