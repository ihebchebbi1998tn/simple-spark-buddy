namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211209150000)]
	public class PlannedDurationTimePosting : Migration
	{
		public override void Up()
		{
			Database.AddColumn("SMS.ServiceOrderTimePostings", new Column("PlannedDurationInMinutes")
			{
				Type = System.Data.DbType.Int32,
				ColumnProperty = ColumnProperty.Null
			});
			Database.ChangeColumn("SMS.ServiceOrderTimePostings", new Column("DurationInMinutes")
			{
				Type = System.Data.DbType.Int32,
				ColumnProperty = ColumnProperty.Null
			});
			Database.ChangeColumn("SMS.ServiceOrderTimePostings", new Column("UserId")
			{
				Type = System.Data.DbType.Guid,
				ColumnProperty = ColumnProperty.Null
			});
		}
	}
}
