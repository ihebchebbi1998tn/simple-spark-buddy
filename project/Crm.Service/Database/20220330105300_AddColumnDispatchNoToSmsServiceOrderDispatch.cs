using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
using System.Data;

namespace Crm.Service.Database
{
	[Migration(20220330105300)]
	public class AddColumnPersonNoToCrmPerson : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ServiceOrderDispatch]"))
			{
				Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("DispatchNo", DbType.String, 20, ColumnProperty.Null));
			}
		}
	}
}
