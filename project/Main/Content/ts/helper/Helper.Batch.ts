export type MethodName = "first" | "toArray"

/**
 * @example
	const articles = ko.observableArray<Crm.Article.Rest.Model.CrmArticle_Article>([]);
	const article = ko.observable<Crm.Article.Rest.Model.CrmArticle_Article>(null);
	const queries = [
		createQuery(
			window.database.CrmArticle_Article.filter("it.Id === this.id"),
			"toArray",
			result => {
				articles(result);
			}
		),
		createQuery(
			window.database.CrmArticle_Article.filter("it.Id === this.id"),
			"first",
			result => {
				article(result)
			}
		),
	];
	await window.Helper.Batch.Execute(queries)
 */
export function createQuery<TEntity, TMethodName extends MethodName>(
	queryable: $data.Queryable<TEntity>,
	method: TMethodName,
	handler: (result: TMethodName extends "first" ? TEntity : TEntity[]) => void
) {
	return {queryable, method, handler};
}

export type TQueries<TEntity, TMethodName extends MethodName> =
	$data.Queryable<TEntity>[]
	| { queryable: $data.Queryable<TEntity>, method: TMethodName | string, handler: (result: any) => unknown }[]

export class HelperBatch {
	static async Execute<TEntity, TMethodName extends MethodName>(queries: TQueries<TEntity, TMethodName>): Promise<any> {
		const promises = [];
		const results = [];
		let start = 0;
		// @ts-ignore
		let limit = window.database.storageProvider.supportedContextOperation?.batchExecuteQuery ? 100 : 1;
		const getBatch = () => queries.slice(start, Math.min(start + limit, queries.length));
		let batch: any = getBatch();
		while (batch.length > 0) {
			const scopedStart = start;
			const scopedBatch = batch;
			const promise = window.database.batchExecuteQuery(scopedBatch).then(function(x) {
				x.forEach(function(result, i) {
					results[scopedStart + i] = result;
					const query = scopedBatch[i];
					if (query.handler) {
						const promise = query.handler(result);
						if (promise) {
							promises.push(promise);
						}
					}
				});
			});
			promises.push(promise);
			start += batch.length;
			batch = getBatch();
		}
		try {
			await Promise.all(promises);
			if (queries.length !== results.length) {
				throw new Error("batching went wrong, result count does not match query count");
			}
			return results;
		} catch (e) {
			window.Log.error("batch failed", JSON.stringify(queries), e);
		}
	}
}
