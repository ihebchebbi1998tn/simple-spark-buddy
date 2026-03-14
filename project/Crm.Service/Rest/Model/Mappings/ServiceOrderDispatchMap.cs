namespace Crm.Service.Rest.Model.Mappings
{
	using System;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.Globalization.Resource;
	using Crm.Service.BackgroundServices;
	using Crm.Service.Model;

	using Main.Rest.Model;

	public class ServiceOrderDispatchMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceOrderDispatch, DocumentGeneratorEntry>()
				.IncludeBase<object, DocumentGeneratorEntry>()
				.ForMember(x => x.ErrorMessage, m => m.MapFrom((source, dest, member, context) =>
				{
					if (Equals(context.Items["DocumentGenerator"], typeof(DispatchDocumentSaverAgent).FullName))
					{
						return source.ReportSavingError;
					}
					if (Equals(context.Items["DocumentGenerator"], typeof(DispatchReportSenderAgent).FullName))
					{
						return source.ReportSendingError;
					}
					return null;
				}))
				;

			mapper.CreateMap<ServiceOrderDispatch, TimelineEvent>()
				.ForMember(x => x.Id, m => m.MapFrom(x => x.Id))
				.ForMember(x => x.Summary, m => m.MapFrom(x => x.CalendarDescription))
				.ForMember(x => x.Description, m => m.MapFrom((source, dest, member, context) => source.GetCalenderBodyText(context.GetService<IResourceManager>())))
				.ForMember(x => x.IsAllDay, m => m.MapFrom(x => false))
				.ForMember(x => x.Start, m => m.MapFrom(x => x.Date))
				.ForMember(x => x.End, m => m.MapFrom(x => x.EndDate))
				.ForMember(x => x.Location, m => m.MapFrom(x => x.OrderHead.ZipCode != null || x.OrderHead.City != null || x.OrderHead.Street != null ? String.Format("{0} {1}, {2}", x.OrderHead.ZipCode, x.OrderHead.City, x.OrderHead.Street) : String.Empty))
				;
		}
	}
}
