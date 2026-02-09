namespace Crm.Service.Model.Notes
{
	using Crm.Model.Notes;

	public class ServiceCaseStatusChangedNote : Note
	{
		public override string ImageTextKey
		{
			get { return "Status"; }
		}
		public override string PermanentLabelResourceKey
		{
			get { return "NotificationStatusSetToBy"; }
		}

		public ServiceCaseStatusChangedNote()
		{
			Plugin = "Crm.Service";
		}
	}
}
