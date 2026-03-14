namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Model;
	using Crm.Service.Model.Lookup;

	public class ServiceObject : Folder
	{
		public virtual string ObjectNo { get; set; }
		public virtual string CategoryKey { get; set; }
		public virtual Guid StandardAddressId { get; set; }
		public virtual ServiceObjectCategory Category
		{
			get { return CategoryKey != null ? LookupManager.Get<ServiceObjectCategory>(CategoryKey) : null; }
		}

		public virtual ICollection<Installation> Installations { get; set; }
		public override string ToString()
		{
			return String.IsNullOrWhiteSpace(ObjectNo) ? Name : String.Format("{0} - {1}", ObjectNo, Name);
		}

		public ServiceObject()
		{
			Installations = new List<Installation>();
		}
	}
}
