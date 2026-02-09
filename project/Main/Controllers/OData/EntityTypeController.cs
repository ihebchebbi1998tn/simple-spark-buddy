namespace Main.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Dynamic.Core;

	using AutoMapper.Extensions.ExpressionMapping;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Services.Interfaces;

	using LMobile.Unicore;

	using Main.Rest.Model;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Extensions;
	using Microsoft.AspNetCore.OData.Query;

	[ControllerName("Main_EntityType")]
	public class EntityTypeController : ODataControllerEx
	{
		private readonly IODataMapper mapper;
		private readonly ISyncService<EntityType> entityTypeSyncService;
		private readonly IUserService userService;
		public EntityTypeController(IODataMapper mapper, ISyncService<EntityType> entityTypeSyncService, IUserService userService)
		{
			this.mapper = mapper;
			this.entityTypeSyncService = entityTypeSyncService;
			this.userService = userService;
		}
		[HttpGet]
		public virtual IActionResult Get(ODataQueryOptions<EntityTypeRest> options) => Get(options, null);
		[HttpGet]
		public virtual IActionResult Get(ODataQueryOptions<EntityTypeRest> options, Guid? key = null)
		{
			var settings = new ODataQuerySettings { EnableConstantParameterization = false, HandleNullPropagation = HandleNullPropagationOption.False };

			var query = entityTypeSyncService.GetAll(userService.CurrentUser);
			var projectedQuery = query
				.UseAsDataSource(mapper)
				.For<EntityTypeRest>();
			var destinationQuery = options.ApplyTo(projectedQuery, settings, AllowedQueryOptions.All & ~(AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top));
			var sourceQuery = (IQueryable<EntityType>)destinationQuery.Provider.Execute(destinationQuery.Expression);

			if (options.Count != null)
			{
				var properties = options.Request.ODataFeature();
				var destinationQueryWithoutExpands = query.UseAsDataSource(mapper).For<EntityTypeRest>();
				var countQuery = options.ApplyTo(destinationQueryWithoutExpands, settings, AllowedQueryOptions.Count | AllowedQueryOptions.Expand | AllowedQueryOptions.Select | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top);
				if (Request.IsCountRequest())
				{
					return new ObjectResult(countQuery.Count());
				}

				properties.TotalCount = countQuery.Count();
			}

			if (key.HasValue)
			{
				var entityType = sourceQuery.SingleOrDefault(x => x.UId == key);
				if (entityType != null)
				{
					return Ok(mapper.Map<EntityTypeRest>(entityType));
				}

				return NotFound();
			}

			return Ok(mapper.Map<IEnumerable<EntityTypeRest>>(sourceQuery));
		}
	}
}
