namespace Main.Controllers
{
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Modularization;
	using Crm.Library.Rest;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class PostingController : Controller
	{
		private readonly IRepository<Posting> postingRepository;
		private readonly IRestSerializer restSerializer;
		public PostingController(IRepository<Posting> postingRepository, IRestSerializer restSerializer)
		{
			this.postingRepository = postingRepository;
			this.restSerializer = restSerializer;
		}

		public virtual ActionResult GetSerializedEntity(int id)
		{
			var posting = postingRepository.Get(id);
			return Json(posting.SerializedEntity);
		}

		public virtual ActionResult Save(int id, string serializedEntity)
		{
			var posting = postingRepository.Get(id);
			if (posting.PostingState == PostingState.Pending || posting.PostingState == PostingState.Failed || posting.PostingState == PostingState.Blocked)
			{
				posting.PostingState = PostingState.Pending;
				posting.Retries = 0;
				posting.RetryAfter = null;
				posting.SerializedEntity = serializedEntity;
				posting.StateDetails = null;
				if (posting.TransactionId != null)
				{
					foreach (var transactionPosting in postingRepository.GetAll().Where(x => x.TransactionId == posting.TransactionId && (x.PostingState == PostingState.Pending || x.PostingState == PostingState.Failed || x.PostingState == PostingState.Blocked)))
					{
						transactionPosting.PostingState = PostingState.Pending;
						transactionPosting.Retries = 0;
						transactionPosting.RetryAfter = null;
						transactionPosting.StateDetails = null;
						postingRepository.SaveOrUpdate(transactionPosting);
					}
				}
				postingRepository.SaveOrUpdate(posting);
			}
			var serializedPosting = restSerializer.SerializeAsJson(posting);
			return Content(serializedPosting);
		}

		public virtual ActionResult Skip(int id)
		{
			var posting = postingRepository.Get(id);
			if (posting.PostingState == PostingState.Pending || posting.PostingState == PostingState.Failed || posting.PostingState == PostingState.Blocked)
			{
				posting.PostingState = PostingState.Skipped;
				postingRepository.SaveOrUpdate(posting);
			}
			var serializedPosting = restSerializer.SerializeAsJson(posting);
			return Content(serializedPosting);
		}
		public virtual ActionResult DetailsTemplate() => PartialView();
		public virtual ActionResult Edit() => PartialView();
		[RenderAction("PostingDetailsMaterialTabHeader", Priority = 80)]
		public virtual ActionResult MaterialPostingsTabHeader() => PartialView();

		[RenderAction("PostingDetailsMaterialTab", Priority = 80)]
		public virtual ActionResult MaterialPostingsTab() => PartialView();

		[RenderAction("PostingDetailsTopMenu")]
		public virtual ActionResult RetryTransactionTopMenu() => PartialView();
	}
}
