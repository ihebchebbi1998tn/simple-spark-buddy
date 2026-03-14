namespace Crm.Database.Modify
{
	using System.Collections.Generic;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220411143000)]
	public class AddColorValuesToServiceOrderDispatchStatus : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ServiceOrderDispatchStatus]") && Database.ColumnExists("[SMS].[ServiceOrderDispatchStatus]", "Color"))
			{
				var colorValues = new List<KeyValuePair<string, string>>()
				{
					new KeyValuePair<string, string>("Scheduled","#ECFFE5"),
					new KeyValuePair<string, string>("Released","#C6FFB2"),
					new KeyValuePair<string, string>("Read","#8DFF66"),
					new KeyValuePair<string, string>("Rejected","#FF0000"),
					new KeyValuePair<string, string>("InProgress","#2196F3"),
					new KeyValuePair<string, string>("SignedByCustomer","#0A6EBD"),
					new KeyValuePair<string, string>("ClosedNotComplete","#EDEDED"),
					new KeyValuePair<string, string>("ClosedComplete","#D3D3D3")
				};
				foreach (var colorvalue in colorValues)
				{
					Database.ExecuteNonQuery($"UPDATE [SMS].[ServiceOrderDispatchStatus] SET Color = '{colorvalue.Value}' Where Value = '{colorvalue.Key}'");
				}
			}
		}
	}
}
