namespace Crm.Service.Model.Notes
{
	using Crm.Model.Notes;

	public class OrderStatusChangedNote : Note
	{
		public override string ImageTextKey
		{
			get { return "Status"; }
		}

		public override string PermanentLabelResourceKey
		{
			get { return "OrderStatusSetToBy"; }
		}

		public OrderStatusChangedNote()
		{
			Plugin = "Crm.Service";
		}
	}
}
