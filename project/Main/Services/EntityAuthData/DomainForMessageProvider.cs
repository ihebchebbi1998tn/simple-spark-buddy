using Crm.Library.Services.Interfaces;
using Crm.Library.Unicore;

using System;

namespace Main.Services.EntityAuthData
{
	public class DomainForMessageProvider : IDomainForTypeProvider<Main.Model.Message>
	{
		public virtual Guid? GetDomain(Main.Model.Message entity) => UnicoreDefaults.CommonDomainId;
	}
}
