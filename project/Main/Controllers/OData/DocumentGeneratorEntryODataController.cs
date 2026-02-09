namespace Main.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Dynamic.Core;
	using System.Reflection;

	using AutoMapper.Extensions.ExpressionMapping;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Services.Interfaces;

	using Main.Model;
	using Main.Rest.Model;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Extensions;
	using Microsoft.AspNetCore.OData.Query;

	using Quartz;
	using Quartz.Impl.Matchers;

	[ControllerName("Main_DocumentGeneratorEntry")]
	public class DocumentGeneratorEntryODataController : ODataControllerEx
	{
		private static readonly MethodInfo GetGenericMethodInfo = typeof(DocumentGeneratorEntryODataController)
			.GetMethods(BindingFlags.Instance | BindingFlags.Public)
			.Single(x => x.Name == nameof(GetGeneric) && x.IsGenericMethod);
		private readonly IODataMapper mapper;
		private readonly IEnumerable<IDocumentGeneratorService> documentGeneratorServices;
		private readonly IScheduler scheduler;
		public DocumentGeneratorEntryODataController(IODataMapper mapper, IEnumerable<IDocumentGeneratorService> documentGeneratorServices, IScheduler scheduler)
		{
			this.mapper = mapper;
			this.documentGeneratorServices = documentGeneratorServices;
			this.scheduler = scheduler;
		}

		[HttpGet]
		[RequiredPermission(nameof(DocumentGeneratorEntry), Group = PermissionGroup.WebApiRead)]
		public virtual IActionResult GetGeneratorServices() => GetGeneratorServices(null);
		[HttpGet]
		[RequiredPermission(nameof(DocumentGeneratorEntry), Group = PermissionGroup.WebApiRead)]
		public virtual IActionResult GetGeneratorServices(string Filter)
		{
			var jobGroups = scheduler.GetJobGroupNames();
			var allJobs = new List<IJobDetail>();
			foreach (string group in jobGroups.Result)
			{
				var groupMatcher = GroupMatcher<JobKey>.GroupEquals(group);
				var jobKeys = scheduler.GetJobKeys(groupMatcher);
				allJobs.AddRange(jobKeys.Result.Select(jobKey => scheduler.GetJobDetail(jobKey).Result));
			}
			var backgroundServices = allJobs.Where(x => x.JobType.Name != "FileScanJob").OrderBy(x => x.Key.Name);
			List<JobTriggerMatch> allJobsWithTriggers = backgroundServices.Select(x => new JobTriggerMatch(x, scheduler)).ToList();

			var filteredGeneratorServices = documentGeneratorServices.Where(s => allJobsWithTriggers.Any(j => j.Job.JobType == s.GetType() && j.Triggers.Count > 0));
			var generatorServices = filteredGeneratorServices.Select(x => new { Type = x.GetType().FullName, Name = allJobs.FirstOrDefault(j => j.JobType == x.GetType())?.Key.Name }).Where(x => string.IsNullOrWhiteSpace(Filter) || x.Type.Contains(Filter, StringComparison.InvariantCultureIgnoreCase)).ToArray();
			
			return Ok(generatorServices);
		}
		[HttpGet]
		[RequiredPermission(nameof(DocumentGeneratorEntry), Group = PermissionGroup.WebApiRead)]
		public virtual IActionResult GetFailed(ODataQueryOptions<DocumentGeneratorEntry> options, string GeneratorService) => Get(options, GeneratorService, true);
		[HttpGet]
		[RequiredPermission(nameof(DocumentGeneratorEntry), Group = PermissionGroup.WebApiRead)]
		public virtual IActionResult GetPending(ODataQueryOptions<DocumentGeneratorEntry> options, string GeneratorService) => Get(options, GeneratorService, false);
		public virtual IActionResult Get(ODataQueryOptions<DocumentGeneratorEntry> options, string GeneratorService, bool failed)
		{
			var service = documentGeneratorServices.SingleOrDefault(x => x.GetType().FullName == GeneratorService);
			if (service == null)
			{
				return new EmptyResult();
			}

			var query = failed ? service.GetFailedDocuments() : service.GetPendingDocuments();
			var queryType = query.GetType().GenericTypeArguments[0];
			var method = GetGenericMethodInfo.MakeGenericMethod(queryType);
			return (IActionResult)method.Invoke(this, new object[] { options, query, service });
		}
		public virtual IActionResult GetGeneric<T>(ODataQueryOptions<DocumentGeneratorEntry> options, IQueryable<T> query, IDocumentGeneratorService documentGeneratorService)
		{
			var settings = new ODataQuerySettings { EnableConstantParameterization = false, HandleNullPropagation = HandleNullPropagationOption.False };

			var projectedQuery = query
				.UseAsDataSource(mapper)
				.For<DocumentGeneratorEntry>();
			var destinationQuery = options.ApplyTo(projectedQuery, settings, AllowedQueryOptions.All & ~(AllowedQueryOptions.Filter | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top));
			var sourceQuery = destinationQuery.Provider.Execute(destinationQuery.Expression);

			if (options.Count != null)
			{
				var properties = options.Request.ODataFeature();
				var destinationQueryWithoutExpands = query.UseAsDataSource(mapper).For<DocumentGeneratorEntry>();
				var countQuery = options.ApplyTo(destinationQueryWithoutExpands, settings, AllowedQueryOptions.Count | AllowedQueryOptions.Expand | AllowedQueryOptions.Select | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top);
				if (Request.IsCountRequest())
				{
					return new ObjectResult(countQuery.Count());
				}

				properties.TotalCount = countQuery.Count();
			}

			return Ok(mapper.Map<IEnumerable<DocumentGeneratorEntry>>(sourceQuery, o => o.Items["DocumentGenerator"] = documentGeneratorService.GetType().FullName));
		}
	}
}
