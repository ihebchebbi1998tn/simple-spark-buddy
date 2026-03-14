using Crm.Library.Services.Interfaces;
using Crm.Service.Model;

using System;

namespace Crm.Service.Services.EntityAuthData
{
	public class DomainForServiceOrderTimeProvider : IDomainForTypeProvider<ServiceOrderTime>
	{
		public virtual Guid? GetDomain(ServiceOrderTime entity) => entity?.ServiceOrderHead?.AuthData?.DomainId;
	}
}
