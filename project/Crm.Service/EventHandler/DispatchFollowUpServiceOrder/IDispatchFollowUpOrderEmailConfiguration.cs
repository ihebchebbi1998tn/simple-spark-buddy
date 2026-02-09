namespace Crm.Service.EventHandler.DispatchFollowUpServiceOrder
{
	using Crm.Library.AutoFac;
	using Crm.Service.Model;

	public interface IDispatchFollowUpOrderEmailConfiguration : IDependency
	{
		string [] GetRecipients (ServiceOrderDispatch dispatch);
		string GetSubject(ServiceOrderDispatch dispatch);
		string GetEmailText(ServiceOrderDispatch dispatch);
	}
}