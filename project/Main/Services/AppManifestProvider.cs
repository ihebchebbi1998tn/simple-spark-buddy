namespace Main.Services
{
	using Crm.Library.Extensions;
	using Crm.Library.Model.Site;
	using Crm.Library.Services.Interfaces;

	using Newtonsoft.Json;

	public class AppManifest
	{
		public string name { get; set; }
		public string short_name { get; set; }
		public string start_url { get; set; }
		public string theme_color { get; set; }
		public string display { get; set; }
		public string description { get; set; }
		public AppManifestIcon[] icons { get; set; }
	}

	public class AppManifestIcon
	{
		public string src { get; set; }
		public string sizes { get; set; }
		public string type { get; set; }
		public string purpose { get; set; }
	}

	public class AppManifestProvider : IAppManifestProvider
	{
		private readonly Site site;
		public AppManifestProvider(Site site)
		{
			this.site = site;
		}
		public virtual string GetManifest()
		{
			var manifest = new AppManifest
			{
				name = site.Name,
				short_name = site.Name,
				start_url = site.GetExtension<DomainExtension>().HostUri.ToString().AppendIfMissing("/") + "Main/Home/Index",
				theme_color = MaterialCacheManifestRegistrar.Themes[site.GetExtension<DomainExtension>().MaterialTheme],
				display = "standalone",
				description = site.Name,
				icons =
				[
					new AppManifestIcon
					{
						src = site.GetExtension<DomainExtension>().HostUri.ToString().AppendIfMissing("/") + "Plugins/Main/Content/img/icon-1024.png",
						type = "image/png",
						sizes = "1024x1024",
						purpose = "any"
					}
				]
			};

			return JsonConvert.SerializeObject(manifest);
		}
	}
}
