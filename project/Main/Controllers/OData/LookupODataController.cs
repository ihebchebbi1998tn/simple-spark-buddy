namespace Main.Controllers.OData
{
	using System.Collections.Concurrent;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.Globalization.Lookup;

	using Main.Rest.Model;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Routing.Controllers;

	public class LookupODataControllerCache : ISingletonDependency
	{
		public virtual ConcurrentDictionary<string, LookupType> LookupTypes { get; } = new();
	}

	public class LookupODataController : ODataController, IODataOperationImportController
	{
		private readonly LookupODataControllerCache cache;
		private readonly ILookupManager lookupManager;
		private readonly IMapper mapper;
		private readonly IUsedLookupsService usedLookupsService;
		public LookupODataController(ILookupManager lookupManager, IMapper mapper, LookupODataControllerCache cache, IUsedLookupsService usedLookupsService)
		{
			this.lookupManager = lookupManager;
			this.mapper = mapper;
			this.cache = cache;
			this.usedLookupsService = usedLookupsService;
		}
		[HttpGet]
		public virtual IActionResult GetLookupType(string FullName)
		{
			if (cache.LookupTypes.TryGetValue(FullName, out var cached))
			{
				return Ok(cached);
			}

			var lookupType = lookupManager.RegisteredTypes(false).FirstOrDefault(x => FullName.Equals(x.FullName));
			if (lookupType == null)
			{
				return NotFound();
			}

			var result = mapper.Map<LookupType[]>(new[] { lookupType })[0];
			cache.LookupTypes.TryAdd(FullName, result);
			return Ok(result);
		}
		[HttpGet]
		public virtual IActionResult GetLookupTypes()
		{
			var lookupTypes = lookupManager.RegisteredTypes(false).OrderBy(x => x.Name).ToArray();
			var result = lookupTypes.Select(x => cache.LookupTypes.GetOrAdd(x.FullName, _ => mapper.Map<LookupType[]>(new[] { x })[0])).ToArray();
			return Ok(result);
		}
		[HttpGet]
		public virtual IActionResult IsLookupUsed(string Key, string Type)
		{
			var lookupType = lookupManager.RegisteredTypes(false).FirstOrDefault(x => Type.Equals(x.FullName));
			if (lookupType == null)
			{
				return NotFound();
			}
			return Ok(usedLookupsService.IsUsed(Key, lookupType));
		}
	}
}
