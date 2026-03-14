;
(function($data: any) {
	const getMember = $data.MemberDefinitionCollection.prototype.getMember;
	$data.MemberDefinitionCollection.prototype.getMember = function(name) {
		const ret = getMember.apply(this, arguments);
		if(ret === undefined && this["$Main_User"] !== undefined && window?.Main?.ViewModels?.HomeStartupViewModel?.initialized) {
			throw new Error(`${name} has no MemberDefinition. Hint: WebAPI::${name.split("_")[1]}`);
		}
		return ret;
	}

	var getEntitySetFromElementType = $data.EntityContext.prototype.getEntitySetFromElementType;
	$data.EntityContext.prototype.getEntitySetFromElementType = function(elementType) {
		try {
			return getEntitySetFromElementType.apply(this, arguments);
		} catch (e) {
			if (elementType.isAssignableTo && elementType.isAssignableTo($data.Entity)) {
				var result = this._entitySetReferences[elementType.name] = new $data.EntitySet(elementType, this, elementType.name);
				result.tableName = this._storageModel[elementType.name].TableName;
				return result;
			}
			throw e;
		} 
	}

	var getPublicMappedProperties = $data.MemberDefinitionCollection.prototype.getPublicMappedProperties;
	$data.MemberDefinitionCollection.prototype.getPublicMappedProperties = function() {
		var results = getPublicMappedProperties.apply(this, arguments);
		return results.sort(function(a, b) {
			return (a.name).localeCompare(b.name);
		});
	}

	if (!!$data.storageProviders.sqLitePro) {
		$data.defaults.sql.orderCaseInsensitive = true;
		// override sqlite bulkinsert implementation with a faster one taken from https://github.com/jaystack/jaydata/pull/178
		function getFieldObject(entitySet, physicalData) {
			var insertSqlString = 'INSERT INTO [' + entitySet.tableName + '](';
			var fieldList = '';
			var fieldValue = '';

			physicalData.constructor.memberDefinitions.getPublicMappedProperties().forEach(function (fieldDef, i) {
				var data = physicalData[fieldDef.name];
				var dataNullOrUndefined = data === null || data === undefined;
				if (fieldDef.key && !fieldDef.computed && dataNullOrUndefined) {
					// @ts-ignore
					$data.Guard.raise(new Exception('Key is not set', 'Value exception', data));
					return;
				}
				if (fieldDef.key && fieldDef.computed && dataNullOrUndefined) {
					var typeName = $data.Container.resolveName(fieldDef.type);
					if (typeof this.supportedAutoincrementKeys[typeName] === 'function') {
						physicalData[fieldDef.name] = this.supportedAutoincrementKeys[typeName]();
					}
				}

				if (fieldList.length > 0 && fieldList[fieldList.length - 1] != ',') {
					fieldList += ',';
					fieldValue += ',';
				}
				var fieldName = fieldDef.name;
				if (/*physicalData[fieldName] !== null && */physicalData[fieldName] !== undefined) {
					if (fieldDef.dataType && (!fieldDef.dataType.isAssignableTo || (fieldDef.dataType.isAssignableTo && !fieldDef.dataType.isAssignableTo($data.EntitySet)))) {
						fieldValue += '?';
						fieldList += '[' + fieldName + ']';
					}
				}
			}, this);

			var insertWithDefaultValuesSqlString = 'INSERT INTO [' + entitySet.tableName + '] Default values';

			if (fieldList[fieldList.length - 1] == ',') {
				fieldList = fieldList.slice(0, fieldList.length - 1);
			}
			if (fieldValue[fieldValue.length - 1] == ',') {
				fieldValue = fieldValue.slice(0, fieldValue.length - 1);
			}
			insertSqlString += fieldList + ') VALUES(' + fieldValue + ');';

			return {
				insertSqlString: insertSqlString,
				insertWithDefaultValuesSqlString: insertWithDefaultValuesSqlString
			};
		};

		function getFieldParam(entitySet, physicalData) {
			var fieldParam = [];
			physicalData.constructor.memberDefinitions.getPublicMappedProperties().forEach(function(fieldDef, i) {
				var fieldName = fieldDef.name;
				if (/*physicalData[fieldName] !== null && */physicalData[fieldName] !== undefined) {
					if (fieldDef.dataType && (!fieldDef.dataType.isAssignableTo || (fieldDef.dataType.isAssignableTo && !fieldDef.dataType.isAssignableTo($data.EntitySet)))) {
						var logicalFieldDef = physicalData.constructor.memberDefinitions.getMember(fieldDef.name);
						if (logicalFieldDef && logicalFieldDef.converter && logicalFieldDef.converter[this.providerName] && typeof logicalFieldDef.converter[this.providerName].toDb == 'function') {
							fieldParam.push(logicalFieldDef.converter[this.providerName].toDb(physicalData[fieldName], logicalFieldDef, this.context, logicalFieldDef.dataType));
						} else {
							fieldParam.push(this.fieldConverter.toDb[$data.Container.resolveName(fieldDef.dataType)](physicalData[fieldName]));
						}
					}
				}
			}, this);
			return fieldParam;
		};

		$data.storageProviders.sqLitePro.SqLiteStorageProvider.prototype.bulkInsert = function(entitySet, fields, datas, callback) {
			if (datas.length === 0) {
				if (callback.success) {
					callback.success();
				}
				return;
			}

			var data = datas[0];
			var dbType = entitySet.entityContext._storageModel.getStorageModel(data.getType()).PhysicalType;
			var physicalData = dbType.convertTo(data);

			var fieldObject = getFieldObject(entitySet, physicalData);

			var insertSqlString = fieldObject.insertSqlString;
			var insertWithDefaultValuesSqlString = fieldObject.insertWithDefaultValuesSqlString;

			var self = this;
			var cmds = [];
			var cmdParams = [];

			datas.forEach(function(entity) {
				var convertedPhysicalData = dbType.convertTo(entity);
				var fieldParam = getFieldParam.call(self, entitySet, convertedPhysicalData);
				if (fieldParam.length < 1) {
					cmds.push(insertWithDefaultValuesSqlString);
				} else {
					cmds.push(insertSqlString);
				}
				cmdParams.push(fieldParam);
			});

			var sqlConnection = this._createSqlConnection();
			var cmd = sqlConnection.createCommand(cmds, cmdParams);
			cmd.executeQuery({
				success: function(sqlResult) {
					if (callback.success) {
						callback.success(sqlResult);
					}
				},
				error: function(error) {
					if (callback.error) {
						callback.error(error);
					}
				}
			});
		};
	}

	if ($data.storageProviders.oData) {
		$data.defaults.openTypeDefaultValue = {};
		$data.defaults.openTypeDefaultPropertyName = "DynamicProperties";

		//remove jaydata validation for nullable and maxlength
		var origCreateProperty = $data.Metadata.prototype.createProperty;
		$data.Metadata.prototype.createProperty = function() {
			var property = origCreateProperty.apply(this, arguments);
			delete property.definition.required;
			delete property.definition.maxLength;
			return property;
		}
		//JayData handles bound functions incorrect as they allow normal query options on the EntitySet they are bound to
		var origCreateReturnTypeDefinition = $data.Metadata.prototype.createReturnTypeDefinition;
		$data.Metadata.prototype.createReturnTypeDefinition = function(propertySchema, definition) {
			origCreateReturnTypeDefinition.apply(this, arguments);
			if (propertySchema.parent
				&& propertySchema.parent.isBound === "true"
				&& propertySchema.parent.parameters
				&& propertySchema.parent.parameters.length
				&& propertySchema.parent.parameters[0].name === "bindingParameter") {
				var realType = propertySchema.parent.parameters[0].type;
				var match = /^Collection\((.*)\)$/.exec(realType);
				definition.operationReturnType = definition.elementType ?? definition.returnType; // save for later
				if (match) {
					definition.elementType = match[1];
				} else {
					definition.elementType = realType;
				}
			}
			return;
		};
		var origCreateQueryable = $data.Container.createQueryable;
		$data.Container.createQueryable = function(queryable, expression) {
			queryable = origCreateQueryable.apply(this, arguments);
			if (expression.expressionType === $data.Expressions.ServiceOperationExpression && expression.cfg.operationReturnType) {
				//now reset the type to make JayData produce the correct result type
				queryable.defaultType = $data.Container.resolveType(expression.cfg.operationReturnType);
			}
			return queryable;
		};

		//handle binaries (FileResource.Content) as string
		$data.Container.mapType($data.Container.getType("Edm.Binary"), $data.String);
		//handle decimals and int64 as number
		$data.Container.mapType($data.Container.getType("Edm.Decimal"), $data.Number);
		$data.Container.mapType($data.Container.getType("Edm.Int64"), $data.Number);

		//escape strings like &,*,%
		$data.oDataConverter.escape["$data.String"] = function (text) {
			return typeof text === 'string' ? "'" + encodeURIComponent(text.replace(/'/g, "''")) + "'" : text;
		}

		// removes property dependentOn which JayData uses to sort inserted entities for batches, resulting in foreign key constraint errors
		var origBuildIndependentBlocks = $data.storageProviders.oData.oDataProvider.prototype.buildIndependentBlocks;
		$data.storageProviders.oData.oDataProvider.prototype.buildIndependentBlocks = function (changedItems) {
			changedItems.forEach(function (x) {
				x.dependentOn = (x.data.dependentOn || []).filter(function (dependency) {
					return changedItems.find(function (changedItem) {
						return changedItem.data === dependency;
					});
				});
			});
			return origBuildIndependentBlocks.apply(this, arguments);
		};
		$data.storageProviders.oData.oDataProvider.prototype.addHeaders = function (request) {
			request.headers["Client-Id"] = $data.version;
			if (request.method === "PUT" && request.data) {
				const timestamp = request.data.ModifyDate ?? request.data.ModifiedAt;
				if (timestamp) {
					request.headers["If-Match"] = '"' + timestamp + '"';
				}
			}
		};
		$data.storageProviders.oData.oDataProvider.prototype.prepareRequest = function (requestData) {
			var request = requestData[0];
			var errorHandler = requestData[2];
			requestData[2] = function(error) {
				window.Log.error(JSON.stringify(error));
				if (errorHandler) {
					errorHandler.apply(this, arguments);
				}
			}
			var provider = this;
			if (request.method === "PUT" || request.method === "POST") {
				if (request.requestUri.indexOf("$batch") > 0) {
					request.data.__batchRequests.forEach(function (batchRequest) {
						(batchRequest.__changeRequests || []).forEach(function (changeRequest) {
							provider.addHeaders(changeRequest);
						});
					});
				} else {
					provider.addHeaders(request);
				}
			}
		};
		// the default recursive odatajs serialization will fail for jaydata entities
		var write = $data.odatajs.oData.json.jsonHandler.write; 
		$data.odatajs.oData.json.jsonHandler.write = function(request, context){
			request.data = JSON.parse(JSON.stringify(request.data));
			return write.apply(this, arguments);
		}

		//filter $metadata for types that the current user is allowed to access, so that the database context will only contain allowed entity sets
		var origInitStorageModelSync = $data.EntityContext.prototype._initStorageModelSync;
		$data.EntityContext.prototype._initStorageModelSync = function() {
			var container = this;
			if (container.storeToken.args.name && container.storeToken.args.name.toString() === "oData") {
				var definitions = container.getType().memberDefinitions;
				definitions.clearCache();
				Object.keys(definitions).forEach(function(key) {
					var definition = definitions[key];
					if (definition && definition.type === $data.EntitySet) {
						var name = definition.name.split("_");
						if (name.length > 1 && !window.AuthorizationManager.hasPermission("WebAPIRead::" + name[1])) {
							if (definition.elementType.inheritsFrom && definition.elementType.inheritsFrom.name === "DynamicFormElementRest") {
								return;
							}
							delete definitions[key];
						}
					}
				});
			}
			origInitStorageModelSync.apply(this, arguments);
		};

		//WebAPI Odata expects JSON-like formatted parameter, so GUIDs must be sent as string
		var origGenerateServiceOperation = $data.ServiceOperation.generateServiceOperation;
		$data.ServiceOperation.generateServiceOperation = function(cfg) {
			(cfg.params || []).forEach(function(p) {
				if (p.type === "Array" && p.elementType === "Edm.Guid") {
					p.elementType = "Edm.String";
				}
			});
			return origGenerateServiceOperation.apply(this, arguments);
		}
	}

	$data.Annotations.convertValue = function(annotation) {
		var type = annotation.annotationType;
		type = type[0].toLowerCase() + type.substring(1);
		var value = annotation[type];
		switch (type) {
			case "guid": return $data.parseGuid(value);
			case "bool": return value === "true" || value === "1";
			case "decimal":
			case "float": return parseFloat(value);
			case "int": return parseInt(value);
			default: return value;
		};
	};
	$data.Annotations.processedAnnotations["Lmobile.DefaultValue"] = function(annotationInfo, typeDef) {
		if (typeDef.definition && annotationInfo.property && typeDef.definition[annotationInfo.property]) {
			var propDef = typeDef.definition[annotationInfo.property];
			propDef.defaultValue = $data.Annotations.convertValue(annotationInfo.annotation);
		}
	}
	$data.Annotations.processedAnnotations["Keys:Lmobile.NavigationProperty"] = function(annotationInfo, typeDef) {
		if (typeDef.definition && annotationInfo.property && typeDef.definition[annotationInfo.property] && annotationInfo.annotation.string) {
			const propDef = typeDef.definition[annotationInfo.property];
			propDef.keys = annotationInfo.annotation.string.split(',');
		}
	}
	$data.Annotations.processedAnnotations["InverseProperty:Lmobile.NavigationProperty"] = function(annotationInfo, typeDef) {
		if (typeDef.definition && annotationInfo.property && typeDef.definition[annotationInfo.property] && annotationInfo.annotation.string) {
			const propDef = typeDef.definition[annotationInfo.property];
			propDef.inverseProperty = annotationInfo.annotation.string;
		}
	}
	const include = {
		"baseType": true,
		"namespace": true,
		"definition": true,
		"type": true,
		"typeName": true,
	}
	$data.typeDefinitions = {};
	const origPreProcessAnnotation = $data.Annotations.prototype.preProcessAnnotation;
	$data.Annotations.prototype.preProcessAnnotation = function(typeDef) {
		const result = origPreProcessAnnotation.apply(this, arguments);

		for (const [name, def] of Object.entries(typeDef.definition)) {
			// @ts-ignore
			if ("defaultValue" in def) {
				continue;
			}
			let defaultValue = undefined;
			// @ts-ignore
			switch (def.type) {
				case "Edm.Boolean":
					if (name === "IsActive") {
						defaultValue = true;
						// @ts-ignore
					} else if (def.nullable === false) {
						defaultValue = false;
					} else {
						defaultValue = null;
					}
					break;
				case "Edm.Int16":
				case "Edm.Int32":
				case "Edm.Int64":
				case "Edm.Decimal":
				case "Edm.Single":
				case "Edm.Double":
					// @ts-ignore
					defaultValue = def.nullable === false ? 0 : null;
					break;
				case "Edm.Guid":
					// @ts-ignore
					defaultValue = def.nullable === false ? "00000000-0000-0000-0000-000000000000" : null;
					break;
				case "Edm.Binary":
					defaultValue = null;
					break;
				case "Edm.DateTimeOffset":
					defaultValue = null;
					break;
				case "Edm.Duration":
					defaultValue = null;
					break;
				case "Edm.String":
					defaultValue = null;
					break;
				case "Array":
					defaultValue = "[]";
					break;
				default:
					if (name === "ExtensionValues") {
						defaultValue = {};
						// @ts-ignore
					} else if (def.keys && def.type){
						defaultValue = null;
					}
					break;
			}
			if (defaultValue !== undefined) {
				// @ts-ignore
				def.defaultValue = defaultValue;
			}
		}

		let clone = Object.entries(typeDef).reduce((map, [key, value]) => {
			if (key in include) {
				map[key] = value;
			}
			return map;
		}, {});
		clone = JSON.parse(JSON.stringify(clone));
		// @ts-ignore
		clone.typeNameShort = clone.typeName.substring(clone.namespace.length + 1);
		// @ts-ignore
		$data.typeDefinitions[clone.typeName] = clone;
		return result;
	};

	var baseEntityContextRemove = $data.EntityContext.prototype.remove;
	$data.EntityContext.prototype.remove = function(entity) {
		if (entity.context && entity.context.storageProvider.name === "oData" && entity.entityState === $data.EntityState.Added) {
			return $data.EntityContext.prototype.detach.apply(this, arguments);
		} else {
			return baseEntityContextRemove.apply(this, arguments);
		}
	};

	var baseEntitySetRemove = $data.EntitySet.prototype.remove;
	$data.EntitySet.prototype.remove = function(entity) {
		if (entity.context && entity.context.storageProvider.name === "oData" && entity.entityState === $data.EntityState.Added) {
			return $data.EntitySet.prototype.detach.apply(this, arguments);
		} else {
			return baseEntitySetRemove.apply(this, arguments);
		}
	};
})(window.$data);