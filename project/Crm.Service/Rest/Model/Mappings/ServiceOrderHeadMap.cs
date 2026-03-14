namespace Crm.Service.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Service.BackgroundServices;
	using Crm.Service.Model;

	using Main.Rest.Model;

	public class ServiceOrderHeadMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ServiceOrderHead, DocumentGeneratorEntry>()
				.IncludeBase<object, DocumentGeneratorEntry>()
				.ForMember(x => x.ErrorMessage, m => m.MapFrom((source, dest, member, context) =>
				{
					if (Equals(context.Items["DocumentGenerator"], typeof(ServiceOrderDocumentSaverAgent).FullName))
					{
						return source.ReportSavingError;
					}
					if (Equals(context.Items["DocumentGenerator"], typeof(ServiceOrderReportSenderAgent).FullName))
					{
						return source.ReportSendingError;
					}
					return null;
				}))
				;
		}
	}
}
