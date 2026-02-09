namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class ReplenishmentOrderMap : EntityClassMapping<ReplenishmentOrder>
	{
		public ReplenishmentOrderMap()
		{
			Schema("SMS");
			Table("ReplenishmentOrder");

			Id(x => x.Id, map =>
			{
				map.Column("ReplenishmentOrderId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.ResponsibleUser);
			Property(x => x.IsClosed);
			Property(x => x.CloseDate);
			Property(x => x.IsSent);
			Property(x => x.SendingError, m => m.Length(int.MaxValue));
			Property(x => x.ClosedBy);
			Property(x => x.IsExported);
			ManyToOne(x => x.ResponsibleUserObject,
				m =>
				{
					m.Column("ResponsibleUser");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});
			this.EntitySet(x => x.Items, map =>
			{
				map.Key(km => km.Column("ReplenishmentOrderKey"));
				map.Fetch(CollectionFetchMode.Select);
				map.Lazy(CollectionLazy.Lazy);
				map.Cascade(Cascade.Remove);
			}, action => action.OneToMany());
		}
	}
}
