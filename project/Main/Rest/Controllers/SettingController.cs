namespace Main.Rest.Controllers
{
	using System;
	using System.Linq;

	using Crm.Library.BaseModel;
	using Crm.Library.Extensions;
	using Crm.Library.Modularization;
	using Crm.Library.Rest;

	using Microsoft.AspNetCore.Mvc;

	public class SettingController : RestController
	{
		private readonly IPluginSettingsProvider pluginSettingsProvider;

		public SettingController(IPluginSettingsProvider pluginSettingsProvider, RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.pluginSettingsProvider = pluginSettingsProvider;
		}

		public virtual ActionResult Get(string id)
		{
			if (pluginSettingsProvider.Settings.TryGetValue(id, out var value))
			{
				var result = value is Enum ? value.ToString() : value;
				return Rest(result);
			}
			return new NotFoundResult();
		}
		public virtual ActionResult List()
		{
			var orderedDic = pluginSettingsProvider.Settings.OrderBy(x => x.Key);
			var result = new SerializableDictionary<string, object>();
			result.AddAll(orderedDic);
			return Rest(result);
		}
	}
}
