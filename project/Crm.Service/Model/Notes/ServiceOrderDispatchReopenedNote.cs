namespace Crm.Service.Model.Notes
{
	using Crm.Model.Notes;

	public class ServiceOrderDispatchReopenedNote : Note
	{
		public override string ImageTextKey => "Dispatch";
		public override string PermanentLabelResourceKey => "DispatchReopenedBy";

		public ServiceOrderDispatchReopenedNote()
		{
			Plugin = ServicePlugin.PluginName;
		}
	}
}
