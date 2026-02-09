namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class ServiceOrderExpensePostingMap : EntityClassMapping<ServiceOrderExpensePosting>
	{
		public ServiceOrderExpensePostingMap()
		{
			Schema("SMS");
			Table("ServiceOrderExpensePostings");

			Id(x => x.Id, map =>
			{
				map.Column("ServiceOrderExpensePostingId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.Date);

			Property(x => x.Amount);
			Property(x => x.Description);
			Property(x => x.CurrencyKey);
			Property(x => x.ExpenseTypeKey, map => map.Column("ExpenseType"));
			Property(x => x.IsExported);
			Property(x => x.IsClosed, map => map.Column("Closed"));
			Property(x => x.CostCenterKey, m => m.Column("CostCenter"));
			Property(x => x.FileResourceKey);
			ManyToOne(
				x => x.FileResource,
				map =>
				{
					map.Column("FileResourceKey");
					map.Fetch(FetchKind.Select);
					map.Lazy(LazyRelation.Proxy);
					map.Cascade(Cascade.Remove);
					map.Insert(false);
					map.Update(false);
				});


			Property(x => x.OrderId);
			ManyToOne(x => x.ServiceOrderHead,
				m =>
				{
					m.Column("OrderId");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});

			Property(x => x.ResponsibleUser);
			ManyToOne(x => x.ResponsibleUserObject,
				m =>
				{
					m.Column("ResponsibleUser");
					m.Fetch(FetchKind.Select);
					m.Insert(false);
					m.Update(false);
				});

			Property(x => x.DispatchId);
			ManyToOne(x => x.ServiceOrderDispatch,
			m =>
			{
				m.Column("DispatchId");
				m.Fetch(FetchKind.Select);
				m.Insert(false);
				m.Update(false);
			});
			Property(x => x.PerDiemReportId);
			ManyToOne(
				x => x.PerDiemReport,
				map =>
				{
					map.Column("PerDiemReportId");
					map.Fetch(FetchKind.Select);
					map.Lazy(LazyRelation.Proxy);
					map.Cascade(Cascade.None);
					map.Insert(false);
					map.Update(false);
				});

			Property(x => x.ServiceOrderTimeId);
			ManyToOne(x => x.ServiceOrderTime, map =>
			{
				map.Column("ServiceOrderTimeId");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});


			Property(x => x.ArticleId);
			ManyToOne(x => x.Article, map =>
			{
				map.Column("ArticleId");
				map.Fetch(FetchKind.Select);
				map.Lazy(LazyRelation.Proxy);
				map.Cascade(Cascade.None);
				map.Insert(false);
				map.Update(false);
			});
			Property(x => x.ItemNo);

		}
	}
}
