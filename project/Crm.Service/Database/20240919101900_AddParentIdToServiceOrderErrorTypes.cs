namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20240919101900)]
	public class AddParentIdToServiceOrderErrorTypes : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderErrorTypes", "ParentServiceOrderErrorTypeId"))
			{
				Database.AddColumn("SMS.ServiceOrderErrorTypes",
					new Column("ParentServiceOrderErrorTypeId",
						DbType.Guid,
						ColumnProperty.Null));

				Database.AddForeignKey("FK_ServiceOrderErrorTypes_ParentServiceOrderErrorType",
					"[SMS].[ServiceOrderErrorTypes]",
					"ParentServiceOrderErrorTypeId",
					"[SMS].[ServiceOrderErrorTypes]",
					"ServiceOrderErrorTypeId",
					ForeignKeyConstraint.NoAction);
			}
		}
	}
}
