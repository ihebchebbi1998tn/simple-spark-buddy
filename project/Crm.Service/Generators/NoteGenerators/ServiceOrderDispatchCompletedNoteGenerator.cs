namespace Crm.Service.Generators.NoteGenerators
{
	using System;

	using Crm.Generators.NoteGenerators.Infrastructure;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Model.Notes;
	using Crm.Service.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Extensions;
	using Crm.Service.Model.Notes;

	using LMobile.Unicore;

	using Main.Infrastructure;

	using Microsoft.AspNetCore.Routing;

	public class ServiceOrderDispatchCompletedNoteGenerator : NoteGenerator<ServiceOrderDispatchCompletedEvent>
	{
		private readonly Func<ServiceOrderDispatchCompletedNote> noteFactory;
		private readonly Func<LinkResource> linkFactory;
		private readonly IResourceManager resourceManager;
		private readonly IAbsolutePathHelper absolutePathHelper;
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		public ServiceOrderDispatchCompletedNoteGenerator(IRepositoryWithTypedId<Note, Guid> noteRepository, Func<ServiceOrderDispatchCompletedNote> noteFactory, Func<LinkResource> linkFactory, IResourceManager resourceManager, IAbsolutePathHelper absolutePathHelper, IClientSideGlobalizationService clientSideGlobalizationService)
			: base(noteRepository)
		{
			this.noteFactory = noteFactory;
			this.linkFactory = linkFactory;
			this.resourceManager = resourceManager;
			this.absolutePathHelper = absolutePathHelper;
			this.clientSideGlobalizationService = clientSideGlobalizationService;
		}
		public override Note GenerateNote(ServiceOrderDispatchCompletedEvent e)
		{
			var note = noteFactory();
			note.IsActive = true;
			note.ContactId = e.ServiceOrderDispatch.OrderId;
			note.GetExtension<NoteExtensions>().DispatchId = e.ServiceOrderDispatch.Id;
			note.Contact = e.ServiceOrderDispatch.OrderHead;
			note.ContactType = "Dispatch";
			note.ContactName = e.ServiceOrderDispatch.OrderHead.OrderNo;
			note.Text = e.ServiceOrderDispatch.StatusKey;
			note.Meta = string.Join(";", e.ServiceOrderDispatch.DispatchedUsername);
			note.Plugin = ServicePlugin.PluginName;
			note.Links.Add(GetDispatchReportLink(e.ServiceOrderDispatch));
			return note;
		}
		public virtual LinkResource GetDispatchReportLink(ServiceOrderDispatch serviceOrderDispatch)
		{
			var dispatchReportLink = linkFactory();
			var language = clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault();
			dispatchReportLink.Description = resourceManager.GetTranslation("DispatchReport", language);
			dispatchReportLink.Url = absolutePathHelper.GetPath("GetReport", "Dispatch", new RouteValueDictionary { { "plugin", ServicePlugin.PluginName }, { "dispatchId", serviceOrderDispatch.Id }, { "orderNo", serviceOrderDispatch.OrderHead.OrderNo } });
			return dispatchReportLink;
		}
	}
}
