namespace Crm.Service.Services
{
	using System;
	using System.Linq;
	using System.Reflection;

	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Model;
	using Crm.Service.Model;

	using LMobile.Unicore.NHibernate;

	using Microsoft.AspNetCore.OData.Query;

	public class ODataQueryCurrentServiceOrderTimeOrder : IODataQueryFunction, IDependency
	{
		protected static MethodInfo OrderByForServiceCaseInfo = typeof(ODataQueryCurrentServiceOrderTimeOrder)
			.GetMethod(nameof(OrderByForServiceCase), BindingFlags.Instance | BindingFlags.NonPublic);
		protected static MethodInfo OrderByForTimePostingInfo = typeof(ODataQueryCurrentServiceOrderTimeOrder)
			.GetMethod(nameof(OrderByForTimePosting), BindingFlags.Instance | BindingFlags.NonPublic);
		protected static MethodInfo OrderByForMaterialInfo = typeof(ODataQueryCurrentServiceOrderTimeOrder)
			.GetMethod(nameof(OrderByForMaterial), BindingFlags.Instance | BindingFlags.NonPublic);
		protected static MethodInfo OrderByForDocumentsInfo = typeof(ODataQueryCurrentServiceOrderTimeOrder)
			.GetMethod(nameof(OrderByForDocuments), BindingFlags.Instance | BindingFlags.NonPublic);
		protected static MethodInfo OrderByForInstallationsInfo = typeof(ODataQueryCurrentServiceOrderTimeOrder)
			.GetMethod(nameof(OrderByForInstallations), BindingFlags.Instance | BindingFlags.NonPublic);
		protected static MethodInfo OrderByForJobsInfo = typeof(ODataQueryCurrentServiceOrderTimeOrder)
			.GetMethod(nameof(OrderByForJobs), BindingFlags.Instance | BindingFlags.NonPublic);
		protected virtual IQueryable<ServiceCase> OrderByForServiceCase(IQueryable<ServiceCase> query, Guid? currentServiceOrderTimeId)
		{
			if (currentServiceOrderTimeId.HasValue && currentServiceOrderTimeId.Value != Guid.Empty)
			{
				query = query.OrderByDescending(x => x.ServiceOrderTimeId == currentServiceOrderTimeId.Value);
			}
			return query.OrderByDescending(x => x.ServiceOrderTimeId == null);
		}
		protected virtual IQueryable<ServiceOrderTimePosting> OrderByForTimePosting(IQueryable<ServiceOrderTimePosting> query, Guid? currentServiceOrderTimeId)
		{
			if (currentServiceOrderTimeId.HasValue && currentServiceOrderTimeId.Value != Guid.Empty)
			{
				query = query.OrderByDescending(x => x.OrderTimesId == currentServiceOrderTimeId.Value);
			}
			return query.OrderByDescending(x => x.OrderTimesId == null);
		}
		protected virtual IQueryable<ServiceOrderMaterial> OrderByForMaterial(IQueryable<ServiceOrderMaterial> query, Guid? currentServiceOrderTimeId)
		{
			if (currentServiceOrderTimeId.HasValue && currentServiceOrderTimeId.Value != Guid.Empty)
			{
				query = query.OrderByDescending(x => x.ServiceOrderTimeId == currentServiceOrderTimeId.Value);
			}
			return query.OrderByDescending(x => x.ServiceOrderTimeId == null);
		}
		protected virtual IQueryable<DocumentAttribute> OrderByForDocuments(IQueryable<DocumentAttribute> query, Guid? currentServiceOrderTimeId)
		{
			if (currentServiceOrderTimeId.HasValue && currentServiceOrderTimeId.Value != Guid.Empty)
			{
				query = query.OrderByDescending(x => x.ModelExtension<DocumentAttributeExtension, Guid?>(e => e.ServiceOrderTimeId) == currentServiceOrderTimeId.Value);
			}
			return query.OrderByDescending(x => x.ModelExtension<DocumentAttributeExtension, Guid?>(e => e.ServiceOrderTimeId) == null)
				.ThenBy(x => x.ModelExtension<DocumentAttributeExtension, string>(e => e.ServiceOrderTimePosNo));
		}
		protected virtual IQueryable<Installation> OrderByForInstallations(IQueryable<Installation> query, Guid? currentServiceOrderTimeInstallationId)
		{
			if (currentServiceOrderTimeInstallationId.HasValue && currentServiceOrderTimeInstallationId.Value != Guid.Empty)
			{
				query = query.OrderByDescending(x => x.Id == currentServiceOrderTimeInstallationId.Value);
			}
			return query;
		}
		protected virtual IQueryable<ServiceOrderTime> OrderByForJobs(IQueryable<ServiceOrderTime> query, Guid? currentServiceOrderTimeId)
		{
			if (currentServiceOrderTimeId.HasValue && currentServiceOrderTimeId.Value != Guid.Empty)
			{
				query = query.OrderByDescending(x => x.Id == currentServiceOrderTimeId.Value);
			}
			return query;
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			MethodInfo orderBy;
			switch (typeof(T).Name)
			{
				case nameof(ServiceCase):
					orderBy = OrderByForServiceCaseInfo;
					break;
				case nameof(ServiceOrderTimePosting):
					orderBy = OrderByForTimePostingInfo;
					break;
				case nameof(ServiceOrderMaterial):
					orderBy = OrderByForMaterialInfo;
					break;
				case nameof(DocumentAttribute):
					orderBy = OrderByForDocumentsInfo;
					break;
				case nameof(Installation):
					orderBy = OrderByForInstallationsInfo;
					break;
				case nameof(ServiceOrderTime):
					orderBy = OrderByForJobsInfo;
					break;
				default: return query;
			}
			const string parameterName = "orderByCurrentServiceOrderTime";
			var parameters = options.Request.Query;
			if (parameters.Keys.Contains(parameterName))
			{
				Guid? id = null;
				if (Guid.TryParse(options.Request.GetQueryParameter(parameterName), out var guid))
				{
					id = guid;
				}
				return (IQueryable<T>)orderBy.Invoke(this, new object[] { query, id });
			}
			return query;
		}
	}
}
