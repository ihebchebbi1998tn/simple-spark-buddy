using System;

using Crm.Library.Services.Interfaces;
using Crm.Service.Model;

namespace Crm.Service.Services.EntityAuthData
{
	public class DomainForServiceOrderHeadProvider : IDomainForTypeProvider<ServiceOrderHead>
	{
		public virtual Guid? GetDomain(ServiceOrderHead entity) => entity?.ServiceContract?.AuthData?.DomainId;
	}
}
