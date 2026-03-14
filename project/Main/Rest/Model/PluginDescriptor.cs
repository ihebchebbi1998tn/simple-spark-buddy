namespace Main.Rest.Model
{
	public class PluginDescriptor
	{
		public bool IsCustomerPlugin { get; set; }
		public string PluginName { get; set; }
		public string[] RequiredPluginNames { get; set; }
	}
}
