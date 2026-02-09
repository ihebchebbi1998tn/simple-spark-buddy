import {HelperUrl} from "./Helper.Url";
import {namespace} from "../namespace";
import $ from "jquery";
import {mergeDeep, resolveUrl} from "./helper";

type oDataStorageOption = {
	provider: "oData",
	oDataServiceHost: string,
	queryCache: boolean,
	UpdateMethod: string
}

type localStorageOption = {
	provider: "local",
	databaseName: string,
	maxSize: number,
	dbCreation: any,
	queryCache: boolean
}

type indexedDbStorageOption = {
	provider: "indexedDb",
	databaseName: string
}

type webApiStorageOption = {
	provider: "webApi",
}

type StorageOptions = oDataStorageOption | localStorageOption | indexedDbStorageOption | webApiStorageOption

class HelperDatabase {
	static initializePromise: Promise<void> = null;
	static dbDefinition = {};
	static dbSchema = {};
	static converters = {};
	static dbIndicesMultiEntry = {};
	static dbIndicesSingleEntry = {};
	static globalFilterFunctions = [];
	static transactionIdFunctions = {};

	static prepareRequest(requestData): void {
		this.prepareRequest(requestData);
	}

	static applyGlobalFilters(query: { expression: any; context: any; }): void {
		const globalFilters = window.Helper.Database.globalFilterFunctions
			.map(globalFilterFunction => globalFilterFunction())
			.filter(globalFilter => !!globalFilter)
			.map(globalFilter => {
				globalFilter.filterScope = globalFilter.filterScope || {};
				globalFilter.requiredColumns = globalFilter.requiredColumns || [];
				return globalFilter;
			});
		let expressionSource = query.expression;
		let tmp = null;
		let entitySetExpression = null;
		const includeExpressions = [];

		while (expressionSource) {
			if (expressionSource instanceof ($data as any).Expressions.EntitySetExpression) {
				entitySetExpression = expressionSource;
			}
			if (expressionSource instanceof ($data as any).Expressions.IncludeExpression) {
				includeExpressions.unshift(expressionSource);
			}
			Object.defineProperty(expressionSource, "parentExpressionSource", {
				enumerable: false,
				writable: true,
				value: tmp
			});
			tmp = expressionSource;
			expressionSource = expressionSource.source;
		}

		const buildPathFromExpressionTree = (expression: any, path = "") => {
			if (expression instanceof ($data as any).Expressions.ConstantExpression) {
				return expression.value;
			}
			if (expression instanceof ($data as any).Expressions.EntityExpression && expression.selector && expression.selector.lambda !== "it") {
				path = expression.source.selector.associationInfo.FromPropertyName + "." + path;
				return buildPathFromExpressionTree(expression.source.source, path);
			} else if (expression instanceof ($data as any).Expressions.EntitySetExpression) {
				path = expression.source.storageModel.Associations.find(function (it) {
					return it.FromType === (expression.source.entityType || expression.source.elementType) && it.ToType === expression.elementType;
				}).FromPropertyName + "." + path;
				return buildPathFromExpressionTree(expression.source, path);
			} else if (expression instanceof ($data as any).Expressions.FrameOperationExpression) {
				return buildPathFromExpressionTree(expression.source, path);
			}
			return path.replace(/\.$/, "");
		};

		function getFilter(elementType) {
			const elementTypeFilters = globalFilters.filter(function (globalFilter) {
				return globalFilter.requiredColumns.every(elementType.getMemberDefinition.bind(elementType));
			});
			if (elementTypeFilters.length === 0) {
				return null;
			}
			const filterScopes = elementTypeFilters.map(function (elementTypeFilter) {
				return elementTypeFilter.filterScope;
			});
			filterScopes.unshift({});
			return {
				filter: "function(it){return " + elementTypeFilters.map(function (elementTypeFilter) {
					return "(" + elementTypeFilter.filter + ")";
				}).join(" && ") + ";}",
				// @ts-ignore
				filterScope: Object.assign.apply(this, filterScopes)
			};
		}

		function setSourceEntityType(path, expression) {
			for (let j = 0; j < path.length; j++) {
				expression = expression.source;
			}
			expression.entityType = expression.entityType || expression.elementType;
		}

		const entitySetFilter = entitySetExpression != null ? getFilter(entitySetExpression.elementType) : null;
		if (entitySetExpression != null && entitySetFilter != null) {
			entitySetExpression.parentExpressionSource.source = ($data as any).Container.createQueryExpressionCreator(query.context).Visit(($data as any).Container.createFilterExpression(entitySetExpression, ($data as any).Container.createCodeExpression(entitySetFilter.filter, entitySetFilter.filterScope)));
			entitySetExpression.parentExpressionSource = entitySetExpression.parentExpressionSource.source;
		}

		const filterPath = {};
		includeExpressions.forEach(includeExpression => {
			let entitySet = entitySetExpression.instance;
			const context = entitySet.entityContext;
			const includePath = buildPathFromExpressionTree(includeExpression.selector.expression || includeExpression.selector);
			const path = includePath.split(".");

			for (let i = 0; i < path.length; i++) {
				const sm = context._storageModel.getStorageModel(entitySet.elementType);
				if (sm) {
					const associations = sm.Associations;
					const current = associations[path[i]];
					const currentFilter = getFilter(current.ToType);
					const navigationPath = path.slice(0, i + 1).join(".");
					let filter;
					if (current.ToMultiplicity !== "*" && !filterPath[navigationPath] && currentFilter != null) {
						filter = currentFilter.filter.replace(/it\./g, "it." + navigationPath + ".");
						entitySetExpression.parentExpressionSource.source = ($data as any).Container.createQueryExpressionCreator(query.context)
							.Visit(($data as any).Container.createFilterExpression(entitySetExpression, ($data as any).Container.createCodeExpression(filter, currentFilter.filterScope)));
						entitySetExpression.parentExpressionSource = entitySetExpression.parentExpressionSource.source;
						filterPath[navigationPath] = true;
					} else if (i === path.length - 1 && current.ToMultiplicity === "*" && currentFilter != null) {
						filter = "it." + includePath + ".filter(" + currentFilter.filter.replace(/\(it\)/, "(" + includePath.replace(/\./g, "__") + ")").replace(/it\./g, includePath.replace(/\./g, "__") + ".") + ")";
						const selector = ($data as any).Container.createQueryExpressionCreator(query.context)
							.Visit(($data as any).Container.createIncludeExpression(entitySetExpression, ($data as any).Container.createCodeExpression(filter, currentFilter.filterScope))).selector;
						if (includeExpression.selector instanceof ($data as any).Expressions.ConstantExpression) {
							includeExpression.selector = selector;
							setSourceEntityType(path, includeExpression.selector.expression);
						} else if (includeExpression.selector instanceof ($data as any).Expressions.ParametricQueryExpression) {
							let lastFrameOperationExpression = includeExpression.selector.expression;
							while (lastFrameOperationExpression.source instanceof ($data as any).Expressions.FrameOperationExpression) {
								lastFrameOperationExpression = lastFrameOperationExpression.source;
							}
							lastFrameOperationExpression.source = selector.expression;
							setSourceEntityType(path, lastFrameOperationExpression.source);
						}
					}
					entitySet = context.getEntitySetFromElementType(current.ToType);
				}
			}
		});
	}

	public static getKeyProperty(storageKey: string): string {
		if (!!window.database[storageKey]
			&& !!window.database[storageKey].defaultType
			&& !!window.database[storageKey].defaultType.memberDefinitions
			&& window.database[storageKey].defaultType.memberDefinitions.getKeyProperties().length === 1) {
			return window.database[storageKey].defaultType.memberDefinitions.getKeyProperties()[0].name;
		} else {
			return "Id";
		}
	}

	public static addGlobalFilter(globalFilterFunction: any): void {
		window.Helper.Database.globalFilterFunctions.push(globalFilterFunction);
	}

	public static addIndex(storageKey: string, index: any[], prefix: string = "IX"): void {
		(window.Helper.Database.dbIndicesMultiEntry)[storageKey] = (window.Helper.Database.dbIndicesMultiEntry)[storageKey] || [];
		(window.Helper.Database.dbIndicesSingleEntry)[storageKey] = (window.Helper.Database.dbIndicesSingleEntry)[storageKey] || [];
		const indexNameMultiEntry = prefix + "_" + storageKey + "_" + index.join("_");
		if (window.Helper.Database.dbIndicesMultiEntry[storageKey].filter((x: any) => x.name === indexNameMultiEntry).length === 0) {
			(window.Helper.Database.dbIndicesMultiEntry)[storageKey].push({name: indexNameMultiEntry, keys: index});
		}
		index.forEach((indexColumn) => {
			const indexNameSingleEntry = prefix + "_" + storageKey + "_" + indexColumn;
			if (window.Helper.Database.dbIndicesSingleEntry[storageKey].filter((x: any) => x.name === indexNameSingleEntry).length === 0) {
				(window.Helper.Database.dbIndicesSingleEntry)[storageKey].push({
					name: indexNameSingleEntry,
					keys: [indexColumn]
				});
			}
		});
	}

	public static clearTrackedEntities(): void {
		if (window.database) {
			window.database.stateManager.trackedEntities.splice(0, window.database.stateManager.trackedEntities.length);
		}
	}

	public static getFromLocalStorage(keyName: string): string {
		keyName = window.Helper.Database.getStoragePrefix() + keyName;
		return window.localStorage.getItem(keyName);
	}

	public static getLocalStorageKeys(): string[] {
		const prefix = window.Helper.Database.getStoragePrefix();
		return Object.keys(window.localStorage)
			.filter(key => key.startsWith(prefix))
			.map(key => key.substring(prefix.length));
	}

	public static getStoragePrefix(): string {
		const usePrefix = window.localStorage.getItem("SchemaInfo") === null;
		let virtualPath = HelperUrl.resolveUrl("~");
		if (!usePrefix || !virtualPath) {
			return "";
		}
		virtualPath = virtualPath.startsWith("/") ? virtualPath.substring(1) : virtualPath;
		return virtualPath + "_";
	}

	public static hasProperty(storageKey: string, propertyName: string): boolean {
		const schema = window.Helper.Database.getSchema(storageKey);
		return schema && propertyName in schema;
	}

	public static removeFromLocalStorage(keyName: string): void {
		keyName = window.Helper.Database.getStoragePrefix() + keyName;
		window.localStorage.removeItem(keyName);
	}

	public static saveToLocalStorage(keyName: string, value: string): void {
		keyName = window.Helper.Database.getStoragePrefix() + keyName;
		window.localStorage.setItem(keyName, value);
	}

	public static registerTable(storageKey: string, columns: any, indices: any[] = []): void {
		const schema = mergeDeep(columns, window.Helper.Database.dbSchema[storageKey] || {});
		window.Helper.Database.dbSchema[storageKey] = {};
		Object.getOwnPropertyNames(schema)
			.sort()
			.forEach((column) => {
				window.Helper.Database.dbSchema[storageKey][column] = schema[column];
			});
		indices ||= [];
		indices.forEach((index) => {
			window.Helper.Database.addIndex(storageKey, index);
		});
	}

	public static registerConverter(targetTypeName: string, sourceTypeName: string, options: any = {}): void {
		options["sourceTypeName"] = sourceTypeName;
		window.Helper.Database.converters[targetTypeName] = options;
	}

	public static registerDependency(entity: any, dependency: any): void {
		entity = window.Helper.Database.getDatabaseEntity(entity);
		dependency = window.Helper.Database.getDatabaseEntity(dependency);
		if (dependency.entityState === $data.EntityState.Detached) {
			return;
		}
		entity.dependentOn = entity.dependentOn || [];
		if (entity.dependentOn.indexOf(dependency) === -1) {
			entity.dependentOn.push(dependency);
		}

		const index = (dependency.dependentOn || []).indexOf(entity);
		if (index !== -1) {
			dependency.dependentOn.splice(index, 1);
		}
	}

	public static initialize(): Promise<void> {
		if (HelperDatabase.initializePromise !== null){
			return HelperDatabase.initializePromise;
		}
		let initPromise: Promise<void> = Promise.resolve();
		if (($data as any).storageProviders.oData) {
			initPromise = window.AuthorizationManager.initPromise()
				.then(() => ($data as any).initService(window.Helper.Database.getODataStorageOptions().oDataServiceHost))
				.then((context) => {
					context.storageProvider.providerConfiguration.UpdateMethod = "PUT";
					context.storageProvider.providerConfiguration.sendAllPropertiesOnChange = true;
					context.prepareRequest = window.Helper.Database.prepareRequest;
					window.Helper.Database.configureStorageModel(context);
					window.oDataDatabase = context;
				});
		}
		initPromise = initPromise.then(async () => {
			const options = window.Helper.Database.getStorageOptions();
			if (options.provider === "oData") {
				namespace("Crm.Offline").Database = window.oDataDatabase;
				window.Helper.Database.dispatchDatabaseInitializedEvent();
				return null;
			}
			let typeDefinitions = window.Helper.Database.getTypeDefinitions();

			let aliases = {};

			// @ts-ignore
			for (const [typeName, typeDef] of Object.entries(typeDefinitions)) {
				// @ts-ignore
				if (typeDef.type === "enum") {
					// @ts-ignore
					$data.Enum.extend(typeName, typeDef.definition);
					// @ts-ignore
				} else if (typeDef.type === "entity") {
					// @ts-ignore
					HelperDatabase.replaceDateProperties(typeDefinitions, typeDef.definition);
					// @ts-ignore
					HelperDatabase.replaceEnumProperties(typeDefinitions, typeDef.definition);
					// @ts-ignore
					HelperDatabase.replaceSelfReferences(typeName, typeDef.definition, aliases);
					// @ts-ignore
					HelperDatabase.autoIndex(typeDefinitions, typeDef);
					// @ts-ignore
					$data.Entity.extend(typeName, typeDef.definition);
				}
			}

			for (const [aliasName, aliasTarget] of Object.entries(aliases)) {
				const sourceType = ($data as any).Container.getType(aliasTarget);
				if (sourceType) {
					const targetType = sourceType.extend(aliasName, {});
					($data as any).Container.registerConverter(targetType, sourceType, x => x, x => x);
				}
			}

			const beforeReadHandler = (items: any, query: { defaultType?: any; expression: any; context: any; }) => {
				//var sqlText = query.context.storageProvider._compile(query).sqlText;
				HelperDatabase.applyGlobalFilters(query);
				$(window.Helper.Offline).trigger("beforeRead", { table: query.defaultType.name, query: query });
			};
			const afterReadHandler = (items: any, entitySet: any, query: { defaultType: { name: any; }; }) => {
				//var sqlText = query.context.storageProvider._compile(query).sqlText;
				$(window.Helper.Offline).trigger("afterRead", { table: query.defaultType.name, query: query });
			};

			HelperDatabase.dbSchema = {};
			// @ts-ignore
			const schema: Record<string, $data.EntitSetDef> = {};
			// @ts-ignore
			if (typeDefinitions["Default.Container"]) {
				// @ts-ignore
				for (const [typeName, setDef] of Object.entries(typeDefinitions["Default.Container"].definition)) {
					// @ts-ignore
					if (setDef.type !== "$data.EntitySet") {
						continue;
					}
					const split = typeName.split("_");
					const name = split[split.length - 1];
					if (window.AuthorizationManager.hasPermission("Sync::" + name) === false && typeName !== "Main_NumberingSequence") {
						window.Log.debug("skipping entity set " + typeName);
						continue;
					}
					// @ts-ignore
					const def: $data.EntitSetDef = {
						afterRead: afterReadHandler,
						beforeRead: beforeReadHandler,
						// @ts-ignore
						elementType: setDef.elementType,
						// @ts-ignore
						type: setDef.type,
						indices: HelperDatabase.getIndices(typeName)
					}
					schema[typeName] = def;
					// @ts-ignore
					let stack = [typeDefinitions[setDef.elementType]];
					// @ts-ignore
					let typeDef: $data.TypeDef;
					while ((typeDef = stack.pop()) != null) {
						HelperDatabase.dbSchema[typeDef.typeNameShort] = typeDef.definition;
						if (typeDef.baseType !== "$data.Entity") {
							// @ts-ignore
							stack.push(typeDefinitions[typeDef.baseType]);
						}
					}
				}
			}

			// @ts-ignore
			window.Crm ||= {};
			// @ts-ignore
			window.Crm.Offline ||= {};
			window.Crm.Offline.LmobileDatabase = $data.EntityContext.extend("window.Crm.Offline.LmobileDatabase", schema);
			return HelperDatabase.tryInitDatabase();
		});

		initPromise = initPromise.then(() => {
			const saveChanges = window.database.saveChanges.bind(window.database);
			window.database.saveChanges = async(): Promise<number> => {
				try {
					return await saveChanges();
				} catch (e: any) {
					window.Log.error(e)
					if (e.data?.response?.statusCode === 412 || e.data?.response?.statusCode === "412") {
						throw new Error(window.Helper.String.getTranslatedString("HttpPreconditionFailedError"));
					}
					throw e;
				}
			};
			window.Helper.Database.EventListeners.attach();
		});
		HelperDatabase.initializePromise = initPromise;
		return initPromise;
	}

	public static getIndices(storageKey: string): any[] {
		const indices = window.Helper.Database.isMultiEntryIndexSupported() ? (window.Helper.Database.dbIndicesMultiEntry)[storageKey] : (window.Helper.Database.dbIndicesSingleEntry)[storageKey];
		return indices || [];
	}

	public static getSchema(storageKey?: string): any {
		return storageKey ? (window.Helper.Database.dbSchema)[storageKey] || null : window.Helper.Database.dbSchema;
	}

	public static async getTransactionIds(storageKey: string, element: any): Promise<string[]> {
		window.Helper.Database.transactionIdFunctions[storageKey] ||= [];
		return (await Promise.all(window.Helper.Database.transactionIdFunctions[storageKey].map(f => f(element)))).concat([element[window.Helper.Database.getKeyProperty(element)]]).flat();
	}

	public static getTypeDefinitions(): any {
		// @ts-ignore
		return $data.typeDefinitions;
	}

	public static setTransactionId<T>(storageKey: string, transactionIdFunction: (entity: T) => Promise<string | string[]>): void {
		(window.Helper.Database.transactionIdFunctions[storageKey] ??= []).push(transactionIdFunction);
	}

	public static async registerEventHandlers(viewModel: any, tableEventHandlers: any): Promise<void> {
		await window.Helper.Database.initialize();
		const eventHandlers = {};
		for (const table of Object.keys(tableEventHandlers)) {
			if (!window.database[table]) {
				window.Log.warn(`Failed registering event handlers for table ${table}`);
				continue;
			}
			eventHandlers[table] ??= {};
			const tableEventHandler = tableEventHandlers[table];
			for (const event of Object.keys(tableEventHandler)) {
				eventHandlers[table][event] ??= {};
				const handler = tableEventHandler[event];
				if (handler) {
					eventHandlers[table][event] = handler.bind(viewModel);
					if (window.database[table].defaultType) {
						window.database[table].defaultType.addEventListener(event, eventHandlers[table][event]);
					}
				}
			}
		}
		const baseDispose = viewModel.dispose;
		viewModel.dispose = function (): void {
			if (typeof baseDispose === "function") {
				// eslint-disable-next-line prefer-rest-params
				baseDispose.apply(viewModel, arguments);
			}
			for (const table of Object.keys(eventHandlers)) {
				for (const event of Object.keys(eventHandlers[table])) {
					const handler = eventHandlers[table][event];
					const defaultType = window.database[table].defaultType;
					if (handler && defaultType) {
						defaultType.removeEventListener(event, handler);
					}
				}
			}
		};
	}

	public static getDatabaseEntity<T = any>(obj): T {
		const value = ko.unwrap(obj);
		if (value instanceof ($data as any).KoObservableEntity) {
			return value.innerInstance;
		} else if (value === null || value === undefined || value instanceof $data.Entity) {
			return value;
		}
		throw new Error("unknown type");
	}

	public static transferData(keys: string[], src: { [x: string]: string | any[]; }, dst: { [x: string]: any; }): void {
		(keys || []).forEach(key => {
			if (!["ExtensionValues", "localTimestamp", "ItemStatus"].includes(key) && !(src[key] instanceof Array)) {
				dst[key] = src[key];
			} else if (src[key] instanceof Array) {
				dst[key] = src[key].slice();
			}
		});
	}

	public static createClone<T>(data: T): T {
		const entity = window.Helper.Database.getDatabaseEntity(data);
		if (entity === null) {
			return null;
		}
		const type = entity.getType();
		const clone = type.create(entity);
		window.Helper.Database.transferData(Object.keys(entity.initData), entity, clone);
		clone.ItemStatus = entity.ItemStatus;
		clone.resetChanges();
		if (entity.ExtensionValues && clone.ExtensionValues) {
			clone.ExtensionValues = this.createClone(entity.ExtensionValues);
		}
		return clone;
	}

	public static EventListeners = {
		"beforeCreate": {
			"BeforeCreateEventListener": (sender, items) => {
				items = Array.isArray(items) ? items : [items];
				items.forEach(function (item) {
					const type = item.getType();
					const user = window.Helper.User.getCurrentUserName();
					const date = new Date();
					if (type.getMemberDefinition("CreateDate")) {
						item.CreateDate = date;
					}
					if (type.getMemberDefinition("CreateUser")) {
						item.CreateUser = user;
					}
					if (type.getMemberDefinition("CreatedAt")) {
						item.CreatedAt = date;
					}
					if (type.getMemberDefinition("CreatedBy")) {
						item.CreatedBy = user;
					}
					if (type.getMemberDefinition("ModifiedAt")) {
						item.ModifiedAt = date;
					}
					if (type.getMemberDefinition("ModifiedBy")) {
						item.ModifiedBy = user;
					}
					if (type.getMemberDefinition("ModifyDate")) {
						item.ModifyDate = date;
					}
					if (type.getMemberDefinition("ModifyUser")) {
						item.ModifyUser = user;
					}
					if (type.getMemberDefinition("IsActive")) {
						item.IsActive = true;
					}
				});
			}
		},
		"afterCreate": {},
		"beforeUpdate": {},
		"afterUpdate": {},
		"beforeDelete": {},
		"afterDelete": {},
		attached: false,
		attach() {
			if (window.Helper.Database.EventListeners.attached) {
				return;
			}
			const storedEntityNames = Array.from(new Set((window.database as any)._storageModel.map(storageModel => storageModel.ItemName)));
			Object.keys(window.Helper.Database.EventListeners).forEach(event => {
				const eventObject = window.Helper.Database.EventListeners[event];
				if (eventObject instanceof Object) {
					Object.keys(eventObject).forEach(listener => {
						window.Helper.Database.EventListeners.attachListener(storedEntityNames, event, listener);
					});
				}
			});
			window.Helper.Database.EventListeners.attached = true;
		},
		attachListener(storedEntityNames, event, listener) {
			const obj = window.Helper.Database.EventListeners[event][listener];
			let listenerFunc;
			let condition = function (...args: any[]) {
				return true;
			};
			if (typeof obj === "function") {
				listenerFunc = obj;
			} else if (typeof obj.listener === "function") {
				listenerFunc = obj.listener;
				if (typeof obj.condition === "function") {
					condition = obj.condition;
				}
			} else {
				throw Error("unsupported listener format");
			}
			storedEntityNames.forEach(function (entityName) {
				if (condition(entityName)) {
					window.database[entityName].elementType.addEventListener(event, listenerFunc);
				}
			});
		}
	};

	public static hasPendingChanges(): boolean {
		if (!window.database) {
			return false;
		}
		let result = false;
		for (const entity of window.database.stateManager.trackedEntities) {
			if (entity.data._entityState === $data.EntityState.Added) {
				for (const changedProperty of (entity.data.changedProperties || [])) {
					if (entity.data[changedProperty.name]) {
						result = true;
					}
				}
			} else if (entity.data._entityState === $data.EntityState.Modified || entity.data._entityState === $data.EntityState.Deleted) {
				result = true;
			} else if (entity.data.ExtensionValues && Array.isArray(entity.data.ExtensionValues.changedProperties) && entity.data.ExtensionValues.changedProperties.length > 0) {
				result = true;
			}
		}
		return result;
	}

	static configureStorageModel(storageModel: any): void {
		const baseAdd = storageModel.add;
		storageModel.add = function (entity) {
			if (entity instanceof ($data as any).EntityWrapper) {
				entity = entity.getEntity();
			}
			const entitySet = this.getEntitySetFromElementType(entity.getType());
			const keyProperties = entitySet.elementType.memberDefinitions.getKeyProperties();
			const isGuidType = keyProperties.length === 1 && keyProperties[0].dataType === ($data as any).Guid;
			const idProperty = keyProperties[0].name;
			if (isGuidType && entity[idProperty] === "00000000-0000-0000-0000-000000000000") {
				entity[idProperty] = window.$data.createGuid().toString().toLowerCase();
			}
			// eslint-disable-next-line prefer-rest-params
			return baseAdd.apply(this, arguments);
		};
	}

	static initDatabase(): Promise<void> {
		namespace("window.Crm.Offline").Database = new window.Crm.Offline.LmobileDatabase(window.Helper.Database.getStorageOptions());
		window.Helper.Database.configureStorageModel(window.Crm.Offline.Database);
		return this.onReadyPromise();
	}

	static tryInitDatabase(): Promise<void> {
		if (!namespace("window.Crm.Offline").Database || window.Crm.Offline.Database._isOK || typeof window.Crm.Offline.Database.onReady !== "function") {
			window.Log.debug("tryInitDatabase: calling initDatabase directly");
			return window.Helper.Database.initDatabase();
		} else {
			return this.onReadyPromise();
		}
	}

	static onReadyPromise(): Promise<void> {
		return new Promise((resolve, reject) => {
			window.Crm.Offline.Database.onReady({
				success: () => {
					window.Helper.Database.dispatchDatabaseInitializedEvent();
					resolve();
				},
				error: e => {
					window.Crm.Offline.Database = null;
					window.Log.error("LmobileDatabase could not be initialized: " + (e.message || e));
					reject(e);
				}
			});
		});
	}

	static replaceArrayDefaultValues(definition: any): void {
		for (const x of Object.getOwnPropertyNames(definition)) {
			if (Array.isArray(definition[x].defaultValue) && definition[x].defaultValue.length === 0) {
				definition[x].defaultValue = () => [];
			}
		}
	}


	// @ts-ignore
	static replaceDateProperties(typeDefinitions: any, definition: Record<string, $data.MemberDef>): void {
		for (const [_, propertyDef] of Object.entries(definition || {})) {
			if (propertyDef.type === "Edm.DateTimeOffset") {
				propertyDef.type = "date";
			}
		}
	}

	// @ts-ignore
	static replaceEnumProperties(typeDefinitions: any, definition: Record<string, $data.MemberDef>): void {
		for (const [_, propertyDef] of Object.entries(definition || {})) {
			// @ts-ignore
			const typeDef = typeDefinitions[propertyDef.type];
			if (typeDef?.type === "enum") {
				propertyDef.type = "Edm.Int32";
			}
		}
	}

	// @ts-ignore
	static replaceSelfReferences(typeName: string, definition: Record<string, $data.MemberDef>, aliases: {}): void {
		for (const [propertyName, propertyDef] of Object.entries(definition || {})) {
			if (propertyDef.type === typeName) {
				propertyDef.originalType = typeName;
				propertyDef.type = propertyName;
			}
			if (propertyDef.originalType) {
				aliases[propertyName] = propertyDef.originalType;
			}
		}
	}

	// @ts-ignore
	static autoIndex(typeDefinitions: any, typeDef: $data.TypeDef): void {
		if (typeDef.typeName.endsWith("_ExtensionValues")) {
			return;
		}
		if (!typeDef.definition){
			return;
		}
		for (const [_, memberDef] of Object.entries(typeDef.definition)) {
			// @ts-ignore
			if (memberDef.type !== "Array" || (memberDef.keys?.length ?? 0) < 1) {
				continue;
			}
			// @ts-ignore
			const targetTypeName = typeDefinitions[memberDef.originalType ?? memberDef.elementType].typeNameShort
			// @ts-ignore
			HelperDatabase.addIndex(targetTypeName, memberDef.keys, "AUTO");
		}
	}

	static getODataStorageOptions(): oDataStorageOption {
		return {
			provider: "oData",
			oDataServiceHost: resolveUrl("~/api"),
			queryCache: false,
			UpdateMethod: "MERGE"
		};
	}

	static getStorageOptions(): StorageOptions {
		return window.Helper.Database.getODataStorageOptions();
	}

	static dispatchDatabaseInitializedEvent(): void {
		document.dispatchEvent(new Event("DatabaseInitialized"));
	}

	// indexedDb provider has a bug regarding multi-entry indices. plus IE currently doesn't support multi-entry indices at all
	static isMultiEntryIndexSupported = () => window.Helper.Database.getStorageOptions().provider !== "indexedDb";

}

function setupDatabase() {
	if (!window.hasOwnProperty("database")) {
		Object.defineProperty(window, "database", {
			get: () => namespace("window.Crm.Offline").Database || null
		});
	}
}

if (!window.Helper || !window.Helper.Database) {
	// @ts-ignore
	(window.Helper = window.Helper || {}).Database = HelperDatabase;
}
setupDatabase();
export {HelperDatabase};

