namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240906120600)]
	public class MoveKilometersToDistanceForTimePostings : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderTimePostings] SET Distance = Kilometers, DistanceUnitKey = 'Km' Where Kilometers IS NOT NULL");
		}
	}
}
