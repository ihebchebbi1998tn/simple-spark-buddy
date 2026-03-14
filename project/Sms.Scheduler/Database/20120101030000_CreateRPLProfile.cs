namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120101030000)]
	public class CreateRPLProfile : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[RPL].[Profile]"))
			{
				Database.AddTable("[RPL].[Profile]",
					new Column("Id", DbType.Int32, ColumnProperty.Identity),
					new Column("Username", DbType.String, 256, ColumnProperty.Null),
					new Column("Name", DbType.String, 400, ColumnProperty.Null),
					new Column("DefaultProfile", DbType.Boolean, ColumnProperty.NotNull),
					new Column("LowerBound", DbType.Decimal, ColumnProperty.NotNull),
					new Column("UpperBound", DbType.Decimal, ColumnProperty.NotNull),
					new Column("PipelineGroupStorage", DbType.String, 4000, ColumnProperty.Null),
					new Column("ResourceGroup", DbType.String, 20, ColumnProperty.Null),
					new Column("PersonDisplayName", DbType.String, 20, ColumnProperty.Null),
					new Column("PersonSortBy", DbType.String, 20, ColumnProperty.Null),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.Null),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.Null),
					new Column("CreateUser", DbType.String, 255, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 255, ColumnProperty.NotNull),
					new Column("TemplateKey", DbType.Int32, ColumnProperty.Null),
					new Column("InternalId", DbType.Guid, ColumnProperty.NotNull, "newsequentialid()")
				);
			}
		}
	}
}
