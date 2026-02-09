namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240820095700)]
	public class AddDurationAndTypeToServiceOrderTimePostings : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderTimePostings", "ServiceOrderTimePostingType"))
			{
				Database.AddColumn("SMS.ServiceOrderTimePostings",
					new Column("DurationInMinutes",
						DbType.Int32,
						ColumnProperty.Null));
				Database.AddColumn("SMS.ServiceOrderTimePostings",
					new Column("ServiceOrderTimePostingType",
						DbType.String,
						ColumnProperty.Null));

				Database.ExecuteNonQuery("SELECT * INTO #ServiceOrderTimePostingsUsed FROM SMS.ServiceOrderTimePostings WHERE PlannedDurationInMinutes IS NOT NULL AND (([From] IS NOT NULL AND [To] IS NOT NULL) OR DispatchId IS NOT NULL)");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderTimePostingsUsed SET AuthDataId = NULL");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderTimePostingsUsed SET ParentServiceOrderTimePostingId = [id]");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderTimePostingsUsed SET DurationInMinutes = COALESCE(DATEDIFF(MINUTE, [From], [To]), 0), ServiceOrderTimePostingType = 'Used', [id] = newid()");

				Database.ExecuteNonQuery("SELECT SMS.ServiceOrderTimePostings.* INTO #ServiceOrderTimePostingsInvoice FROM SMS.ServiceOrderTimePostings JOIN SMS.ServiceOrderHead ON OrderId = ContactKey WHERE SMS.ServiceOrderHead.Status IN ('PostProcessing', 'ReadyForInvoice', 'Invoiced', 'Closed')");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderTimePostingsInvoice SET AuthDataId = NULL");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderTimePostingsInvoice SET ParentServiceOrderTimePostingId = [id]");
				Database.ExecuteNonQuery("UPDATE #ServiceOrderTimePostingsInvoice SET DurationInMinutes = COALESCE(DATEDIFF(MINUTE, [From], [To]), 0), ServiceOrderTimePostingType = 'Invoice', [id] = newid()");

				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderTimePostings SET ServiceOrderTimePostingType = 'Preplanned', DurationInMinutes = PlannedDurationInMinutes WHERE PlannedDurationInMinutes IS NOT NULL");
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderTimePostings SET ServiceOrderTimePostingType = 'Used', DurationInMinutes = COALESCE(DATEDIFF(MINUTE, [From], [To]), 0) WHERE PlannedDurationInMinutes IS NULL");

				Database.ExecuteNonQuery("INSERT INTO SMS.ServiceOrderTimePostings SELECT * FROM #ServiceOrderTimePostingsUsed");
				Database.ExecuteNonQuery("INSERT INTO SMS.ServiceOrderTimePostings SELECT * FROM #ServiceOrderTimePostingsInvoice");

				Database.ChangeColumn("SMS.ServiceOrderTimePostings",
					new Column("DurationInMinutes",
						DbType.Int32,
						ColumnProperty.NotNull));
				Database.ChangeColumn("SMS.ServiceOrderTimePostings",
					new Column("ServiceOrderTimePostingType",
						DbType.String,
						ColumnProperty.NotNull));
				Database.RemoveColumn("SMS.ServiceOrderTimePostings", "PlannedDurationInMinutes");
			}
		}
	}
}
