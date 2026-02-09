namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20240919104600)]
	public class AddDispatchIdToServiceOrderErrorCauses : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderErrorCauses", "DispatchId"))
			{
				Database.AddColumn("SMS.ServiceOrderErrorCauses",
					new Column("DispatchId",
						DbType.Guid,
						ColumnProperty.Null));

				Database.AddForeignKey("FK_ServiceOrderErrorCauses_Dispatch",
					"[SMS].[ServiceOrderErrorCauses]",
					"DispatchId",
					"[SMS].[ServiceOrderDispatch]",
					"DispatchId",
					ForeignKeyConstraint.NoAction);
			}
		}
	}
}
