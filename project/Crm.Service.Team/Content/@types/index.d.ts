import {ArticleDetailsTeamsTabViewModel as ArticleDetailsTeamsTabViewModelType} from "../ts/ArticleDetailsTeamsTabViewModel";
import {ArticleUserGroupRelationshipEditModalViewModel as ArticleUserGroupRelationshipEditModalViewModelType} from "../ts/ArticleUserGroupRelationshipEditModalViewModel";
import {ServiceTeamListIndexViewModel as ServiceTeamListIndexViewModelType} from "../ts/ServiceTeamListIndexViewModel";
import {UserGroupDetailsArticlesTabViewModel as UserGroupDetailsArticlesTabViewModelType} from "../ts/UserGroupDetailsArticlesTabViewModel";
import {UserGroupDetailsDispatchesTabViewModel as UserGroupDetailsDispatchesTabViewModelType } from "../ts/UserGroupDetailsDispatchesTabViewModel";

declare global {
	namespace Crm {
		namespace Article {
			namespace ViewModels {
				let ArticleDetailsTeamsTabViewModel: typeof ArticleDetailsTeamsTabViewModelType;
			}
		}
	}
	namespace Crm {
		namespace Service {
			namespace Team {
				namespace ViewModels {
					let ArticleUserGroupRelationshipEditModalViewModel: typeof ArticleUserGroupRelationshipEditModalViewModelType;
					let ServiceTeamListIndexViewModel: typeof ServiceTeamListIndexViewModelType;
				}
			}
		}
	}
	namespace Main {
		namespace ViewModels {
			let UserGroupDetailsArticlesTabViewModel: typeof UserGroupDetailsArticlesTabViewModelType;
			let UserGroupDetailsDispatchesTabViewModel: typeof UserGroupDetailsDispatchesTabViewModelType;
		}
	}
}