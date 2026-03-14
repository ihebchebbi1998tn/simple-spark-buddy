namespace Crm.Service.Team.Model
{
		using System;

		using Crm.Article.Model;
		using Crm.Library.BaseModel;
		using Crm.Library.BaseModel.Interfaces;
		using Crm.Library.Model;

		public class ArticleUserGroupRelationship : EntityBase<Guid>, ISoftDelete
	{
		public virtual Article Article { get; set; }
		public virtual Guid ArticleKey { get; set; }
		public virtual Usergroup UserGroup { get; set; }
		public virtual Guid UserGroupKey { get; set; }
		public virtual DateTime? From { get; set; }
		public virtual DateTime? To { get; set; }

	}
}
