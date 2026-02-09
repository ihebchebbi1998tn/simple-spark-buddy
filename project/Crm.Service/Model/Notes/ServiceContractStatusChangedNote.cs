namespace Crm.Service.Model.Notes
{
	using Crm.Model.Notes;

	public class ServiceContractStatusChangedNote : Note
	{
		public override string ImageTextKey
		{
			get { return "Status"; }
		}
		public override string PermanentLabelResourceKey
		{
			get { return "NotificationStatusSetToBy"; }
		}
	}
}
