namespace Main.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Dynamic.Core;

	using AutoMapper;
	using AutoMapper.QueryableExtensions;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest.Model;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Extensions;
	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.OData.Edm;
	using Microsoft.OData.UriParser;

	[ControllerName("Main_Posting")]
	public class PostingODataController : ODataControllerEx, IEntityApiController
	{
		private static readonly ISet<string> TransactionPropertyNames = typeof(Transaction).GetProperties().Select(x => x.Name).ToHashSet();
		private readonly ODataModelBuilderHelper modelBuilderHelper;
		private readonly IRepositoryWithTypedId<Posting, Guid> postingRepository;
		private readonly IMapper mapper;
		public Type EntityType => typeof(Posting);
		public PostingODataController(
			ODataModelBuilderHelper modelBuilderHelper,
			IRepositoryWithTypedId<Posting, Guid> postingRepository,
			IMapper mapper //can't use IOdataMapper because of the projections used below (it would fail mapping RestEntity properties)
		)
		{
			this.modelBuilderHelper = modelBuilderHelper;
			this.postingRepository = postingRepository;
			this.mapper = mapper;
		}
		[HttpGet]
		public virtual IActionResult GetTransactions(ODataQueryOptions<PostingRest> options)
		{
			var querySettings = new ODataQuerySettings { EnableConstantParameterization = false, HandleNullPropagation = HandleNullPropagationOption.False, EnsureStableOrdering = false };
			var filterQuery = (IQueryable<PostingRest>)options.ApplyTo(Enumerable.Empty<PostingRest>().AsQueryable(), querySettings, AllowedQueryOptions.All & ~AllowedQueryOptions.Filter);
			var baseQuery = filterQuery.Map(postingRepository.GetAll(), mapper.ConfigurationProvider).Select(x => x.TransactionId).Distinct();
			var groupedQuery = postingRepository.GetAll()
				.Where(x => baseQuery.Contains(x.TransactionId))
				.GroupBy(x => x.TransactionId)
				.Select(x => new Transaction
				{
					CreateDate = x.Min(p => p.CreateDate),
					CreateUser = x.Min(p => p.CreateUser),
					Id = x.Key,
					PostingCount = x.Count(),
					Retries = x.Max(p => p.Retries),
					RetryAfter = x.Max(p => p.RetryAfter),
					TransactionState = x.Min(p => p.PostingState),
				});
			if (options.Count != null)
			{
				options.Request.ODataFeature().TotalCount = postingRepository.GetAll().Where(x => baseQuery.Contains(x.TransactionId)).Select(x => x.TransactionId).Distinct().Count();
			}
			var orderBys = options.OrderBy?.OrderByNodes.OfType<OrderByPropertyNode>()
				.Select(node => $"{node.Property.Name} {(node.Direction == OrderByDirection.Ascending ? "asc" : "desc")}");
			groupedQuery = groupedQuery.OrderBy(string.Join(",", orderBys ?? new[] { $"{nameof(Transaction.CreateDate)} desc" }));
			groupedQuery = groupedQuery.Take(options.Top?.Value ?? 25).Skip(options.Skip?.Value ?? 0);

			//create a SelectExpandClause from the Request, with matching Properties of Transaction
			var selectedPropertyNames = options.SelectExpand?
				.SelectExpandClause
				.SelectedItems
				.OfType<PathSelectItem>()
				.Select(x => x.SelectedPath.FirstSegment)
				.OfType<PropertySegment>()
				.Select(x => x.Property.Name)
				.Where(TransactionPropertyNames.Contains)
				.ToList();
			if (selectedPropertyNames?.Any() == true)
			{
				var items = new List<SelectItem>();
				var model = Request.GetModel();
				var type = (IEdmComplexType)model.FindDeclaredType($"{typeof(Transaction).Namespace}.{typeof(Transaction).Name}");
				foreach (var propertyName in selectedPropertyNames)
				{
					var property = (IEdmStructuralProperty)type.FindProperty(propertyName);
					var segment = new PropertySegment(property);
					var path = new ODataSelectPath(segment);
					var item = new PathSelectItem(path);
					items.Add(item);
				}
				Request.ODataFeature().SelectExpandClause = new SelectExpandClause(items, false);
			}
			return Ok(mapper.Map<IEnumerable<Transaction>>(groupedQuery));
		}
	}
}
