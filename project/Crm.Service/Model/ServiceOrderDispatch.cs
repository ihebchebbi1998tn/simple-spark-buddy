namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model;
	using Crm.Service.Model.Lookup;

	using Main.Helpers;

	public class ServiceOrderDispatch : EntityBase<Guid>, ISoftDelete
	{
		public virtual string DispatchNo { get; set; }
		public virtual Guid OrderId { get; set; }
		public virtual ServiceOrderHead OrderHead { get; set; }

		public virtual string CalendarDescription
		{
			get { return OrderHead != null && DispatchedUser != null ? OrderHead.OrderNo + " - " + DispatchedUser.DisplayName : String.Empty; }
		}
		public virtual string GetCalenderBodyText(IResourceManager resourceManager)
		{
			var tokens = new List<string>();

			if (OrderHead.CustomerContact != null)
			{
				tokens.Add(String.Format("{0}: {1}", resourceManager.GetTranslation("Customer"), OrderHead.CustomerContact.Name));
			}
			if (!string.IsNullOrWhiteSpace(OrderHead.AffectedInstallation?.InstallationNo))
			{
				tokens.Add(String.Format("{0}: {1}", resourceManager.GetTranslation("Installation"), OrderHead.AffectedInstallation?.InstallationNo));
			}
			if (DispatchedUser != null)
			{
				tokens.Add(String.Format("{0}: {1}", resourceManager.GetTranslation("Technician"), DispatchedUser.DisplayName));
			}

			tokens.Add(String.Format("{0}: {1} {2}", resourceManager.GetTranslation("Date"), Date.ToLocalTime().ToShortDateString(), Date.ToLocalTime().TimeOfDay));

			return String.Join(Environment.NewLine, tokens);
		}
		public virtual Guid? CurrentServiceOrderTimeId { get; set; }
		public virtual ServiceOrderTime CurrentServiceOrderTime { get; set; }
		public virtual string TimeZone { get; set; }
		public virtual DateTime Date { get; set; }
		public virtual bool IsFixed { get; set; }
		public virtual string Remark { get; set; }
		public virtual string RequiredOperations { get; set; }
		public virtual bool FollowUpServiceOrder { get; set; }
		public virtual string FollowUpServiceOrderRemark { get; set; }
		public virtual string Diagnosis { get; set; }
		public virtual double? LatitudeOnDispatchStart { get; set; }
		public virtual double? LongitudeOnDispatchStart { get; set; }
		public virtual string ReportSendingError { get; set; }
		public virtual bool ReportSent { get; set; }
		public virtual bool ReportSaved { get; set; }
		public virtual string ReportSavingError { get; set; }

		public virtual string ComponentKey { get; set; }
		public virtual Component Component
		{
			get { return ComponentKey != null ? LookupManager.Get<Component>(ComponentKey) : null; }
		}
		public virtual string StatusKey { get; set; }
		public virtual ServiceOrderDispatchStatus Status
		{
			get { return StatusKey != null ? LookupManager.Get<ServiceOrderDispatchStatus>(StatusKey) : null; }
		}

		public virtual string RejectReasonKey { get; set; }
		public virtual ServiceOrderDispatchRejectReason RejectReason
		{
			get { return RejectReasonKey != null ? LookupManager.Get<ServiceOrderDispatchRejectReason>(RejectReasonKey) : null; }
		}
		public virtual string RejectRemark { get; set; }

		public virtual string CancellationReasonKey { get; set; }

		public virtual ServiceOrderDispatchCancellationReason CancellationReason
		{
			get { return CancellationReasonKey != null ? LookupManager.Get<ServiceOrderDispatchCancellationReason>(CancellationReasonKey) : null; }
		}

		public virtual string CancellationRemark { get; set; }

		public virtual string DispatchedUsername { get; set; }
		public virtual User DispatchedUser { get; set; }

		public virtual IList<User> Users { get; set; }

		public virtual string SignatureContactName { get; set; }
		public virtual string SignatureJson { get; set; }
		public virtual DateTime? SignatureDate { get; set; }
		public virtual bool SignPrivacyPolicyAccepted { get; set; }

		public virtual string SignatureTechnicianName { get; set; }
		public virtual string SignatureTechnicianJson { get; set; }
		public virtual DateTime? SignatureTechnicianDate { get; set; }

		public virtual string SignatureOriginatorName { get; set; }
		public virtual string SignatureOriginatorJson { get; set; }
		public virtual DateTime? SignatureOriginatorDate { get; set; }
		public virtual DateTime? CloseDate { get; set; }
		public virtual DateTime EndDate { get; set; }
		public virtual int? NetWorkMinutes { get; set; }
		public virtual string InfoForTechnician { get; set; }

		public virtual int? SignatureWidth
		{
			get { return IsSignedByCustomer ? (int?)SignatureToImage.GetSignatureWidth(SignatureJson) : null; }
		}
		public virtual int? SignatureHeight
		{
			get { return IsSignedByCustomer ? (int?)SignatureToImage.GetSignatureHeight(SignatureJson) : null; }
		}
		public virtual bool IsSignedByCustomer
		{
			get { return !String.IsNullOrEmpty(SignatureJson); }
		}
		public virtual byte[] SignatureByteArray
		{
			get { return IsSignedByCustomer ? SignatureToImage.SigJsonToByteArray(SignatureJson) : null; }
		}
		public virtual ICollection<ServiceOrderDispatchReportRecipient> ReportRecipients { get; set; }
		public virtual ICollection<ServiceOrderTimeDispatch> ServiceOrderTimeDispatches { get; set; }
		public virtual ICollection<ServiceOrderTimePosting> TimePostings { get; set; }
		public virtual ICollection<ServiceOrderExpensePosting> ExpensePostings { get; set; }
		public virtual ICollection<ServiceOrderErrorType> ServiceOrderErrors { get; set; }
		public virtual ICollection<ServiceOrderMaterial> ServiceOrderMaterial { get; set; }

		public ServiceOrderDispatch()
		{
			ReportRecipients = new List<ServiceOrderDispatchReportRecipient>();
			ServiceOrderTimeDispatches = new List<ServiceOrderTimeDispatch>();
			TimePostings = new List<ServiceOrderTimePosting>();
			ExpensePostings = new List<ServiceOrderExpensePosting>();
			ServiceOrderErrors = new List<ServiceOrderErrorType>();
			ServiceOrderMaterial = new List<ServiceOrderMaterial>();
		}
	}
}
