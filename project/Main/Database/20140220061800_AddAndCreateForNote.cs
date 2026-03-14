namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140220061800)]
	public class AddAndCreateForNote : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("delete  [CRM].[Permission]  where PGroup= 'Note' And Name = 'Add'");
		}
	}
}