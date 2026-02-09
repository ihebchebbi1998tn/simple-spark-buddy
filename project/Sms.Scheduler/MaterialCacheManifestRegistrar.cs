namespace Sms.Scheduler
{
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Offline;
	using Crm.Library.Offline.Interfaces;

	public class MaterialCacheManifestRegistrar : CacheManifestRegistrar<MaterialCacheManifest>
	{
		public MaterialCacheManifestRegistrar(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
		}
		protected override void Initialize()
		{
			CacheCss("schedulerCss");
			CacheJs("schedulerTs");
			Cache("~/static-dist/Sms.Scheduler/style/fonts/Roboto-Regular.woff2");
			Cache("~/static-dist/Sms.Scheduler/style/fonts/Roboto-Medium.woff2");
			Cache("~/static-dist/Sms.Scheduler/style/fonts/Roboto-Bold.woff2");
		}
	}
}
