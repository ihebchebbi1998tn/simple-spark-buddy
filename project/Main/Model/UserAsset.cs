namespace Main.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Model;

	using Main.Model.Lookups;

	public class UserAsset : EntityBase<Guid>, ISoftDelete
	{
		public virtual string Username { get; set; }
		public virtual User User { get; set; }
		public virtual string AssetKey { get; set; }
		public virtual Asset Asset
		{
			get { return AssetKey != null ? LookupManager.Get<Asset>(AssetKey) : null; }
		}
		public virtual DateTime? ValidFrom { get; set; }
		public virtual DateTime? ValidTo { get; set; }
		public virtual int? DaysToNotifyBeforeExpiration { get; set; }
	}
}
