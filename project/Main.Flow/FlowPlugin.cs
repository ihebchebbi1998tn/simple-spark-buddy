using Crm.Library.Configuration;
using Crm.Library.Modularization;

namespace Main.Flow
{
	[Plugin(ModuleId = "FLD03230", Requires = "Main")]
	public class FlowPlugin : Plugin
	{
		public static string PluginName = typeof(FlowPlugin).Namespace;
		public static class Settings
		{
			public static class System
			{
				public static SettingDefinition<int> MaxRetries => new SettingDefinition<int>("MaxRetries", PluginName);
			}
		}
	}
}
