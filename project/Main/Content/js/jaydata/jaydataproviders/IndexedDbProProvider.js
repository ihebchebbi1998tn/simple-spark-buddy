// JayData 1.5.12 Pro - Commercial License
// Copyright JayStack Technologies (http://jaydata.org/licensing)
// Support: http://jaystack.com
(function(f){if(typeof exports==="object"&&typeof module!=="undefined"){module.exports=f()}else if(typeof define==="function"&&define.amd){define("jaydata/indexeddb-pro",["jaydata/core"],f)}else{var g;if(typeof window!=="undefined"){g=window}else if(typeof global!=="undefined"){g=global}else if(typeof self!=="undefined"){g=self}else{g=this}g.$data = f()}})(function(){var define,module,exports;return (function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(_dereq_,module,exports){
'use strict';

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

(0, _core.$C)('$data.storageProviders.IndexedDBPro.IndexedDBCompiler', _core2.default.Expressions.EntityExpressionVisitor, null, {
    constructor: function constructor(provider) {
        this.db = provider.db;
        this.provider = provider;
    },
    compile: function compile(query, callback) {
        this.defaultType = query.defaultType;
        var start = new Date().getTime();
        this.subqueryIndex = 0;
        var ctx = {};
        var newExpression = this.Visit(query.expression, ctx);

        //search new indexes
        var newIndexContext = { db: this.db, provider: this.provider };
        var objectStoreMonitor = _core2.default.storageProviders.IndexedDBPro.FindAllObjectStores.create();
        objectStoreMonitor.Visit(newExpression, newIndexContext);
        var indexMonitor = _core2.default.storageProviders.IndexedDBPro.PhysicalIndexSearch.create();
        newExpression = indexMonitor.Visit(newExpression, newIndexContext);

        //createIndexes
        this._createNewIndexes(newIndexContext, {
            success: function success() {
                _core2.default.Trace.log("Compiler in milliseconds: ", new Date().getTime() - start);
                callback.success({ context: query.context, defaultType: query.defaultType, expression: newExpression });
            }
        });
    },
    _createNewIndexes: function _createNewIndexes(ctx, callback) {
        //TODO: make safety
        /*if (false && ctx.newIndexes && ctx.newIndexes.length > 0) {
            var self = this;
            this.provider.db.close();
            var newVersion = this.provider.db.version || 0;
            //close db and reopen it with incrased version number
            this.provider.indexedDB.open(this.provider.providerConfiguration.databaseName, ++newVersion).setCallbacks({
                onupgradeneeded: function (event) {
                    var writeTran = event.target.transaction;
                    for (var i = 0; i < ctx.newIndexes.length; i++) {
                        var expression = ctx.newIndexes[i];
                        var writeObjectStore = writeTran.objectStore(expression.newIndex.field.objectStoreName);
                        if (!writeObjectStore.indexNames.contains(expression.newIndex.field.name)) {
                            writeObjectStore.createIndex(expression.newIndex.field.name, expression.newIndex.field.name, { unique: false });
                        }
                        expression.suggestedIndex = expression.newIndex;
                        expression.newIndex = null;
                    }
                    writeTran.db.close();
                    self.provider.indexedDB.open(self.provider.providerConfiguration.databaseName).onsuccess = function (e) {
                        self.provider.db = e.target.result;
                        callback.success();
                    }
                }
            });
        } else {*/
        callback.success();
        //}
    },
    _buildNavigationTree: function _buildNavigationTree(expression, context) {
        return expression;
        /*var self = this;
        var m = Container.createExpressionMonitor({
            MutateEntityExpression: function (exp, ctx) {
                if (exp.entityType == self.defaultType) {
                    return exp;
                }
                return { TODO: true };
            },
            MutateEntitySetExpression: function (exp, ctx) {
                if (!exp.source instanceof $data.EntityContext) {
                    return Container.createSimpleBinaryExpression();
                }
                return exp;
            },
            VisitEntityContextExpression: function (exp, ctx) {
                ctx.___entityContext = exp.instance;
            }
        });
        return m.Visit(expression, context);*/
    },
    VisitFilterExpression: function VisitFilterExpression(expression, context) {
        var newSelector = this.Visit(expression.selector, context);
        return _core.Container.createFilterExpression(expression.source, newSelector);
    },

    VisitParametricQueryExpression: function VisitParametricQueryExpression(expression, context) {
        var tempParentNodeType = context.parentNodeType;
        context.parentNodeType = null;
        var newExp = this.Visit(expression.expression, context);
        context.parentNodeType = tempParentNodeType;

        return _core.Container.createParametricQueryExpression(newExp, expression.parameters);
    },
    VisitSimpleBinaryExpression: function VisitSimpleBinaryExpression(expression, context) {
        var origType = context.parentNodeType;

        context.parentNodeType = expression.nodeType;
        var nLeft = this.Visit(expression.left, context);
        var nRight = this.Visit(expression.right, context);
        context.parentNodeType = origType;
        /* if (context.navProp) {
             var newExp = this._buildNavigationTree(expression, context);
             context.navProp = undefined;
             return newExp;
         } else if (nLeft instanceof $data.Expressions.EntityExpression || nRight instanceof $data.Expressions.EntityExpression) {
             return $data.storageProviders.IndexedDBPro.IndexedDBLogicalInFilterExpression.create(nLeft, nRight);
         } else {*/
        switch (expression.nodeType) {
            case "and":
                if (nLeft instanceof Array && nRight instanceof Array) {
                    var filters = nLeft.concat(nRight);
                    if (context.parentNodeType == "and") {
                        return filters;
                    }
                    return _core2.default.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression.create(filters);
                } else {
                    if (nLeft instanceof Array) {
                        nLeft = _core2.default.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression.create(nLeft);
                    }
                    if (nRight instanceof Array) {
                        nRight = _core2.default.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression.create(nRight);
                    }
                    return _core2.default.storageProviders.IndexedDBPro.IndexedDBLogicalAndFilterExpression.create(nLeft, nRight);
                }
                break;
            case "or":
                if (nLeft instanceof Array) {
                    nLeft = _core2.default.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression.create(nLeft);
                }
                if (nRight instanceof Array) {
                    nRight = _core2.default.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression.create(nRight);
                }

                return _core2.default.storageProviders.IndexedDBPro.IndexedDBLogicalOrFilterExpression.create(nLeft, nRight);
                break;
            case "in":
                if (nLeft instanceof _core2.default.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression || nRight instanceof _core2.default.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression) {
                    return _core2.default.storageProviders.IndexedDBPro.IndexedDBLogicalInFilterExpression.create(nLeft, nRight);
                }
                if (context.parentNodeType !== 'and') {
                    return _core2.default.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression.create([expression]);
                }
                return [expression];
                break;
            default:
                if (context.parentNodeType !== 'and') {
                    return _core2.default.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression.create([expression]);
                }
                return [expression];
                break;
        }
        //}
    },
    VisitEntityExpression: function VisitEntityExpression(expression, context) {
        if (expression.entityType !== this.defaultType) {
            context.navProp = true;
        }
        return expression;
    }
}, {});
(0, _core.$C)('$data.storageProviders.IndexedDBPro.FindAllObjectStores', _core2.default.Expressions.EntityExpressionVisitor, null, {
    VisitEntitySetExpression: function VisitEntitySetExpression(expression, context) {
        var tName = expression.storageModel.TableName;
        context.objectStoresName = context.objectStoresName || [];
        if (!context.objectStoresName.some(function (it) {
            return it == tName;
        })) {
            context.objectStoresName.push(tName);
        }
    },
    VisitIndexedDBPhysicalAndFilterExpression: function VisitIndexedDBPhysicalAndFilterExpression(expression, context) {
        var i = 0;
        var filter = expression.filters[i];
        while (filter) {
            this.Visit(filter.left, context);
            this.Visit(filter.right, context);
            filter = expression.filters[++i];
        }
    },
    VisitIndexedDBLogicalAndFilterExpression: function VisitIndexedDBLogicalAndFilterExpression(expression, context) {
        this.Visit(expression.left, context);
        this.Visit(expression.right, context);
    },
    VisitIndexedDBLogicalOrFilterExpression: function VisitIndexedDBLogicalOrFilterExpression(expression, context) {
        this.Visit(expression.left, context);
        this.Visit(expression.right, context);
    },
    VisitIndexedDBLogicalInFilterExpression: function VisitIndexedDBLogicalInFilterExpression(expression, context) {
        this.Visit(expression.left, context);
        this.Visit(expression.right, context);
    }
});
(0, _core.$C)('$data.storageProviders.IndexedDBPro.PhysicalIndexSearch', _core2.default.Expressions.EntityExpressionVisitor, null, {
    //VisitEntitySetExpression: function (expression, context) {
    //    var tName = expression.storageModel.TableName;
    //    context.objectStoresName = context.objectStoresName || [];
    //    if (!context.objectStoresName.some(function (it) { return it == tName; })) {
    //        context.objectStoresName.push(tName);
    //    }
    //},
    VisitParametricQueryExpression: function VisitParametricQueryExpression(expression, context) {
        context.tran = context.db.transaction(context.objectStoresName, context.provider.IDBTransactionType.READ_ONLY);
        this.Visit(expression.expression, context);
    },
    VisitIndexedDBPhysicalAndFilterExpression: function VisitIndexedDBPhysicalAndFilterExpression(expression, context) {
        var suggestedIndex = { coverdField: 0, expression: [] };
        var operators = ['equal', 'equalTyped', 'greaterThan', 'greaterThanOrEqual', 'lessThan', 'lessThenOrEqual'];
        var newIndex = null;
        var objectStoreName = '';
        context.objectStores = context.objectStores || {};
        context.objectStoreIndices = context.objectStoreIndices || {};

        for (var i = 0; i < expression.filters.length; i++) {
            var filter = expression.filters[i];
            var filterValue = { value: null };
            this.Visit(filter.left, filterValue);
            this.Visit(filter.right, filterValue);

            filterValue.nodeType = filter.nodeType;
            filterValue.index = i;
            filter.filterValue = filterValue;
            objectStoreName = filterValue.field.objectStoreName;
        }

        //get objectstore indices
        var indices = context.objectStoreIndices[objectStoreName];
        if (!indices) {
            var objectStore = context.objectStores[objectStoreName];
            if (!objectStore) {
                //get objectStore from transact
                objectStore = context.tran.objectStore(objectStoreName);
                context.objectStores[objectStoreName] = objectStore;
            }
            //populate indices
            indices = [];
            for (var i = 0; i < objectStore.indexNames.length; i++) {
                var index = objectStore.index(objectStore.indexNames[i]);
                indices.push({ indexName: objectStore.indexNames[i], keyPath: index.keyPath });
            };
            context.objectStoreIndices[objectStoreName] = indices;
        }
        for (var i = 0; i < indices.length; i++) {
            var index = indices[i];
            for (var o = 0; o < operators.length; o++) {
                var coverIndex = 0;
                var op = operators[o];
                var filterIndexSum = 0;
                var filterIndexArray = [];

                var filteredExpression = expression.filters.filter(function (exp) {
                    return exp.nodeType == op;
                });
                if (suggestedIndex.coverdField <= filteredExpression.length) {
                    var keyPath = index.keyPath;
                    if (typeof keyPath === 'string') {
                        keyPath = [keyPath];
                    }

                    var indexedExpression = filteredExpression.filter(function (exp) {
                        return exp.left.nodeType === "EntityField" && keyPath.indexOf(exp.filterValue.field.name) >= 0;
                    });
                    if (indexedExpression.length == keyPath.length) {
                        indexedExpression.forEach(function (exp) {
                            coverIndex++;
                            filterIndexSum += exp.filterValue.index;
                            filterIndexArray.push(exp.filterValue.index);
                        });
                    }

                    if (coverIndex > 0 && (suggestedIndex.coverdField < coverIndex || suggestedIndex.coverdField === coverIndex && suggestedIndex.filterIndexSum > filterIndexSum)) {
                        suggestedIndex.coverdField = coverIndex;
                        suggestedIndex.nodeType = op;
                        suggestedIndex.name = index.indexName;
                        suggestedIndex.filterIndexSum = filterIndexSum;
                        suggestedIndex.filterIndexArray = filterIndexArray;
                        suggestedIndex.hasMultipleValue = typeof index.keyPath !== "string";
                    }
                }
            }
        }

        if (suggestedIndex.coverdField > 0) {
            suggestedIndex.expression = expression.filters.filter(function (exp) {
                return suggestedIndex.filterIndexArray.indexOf(exp.filterValue.index) >= 0;
            });
            suggestedIndex.value = [];
            var newFilterList = expression.filters.filter(function (exp) {
                return suggestedIndex.filterIndexArray.indexOf(exp.filterValue.index) === -1;
            });

            var i = 0;
            while (i < suggestedIndex.expression.length) {
                var exp = suggestedIndex.expression[i];
                if (suggestedIndex.hasMultipleValue) {
                    suggestedIndex.value.push(exp.filterValue.value);
                } else {
                    suggestedIndex.value = exp.filterValue.value;
                }
                i = i + 1;
            }
            expression.suggestedIndex = suggestedIndex;
            expression.filters = newFilterList;
        }
        /*
        var i = 0;
        var filter = expression.filters[i];
        while (filter && !suggestedIndex) {
            var filterValue = { value: null };
            this.Visit(filter.left, filterValue);
            this.Visit(filter.right, filterValue);
             filterValue.nodeType = filter.nodeType;
            filterValue.index = i;
            filter.filterValue = filterValue;
             if (filterValue.field) {
                //get objectStore from transact
                var objectStore = context.objectStores[filterValue.field.objectStoreName];
                if (!objectStore) {
                    objectStore = context.tran.objectStore(filterValue.field.objectStoreName);
                    context.objectStores[filterValue.field.objectStoreName] = objectStore;
                }
                //check existing filter
                if (objectStore.indexNames.contains(filterValue.field.name)) {
                    suggestedIndex = filterValue;
                    newIndex = null;
                } else if (!newIndex && filterValue.value !== null) {
                    newIndex = filterValue;
                }
            }
            filter = expression.filters[++i];
        }
        expression.suggestedIndex = suggestedIndex;
        expression.newIndex = newIndex;
        if (newIndex && !suggestedIndex) {
            context.newIndexes = context.newIndexes || [];
            context.newIndexes.push(expression);
        }*/
    },
    VisitEntityFieldExpression: function VisitEntityFieldExpression(expression, context) {
        context.field = context.field || { name: expression.selector.memberName, objectStoreName: expression.source.storageModel.TableName };
    },
    VisitConstantExpression: function VisitConstantExpression(expression, context) {
        if (context) {
            context.value = context.value || expression.value;
        }
    },
    VisitIndexedDBLogicalAndFilterExpression: function VisitIndexedDBLogicalAndFilterExpression(expression, context) {
        this.Visit(expression.left, context);
        this.Visit(expression.right, context);
    },
    VisitIndexedDBLogicalOrFilterExpression: function VisitIndexedDBLogicalOrFilterExpression(expression, context) {
        this.Visit(expression.left, context);
        this.Visit(expression.right, context);
    },
    VisitIndexedDBLogicalInFilterExpression: function VisitIndexedDBLogicalInFilterExpression(expression, context) {
        this.Visit(expression.left, context);
        this.Visit(expression.right, context);
    }
});

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}],2:[function(_dereq_,module,exports){
'use strict';

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

_core2.default.IndexedDBProConverter = {
    fromDb: {
        '$data.Enum': function $dataEnum(v, enumType) {
            return _core2.default.Container.convertTo(v, enumType);
        },
        '$data.Byte': _core2.default.Container.proxyConverter,
        '$data.SByte': _core2.default.Container.proxyConverter,
        '$data.Decimal': _core2.default.Container.proxyConverter,
        '$data.Float': _core2.default.Container.proxyConverter,
        '$data.Int16': _core2.default.Container.proxyConverter,
        '$data.Int64': _core2.default.Container.proxyConverter,
        '$data.Integer': _core2.default.Container.proxyConverter,
        '$data.Int32': _core2.default.Container.proxyConverter,
        '$data.Number': _core2.default.Container.proxyConverter,
        '$data.Date': _core2.default.Container.proxyConverter,
        '$data.DateTimeOffset': _core2.default.Container.proxyConverter,
        '$data.Duration': _core2.default.Container.proxyConverter,
        '$data.Day': _core2.default.Container.proxyConverter,
        '$data.Time': _core2.default.Container.proxyConverter,
        '$data.String': _core2.default.Container.proxyConverter,
        '$data.Boolean': _core2.default.Container.proxyConverter,
        '$data.Blob': function $dataBlob(b) {
            return b ? _core2.default.Container.convertTo(b, _core2.default.Blob) : b;
        },
        '$data.Array': function $dataArray(arr) {
            if (arr === undefined) {
                return new _core2.default.Array();
            }return arr;
        },
        '$data.Object': _core2.default.Container.proxyConverter,
        "$data.Guid": function $dataGuid(g) {
            return g ? _core2.default.parseGuid(g).toString() : g;
        },
        '$data.GeographyPoint': function $dataGeographyPoint(g) {
            if (g) {
                return new _core2.default.GeographyPoint(g);
            }return g;
        },
        '$data.GeographyLineString': function $dataGeographyLineString(g) {
            if (g) {
                return new _core2.default.GeographyLineString(g);
            }return g;
        },
        '$data.GeographyPolygon': function $dataGeographyPolygon(g) {
            if (g) {
                return new _core2.default.GeographyPolygon(g);
            }return g;
        },
        '$data.GeographyMultiPoint': function $dataGeographyMultiPoint(g) {
            if (g) {
                return new _core2.default.GeographyMultiPoint(g);
            }return g;
        },
        '$data.GeographyMultiLineString': function $dataGeographyMultiLineString(g) {
            if (g) {
                return new _core2.default.GeographyMultiLineString(g);
            }return g;
        },
        '$data.GeographyMultiPolygon': function $dataGeographyMultiPolygon(g) {
            if (g) {
                return new _core2.default.GeographyMultiPolygon(g);
            }return g;
        },
        '$data.GeographyCollection': function $dataGeographyCollection(g) {
            if (g) {
                return new _core2.default.GeographyCollection(g);
            }return g;
        },
        '$data.GeometryPoint': function $dataGeometryPoint(g) {
            if (g) {
                return new _core2.default.GeometryPoint(g);
            }return g;
        },
        '$data.GeometryLineString': function $dataGeometryLineString(g) {
            if (g) {
                return new _core2.default.GeometryLineString(g);
            }return g;
        },
        '$data.GeometryPolygon': function $dataGeometryPolygon(g) {
            if (g) {
                return new _core2.default.GeometryPolygon(g);
            }return g;
        },
        '$data.GeometryMultiPoint': function $dataGeometryMultiPoint(g) {
            if (g) {
                return new _core2.default.GeometryMultiPoint(g);
            }return g;
        },
        '$data.GeometryMultiLineString': function $dataGeometryMultiLineString(g) {
            if (g) {
                return new _core2.default.GeometryMultiLineString(g);
            }return g;
        },
        '$data.GeometryMultiPolygon': function $dataGeometryMultiPolygon(g) {
            if (g) {
                return new _core2.default.GeometryMultiPolygon(g);
            }return g;
        },
        '$data.GeometryCollection': function $dataGeometryCollection(g) {
            if (g) {
                return new _core2.default.GeometryCollection(g);
            }return g;
        }
    },
    toDb: {
        '$data.Enum': _core2.default.Container.proxyConverter,
        '$data.Byte': _core2.default.Container.proxyConverter,
        '$data.SByte': _core2.default.Container.proxyConverter,
        '$data.Decimal': _core2.default.Container.proxyConverter,
        '$data.Float': _core2.default.Container.proxyConverter,
        '$data.Int16': _core2.default.Container.proxyConverter,
        '$data.Int64': _core2.default.Container.proxyConverter,
        '$data.Integer': _core2.default.Container.proxyConverter,
        '$data.Int32': _core2.default.Container.proxyConverter,
        '$data.Number': _core2.default.Container.proxyConverter,
        '$data.Date': _core2.default.Container.proxyConverter,
        '$data.DateTimeOffset': _core2.default.Container.proxyConverter,
        '$data.Duration': _core2.default.Container.proxyConverter,
        '$data.Day': _core2.default.Container.proxyConverter,
        '$data.Time': _core2.default.Container.proxyConverter,
        '$data.String': _core2.default.Container.proxyConverter,
        '$data.Boolean': _core2.default.Container.proxyConverter,
        '$data.Blob': function $dataBlob(b) {
            return b ? _core2.default.Blob.toString(b) : b;
        },
        '$data.Array': function $dataArray(arr) {
            return arr ? JSON.parse(JSON.stringify(arr)) : arr;
        },
        '$data.Object': _core2.default.Container.proxyConverter,
        "$data.Guid": function $dataGuid(g) {
            return g ? g.toString() : g;
        },
        '$data.GeographyPoint': function $dataGeographyPoint(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeographyLineString': function $dataGeographyLineString(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeographyPolygon': function $dataGeographyPolygon(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeographyMultiPoint': function $dataGeographyMultiPoint(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeographyMultiLineString': function $dataGeographyMultiLineString(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeographyMultiPolygon': function $dataGeographyMultiPolygon(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeographyCollection': function $dataGeographyCollection(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeometryPoint': function $dataGeometryPoint(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeometryLineString': function $dataGeometryLineString(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeometryPolygon': function $dataGeometryPolygon(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeometryMultiPoint': function $dataGeometryMultiPoint(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeometryMultiLineString': function $dataGeometryMultiLineString(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeometryMultiPolygon': function $dataGeometryMultiPolygon(g) {
            if (g) {
                return g;
            }return g;
        },
        '$data.GeometryCollection': function $dataGeometryCollection(g) {
            if (g) {
                return g;
            }return g;
        }
    }
};

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}],3:[function(_dereq_,module,exports){
'use strict';

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

(0, _core.$C)('$data.storageProviders.IndexedDBPro.IndexedDBExpressionExecutor', _core2.default.Expressions.EntityExpressionVisitor, null, {
    constructor: function constructor(provider, tran) {
        this.provider = provider;
        this.tran = tran.transaction;
    },
    runQuery: function runQuery(query, callback) {
        var start = new Date().getTime();
        this.entitySet = query.context.getEntitySetFromElementType(query.defaultType);
        //this.tran = query.transaction;// this.provider.db.transaction([this.entitySet.tableName], this.provider.IDBTransactionType.READ_ONLY);
        this.objectStore = this.tran.objectStore(this.entitySet.tableName);

        var ctx = {
            objectStore: this.objectStore,
            promises: [],
            callback: {}
        };
        ctx.callback.success = function (result) {
            if (ctx.skip !== 0) {
                if (result.skip && result.objects) {
                    result.objects = result.objects.slice(result.skip);
                }
            }

            if (ctx.take !== 0) {
                if (result.take && result.objects) {
                    result.objects = result.objects.slice(0, result.take);
                }
            }
            ctx.result = result.objects || result;
            _core2.default.Trace.log("Executor in milliseconds : ", new Date().getTime() - start);
            callback.success(ctx.result);
        };
        this.Visit(query.expression, ctx);
    },
    VisitToArrayExpression: function VisitToArrayExpression(expression, context) {
        this.Visit(expression.source, context);
    },
    VisitFilterExpression: function VisitFilterExpression(expression, context) {
        this.Visit(expression.selector, context);
        if (expression.selector && expression.selector.expression && expression.selector.expression.expressionType == _core2.default.Expressions.ConstantExpression) {
            if (context.value) this.Visit(expression.source, context);else context.callback.success({ ids: [], objects: [] });
        }
    },
    VisitParametricQueryExpression: function VisitParametricQueryExpression(expression, context) {
        var tmpCallback = context.callback;
        context.callback = {
            success: function success(eResult) {
                //context.result = eResult || expression.expression.resultSet;
                //context.result = context.result ? context.result.objects : [];
                tmpCallback.success(eResult);
            }
        };
        this.Visit(expression.expression, context);
    },
    VisitProjectionExpression: function VisitProjectionExpression(expression, context) {
        this.Visit(expression.source, context);
    },
    VisitOrderExpression: function VisitOrderExpression(expression, context) {
        if (!context.hasOrder) {
            context.hasOrder = true;
            context.orderExpressions = [];
            var tmpCallback = context.callback;
            var self = this;
            context.callback = {
                success: function success(result) {
                    if (result.applyOrder) {
                        tmpCallback.success(result);
                    } else {
                        var f = function f(a, b) {
                            var r;
                            for (var i = context.orderExpressions.length - 1; i >= 0; i--) {
                                r = 0;
                                var expr = context.orderExpressions[i];

                                var ctx = { instance: a };
                                self.Visit(expr.selector.expression, ctx);
                                var aValue = ctx.value;

                                ctx = { instance: b };
                                self.Visit(expr.selector.expression, ctx);
                                var bValue = ctx.value;

                                if (expr.nodeType === "OrderBy") {
                                    r = aValue === bValue ? 0 : aValue > bValue || bValue === null ? 1 : -1;
                                } else {
                                    r = aValue === bValue ? 0 : aValue < bValue || aValue === null ? 1 : -1;
                                }

                                if (r !== 0) break;
                            }
                            return r;
                        };

                        var resultSet = { ids: [], objects: [] };
                        resultSet.objects = result.objects.sort(f);
                        tmpCallback.success(resultSet);
                    }
                }
            };
        }
        context.orderExpressions.push(expression);
        this.Visit(expression.source, context);
    },
    VisitCountExpression: function VisitCountExpression(expression, context) {
        var tmpCallback = context.callback;
        context.callback = {
            success: function success(result) {
                tmpCallback.success({ cnt: result.objects.length });
            }
        };
        this.Visit(expression.source, context);
    },
    VisitPagingExpression: function VisitPagingExpression(expression, context) {
        var pagingValue = {};
        this.Visit(expression.amount, pagingValue);
        switch (expression.nodeType) {
            case "Skip":
                context.skip = pagingValue.value;break;
            case "Take":
                context.take = pagingValue.value;break;
        }
        var tmp = context.callback;
        var self = this;
        context.callback = {
            success: function success(result) {
                var v = {};
                self.Visit(expression.amount, v);
                var resultSet = { ids: [], objects: [] };
                switch (expression.nodeType) {
                    case "Skip":
                        result.skip = v.value;break; //resultSet.ids = result.ids.slice(v.value); resultSet.objects = result.objects.slice(v.value); break;
                    case "Take":
                        result.take = v.value;break; //resultSet.ids = result.ids.slice(0, v.value); resultSet.objects = result.objects.slice(0, v.value); break;
                }
                tmp.success(result);
            }
        };
        this.Visit(expression.source, context);
    },

    VisitEntitySetExpression: function VisitEntitySetExpression(expression, context) {
        var resultSet = { ids: [], objects: [], applyPaging: false, applyOrder: false };
        if (context.hasOrder) {
            if (context.orderExpressions.length === 1 && context.orderExpressions[0].selector.expression.selector.memberName === context.objectStore.keyPath) {
                resultSet.applyOrder = true;
                resultSet.applyPaging = true;
            }
        } else {
            resultSet.applyPaging = true;
        }
        var curReq = null;
        if (resultSet.applyOrder && context.orderExpressions[0].nodeType === "OrderByDescending") {
            curReq = context.objectStore.openCursor(null, 'prev');
        } else {
            curReq = context.objectStore.openCursor();
        }

        curReq.onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                if (resultSet.applyPaging && !!context['skip']) {
                    var skip = context.skip;
                    context.skip = 0;
                    try {
                        cursor['advance'](skip);
                    } catch (e) {
                        context.callback.success(resultSet);
                    }
                } else {
                    resultSet.objects.push(cursor.value);
                    resultSet.ids.push(JSON.stringify(cursor.primaryKey));
                    if (resultSet.applyPaging && context['take'] !== undefined && context['take'] !== null) {
                        context.take = context.take - 1;
                        if (context.take > 0) {
                            cursor['continue']();
                        } else {
                            context.callback.success(resultSet);
                        }
                    } else {
                        cursor['continue']();
                    }
                }
            } else {
                context.callback.success(resultSet);
            }
        };
    },

    VisitIndexedDBLogicalAndFilterExpression: function VisitIndexedDBLogicalAndFilterExpression(expression, context) {
        var tmpCallback = context.callback;
        context.callback = {};
        var self = this;
        context.callback.success = function (lResult) {
            context.callback = {
                success: function success(rResult) {
                    var start = new Date().getTime();

                    var resultList = { ids: [], objects: [] };
                    if (lResult && lResult.ids && rResult && rResult.ids) {
                        var largeList = lResult.ids.length <= rResult.ids.length ? rResult : lResult;
                        var smallList = lResult.ids.length > rResult.ids.length ? rResult : lResult;
                        for (var i = 0; i < smallList.ids.length; i++) {

                            if (largeList.ids.indexOf(smallList.ids[i]) >= 0) {
                                resultList.objects.push(smallList.objects[i]);
                                resultList.ids.push(smallList.ids[i]);
                            }
                        }
                    } else if (lResult && lResult.ids && rResult) {
                        resultList = lResult;
                    } else if (rResult && rResult.ids && lResult) {
                        resultList = rResult;
                    }
                    _core2.default.Trace.log("Logical and: ", new Date().getTime() - start);
                    tmpCallback.success(resultList);
                }
            };
            self.Visit(expression.right, context);
            if (expression.right.expressionType == _core2.default.Expressions.ConstantExpression) {
                context.callback.success(context.value);
            }
        };
        this.Visit(expression.left, context);
        if (expression.left.expressionType == _core2.default.Expressions.ConstantExpression) {
            context.callback.success(context.value);
        }
    },
    VisitIndexedDBLogicalOrFilterExpression: function VisitIndexedDBLogicalOrFilterExpression(expression, context) {
        var tmpCallback = context.callback;
        context.callback = {};
        var self = this;
        context.callback.success = function (lResult) {
            context.callback = {
                success: function success(rResult) {
                    var start = new Date().getTime();

                    var resultList = { ids: [], objects: [] };
                    if (!lResult || lResult && !lResult.ids) lResult = rResult;
                    if (!rResult || rResult && !rResult.ids) rResult = lResult;
                    var resultList = lResult.ids.length <= rResult.ids.length ? rResult : lResult;
                    var smallList = lResult.ids.length > rResult.ids.length ? rResult : lResult;
                    for (var i = 0; i < smallList.ids.length; i++) {
                        if (resultList.ids.indexOf(smallList.ids[i]) < 0) {
                            resultList.objects.push(smallList.objects[i]);
                            resultList.ids.push(smallList.ids[i]);
                        }
                    }
                    _core2.default.Trace.log("Logical Or: ", new Date().getTime() - start);
                    tmpCallback.success(resultList);
                }
            };
            self.Visit(expression.right, context);
            if (expression.right.expressionType == _core2.default.Expressions.ConstantExpression) {
                context.callback.success(context.value);
            }
        };
        this.Visit(expression.left, context);
        if (expression.left.expressionType == _core2.default.Expressions.ConstantExpression) {
            context.callback.success(context.value);
        }
    },

    VisitIndexedDBPhysicalAndFilterExpression: function VisitIndexedDBPhysicalAndFilterExpression(expression, context) {
        var self = this;
        var cursor;
        var resultSet = { ids: [], objects: [] };
        if (expression.suggestedIndex) {
            var keyRange = undefined;
            switch (expression.suggestedIndex.nodeType) {
                case "equal":
                    keyRange = self.provider.IDBKeyRange.only(expression.suggestedIndex.value);
                    break;
                case "equalTyped":
                    keyRange = self.provider.IDBKeyRange.only(expression.suggestedIndex.value);
                    break;
                case "greaterThan":
                    keyRange = self.provider.IDBKeyRange.lowerBound(expression.suggestedIndex.value, true);
                    break;
                case "greaterThanOrEqual":
                    keyRange = self.provider.IDBKeyRange.lowerBound(expression.suggestedIndex.value, false);
                    break;
                case "lessThan":
                    keyRange = self.provider.IDBKeyRange.upperBound(expression.suggestedIndex.value, true);
                    break;
                case "lessThenOrEqual":
                    keyRange = self.provider.IDBKeyRange.upperBound(expression.suggestedIndex.value, false);
                    break;
                default:
                    alert("filter nodetype:" + expression.suggestedIndex.nodeType);
                    break;
            }
            //expression.filters.splice(expression.suggestedIndex.index, 1);
            cursor = self.objectStore.index(expression.suggestedIndex.name).openCursor(keyRange);
        } else {
            cursor = self.objectStore.openCursor();
        }

        cursor.onsuccess = function (event) {
            var c = event.target.result;
            if (c) {
                var i = 0;
                var addToResultSet = true;
                while (expression.filters[i]) {
                    var filter = expression.filters[i++];

                    var filterCtx = { instance: c.value, value: undefined };
                    self.Visit(filter.left, filterCtx);
                    var lValue = filterCtx.value;

                    var filterCtx = { instance: c.value, value: undefined };
                    self.Visit(filter.right, filterCtx);
                    var rValue = filterCtx.value;
                    switch (filter.nodeType) {
                        case "equal":
                            addToResultSet &= lValue == rValue;
                            break;
                        case "notEqual":
                            addToResultSet &= lValue != rValue;
                            break;
                        case "equalTyped":
                            addToResultSet &= lValue === rValue;
                            break;
                        case "notEqualTyped":
                            addToResultSet &= lValue !== rValue;
                            break;
                        case "greaterThan":
                            addToResultSet &= lValue > rValue;
                            break;
                        case "greaterThanOrEqual":
                            addToResultSet &= lValue >= rValue;
                            break;
                        case "lessThan":
                            addToResultSet &= lValue < rValue;
                            break;
                        case "lessThenOrEqual":
                            addToResultSet &= lValue <= rValue;
                            break;
                        case "in":
                            if (rValue instanceof Array) {
                                addToResultSet &= rValue.map(function (item) {
                                    return item.value;
                                }).indexOf(lValue) >= 0;
                            } else {
                                throw new _core.Exception("Not supported");
                            }
                            break;
                        default:
                            alert("filter nodetype:" + filter.nodeType);
                            break;
                    }
                }
                if (addToResultSet) {
                    resultSet.objects.push(c.value);
                    resultSet.ids.push(JSON.stringify(c.primaryKey));
                }
                c['continue']();
            } else {
                context.callback.success(resultSet);
            }
        };
    },

    VisitConstantExpression: function VisitConstantExpression(expression, context) {
        context.value = expression.value;
    },
    VisitEntityFieldExpression: function VisitEntityFieldExpression(expression, context) {
        if (context.instance) {
            context.value = context.instance[expression.selector.memberName];
        }
    },
    VisitEntityFieldOperationExpression: function VisitEntityFieldOperationExpression(expression, context) {
        var ctx = { instance: context.instance };
        this.Visit(expression.source, ctx);
        var item = ctx.value;

        var params = [];
        for (var i = 0; i < expression.parameters.length; i++) {
            ctx.value = null;
            this.Visit(expression.parameters[i], ctx);
            if (ctx.value !== null) {
                params.push(ctx.value);
            }
        }
        if (item === null || item === undefined) {
            context.value = item;
        } else {
            if (expression.compiledFn) {
                context.value = expression.compiledFn(item, params);
            } else {
                if (item.hasOwnProperty(expression.operation.memberDefinition.mapTo)) {
                    context.value = item[expression.operation.memberDefinition.mapTo];
                } else if (typeof item[expression.operation.memberDefinition.mapTo] === 'function') {
                    context.value = item[expression.operation.memberDefinition.mapTo].apply(item, params);
                } else {
                    var f = new Function("item", "params", "return " + expression.operation.memberDefinition.mapTo + ".apply(item,params);");
                    expression.compiledFn = f;
                    context.value = f(item, params);
                }
            }
        }
    }
});

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}],4:[function(_dereq_,module,exports){
'use strict';

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

(0, _core.$C)('$data.storageProviders.IndexedDBPro.IndexedDBLogicalAndFilterExpression', _core2.default.Expressions.ExpressionNode, null, {
    constructor: function constructor(l, r) {
        this.left = l;
        this.right = r;
    },
    nodeType: { value: _core2.default.Expressions.ExpressionType.IndexedLogicalAnd, enumerable: true }
});

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}],5:[function(_dereq_,module,exports){
'use strict';

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

(0, _core.$C)('$data.storageProviders.IndexedDBPro.IndexedDBLogicalInFilterExpression', _core2.default.Expressions.ExpressionNode, null, {
    constructor: function constructor(l, r) {
        this.left = l;
        this.right = r;
    },
    nodeType: { value: _core2.default.Expressions.ExpressionType.IndexedLogicalAnd, enumerable: true }
});

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}],6:[function(_dereq_,module,exports){
'use strict';

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

(0, _core.$C)('$data.storageProviders.IndexedDBPro.IndexedDBLogicalOrFilterExpression', _core2.default.Expressions.ExpressionNode, null, {
    constructor: function constructor(l, r) {
        this.left = l;
        this.right = r;
    },
    nodeType: { value: _core2.default.Expressions.ExpressionType.IndexedLogicalOr, enumerable: true }
});

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}],7:[function(_dereq_,module,exports){
'use strict';

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

(0, _core.$C)('$data.storageProviders.IndexedDBPro.IndexedDBPhysicalAndFilterExpression', _core2.default.Expressions.ExpressionNode, null, {
    constructor: function constructor(f) {
        this.filters = f;
    },
    nodeType: { value: _core2.default.Expressions.ExpressionType.IndexedPhysicalAnd, enumerable: true }
});

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}],8:[function(_dereq_,module,exports){
'use strict';

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

_core2.default.Class.define('$data.storageProviders.indexedDbPro.IndexedDBStorageProvider', _core2.default.StorageProviderBase, null, {
    constructor: function constructor(cfg, ctxInstance) {
        // mapping IndexedDB types to browser invariant name
        this.indexedDB = _core2.default.__global.indexedDB || _core2.default.__global.webkitIndexedDB || _core2.default.__global.mozIndexedDB || _core2.default.__global.msIndexedDB;
        this.IDBRequest = _core2.default.__global.IDBRequest || _core2.default.__global.webkitIDBRequest || _core2.default.__global.mozIDBRequest || _core2.default.__global.msIDBRequest;
        this.IDBTransaction = _core2.default.__global.IDBTransaction || _core2.default.__global.webkitIDBTransaction || _core2.default.__global.mozIDBTransaction || _core2.default.__global.msIDBTransaction;
        this.IDBTransactionType = { READ_ONLY: "readonly", READ_WRITE: "readwrite", VERSIONCHANGE: "versionchange" };
        if (typeof this.IDBTransaction.READ_ONLY !== 'undefined' && typeof this.IDBTransaction.READ_WRITE !== 'undefined') {
            this.IDBTransactionType.READ_ONLY = this.IDBTransaction.READ_ONLY;
            this.IDBTransactionType.READ_WRITE = this.IDBTransaction.READ_WRITE;
        }

        this.IDBKeyRange = _core2.default.__global.IDBKeyRange || _core2.default.__global.webkitIDBKeyRange || _core2.default.__global.mozIDBKeyRange || _core2.default.__global.msIDBKeyRange;
        this.IDBDatabaseException = _core2.default.__global.IDBDatabaseException || _core2.default.__global.webkitIDBDatabaseException || _core2.default.__global.mozIDBDatabaseException || _core2.default.__global.msIDBDatabaseException;
        this.IDBOpenDBRequest = _core2.default.__global.IDBOpenDBRequest || _core2.default.__global.webkitIDBOpenDBRequest || _core2.default.__global.mozIDBOpenDBRequest || _core2.default.__global.msIDBOpenDBRequest;
        this.newVersionAPI = !!(_core2.default.__global.IDBFactory && IDBFactory.prototype.deleteDatabase);
        this.sequenceStore = '__jayData_sequence';
        this.SqlCommands = [];
        this.context = ctxInstance;
        this.providerConfiguration = _core2.default.typeSystem.extend({
            databaseName: _core2.default.defaults.defaultDatabaseName,
            version: 1,
            dbCreation: _core2.default.storageProviders.DbCreationType.DropTableIfChanged,
            memoryOperations: true
        }, cfg);
        this._setupExtensionMethods();

        if (ctxInstance) this.originalContext = ctxInstance.getType();

        if (this.context && this.context._buildDbType_generateConvertToFunction && this.buildDbType_generateConvertToFunction) {
            this.context._buildDbType_generateConvertToFunction = this.buildDbType_generateConvertToFunction;
        }
        if (this.context && this.context._buildDbType_modifyInstanceDefinition && this.buildDbType_modifyInstanceDefinition) {
            this.context._buildDbType_modifyInstanceDefinition = this.buildDbType_modifyInstanceDefinition;
        }
    },
    supportedBinaryOperators: {
        value: {
            equal: { mapTo: ' == ', dataType: _core2.default.Boolean },
            notEqual: { mapTo: ' != ', dataType: _core2.default.Boolean },
            equalTyped: { mapTo: ' === ', dataType: _core2.default.Boolean },
            notEqualTyped: { mapTo: ' !== ', dataType: _core2.default.Boolean },
            greaterThan: { mapTo: ' > ', dataType: _core2.default.Boolean },
            greaterThanOrEqual: { mapTo: ' >= ', dataType: _core2.default.Boolean },
            lessThan: { mapTo: ' < ', dataType: _core2.default.Boolean },
            lessThenOrEqual: { mapTo: ' <= ', dataType: _core2.default.Boolean },

            or: { mapTo: ' || ', dataType: _core2.default.Boolean },
            and: { mapTo: ' && ', dataType: _core2.default.Boolean },
            'in': { mapTo: ' in ', dataType: _core2.default.Boolean, resolvableType: [_core2.default.Array] }
        }
    },
    supportedSetOperations: {
        value: {
            filter: {},
            map: {},
            length: {},
            forEach: {},
            toArray: {},
            single: {},
            some: {},
            every: {},
            take: {},
            skip: {},
            orderBy: {},
            orderByDescending: {},
            first: {}
        },
        enumerable: true,
        writable: true
    },
    supportedFieldOperations: {
        value: {
            length: { mapTo: "length", dataType: 'number' },
            startsWith: { mapTo: "$data.StringFunctions.startsWith", dataType: _core2.default.Boolean, parameters: [{ name: "p1", dataType: "string" }] },
            endsWith: { mapTo: "$data.StringFunctions.endsWith", dataType: _core2.default.Boolean, parameters: [{ name: "p1", dataType: "string" }] },
            contains: { mapTo: "$data.StringFunctions.contains", dataType: _core2.default.Boolean, parameters: [{ name: "p1", dataType: "string" }] },
            substr: { mapTo: "substr", dataType: _core2.default.String, parameters: [{ name: "startFrom", dataType: "number" }, { name: "length", dataType: "number" }] },
            toLowerCase: { mapTo: "toLowerCase", dataType: _core2.default.String },
            toUpperCase: { mapTo: "toUpperCase", dataType: _core2.default.String },
            trim: { mapTo: 'trim', dataType: _core2.default.String },
            ltrim: { mapTo: 'trimLeft', dataType: _core2.default.String },
            rtrim: { mapTo: 'trimRight', dataType: _core2.default.String }
        },
        enumerable: true,
        writable: true
    },
    supportedUnaryOperators: {
        value: {},
        enumerable: true,
        writable: true
    },
    _setupExtensionMethods: function _setupExtensionMethods() {
        /// <summary>
        /// Sets the extension method 'setCallback' on IDBRequest, IDBOpenDBRequest, and IDBTransaction types
        /// </summary>
        var self = this;
        var idbRequest = this.IDBRequest;
        var idbTran = this.IDBTransaction;
        var idbOpenDBRequest = this.IDBOpenDBRequest;
        var setCallbacks = function setCallbacks(callbackSettings) {
            /// <summary>
            /// Sets the callbacks on the object.
            /// </summary>
            /// <param name="callbackSettings">Named value pairs of the callbacks</param>
            if ((typeof callbackSettings === 'undefined' ? 'undefined' : _typeof(callbackSettings)) !== 'object') _core.Guard.raise(new _core.Exception('Invalid callbackSettings', null, callbackSettings));
            for (var i in callbackSettings) {
                if (typeof this[i] === 'undefined' || typeof callbackSettings[i] !== 'function') continue;
                this[i] = callbackSettings[i];
            }

            //if (this.readyState == self.IDBRequest.DONE)
            //    console.log('WARNING: request finished before setCallbacks. Do not use breakpoints between creating the request object and finishing the setting of callbacks');
            return this;
        };
        if (idbRequest && typeof idbRequest.prototype.setCallbacks !== 'function') idbRequest.prototype.setCallbacks = setCallbacks;
        if (idbTran && typeof idbTran.prototype.setCallbacks !== 'function') idbTran.prototype.setCallbacks = setCallbacks;
        if (idbOpenDBRequest && typeof idbOpenDBRequest.prototype.setCallbacks !== 'function') idbOpenDBRequest.prototype.setCallbacks = setCallbacks;
    },
    supportedDataTypes: {
        value: [_core2.default.Integer, _core2.default.Number, _core2.default.Date, _core2.default.String, _core2.default.Boolean, _core2.default.Blob, _core2.default.Array, _core2.default.Object, _core2.default.Guid, _core2.default.GeographyPoint, _core2.default.GeographyLineString, _core2.default.GeographyPolygon, _core2.default.GeographyMultiPoint, _core2.default.GeographyMultiLineString, _core2.default.GeographyMultiPolygon, _core2.default.GeographyCollection, _core2.default.GeometryPoint, _core2.default.GeometryLineString, _core2.default.GeometryPolygon, _core2.default.GeometryMultiPoint, _core2.default.GeometryMultiLineString, _core2.default.GeometryMultiPolygon, _core2.default.GeometryCollection, _core2.default.Byte, _core2.default.SByte, _core2.default.Decimal, _core2.default.Float, _core2.default.Int16, _core2.default.Int32, _core2.default.Int64, _core2.default.Duration, _core2.default.Day, _core2.default.Time, _core2.default.DateTimeOffset],
        writable: false
    },
    fieldConverter: { value: _core2.default.IndexedDBProConverter },
    supportedAutoincrementKeys: {
        value: {
            '$data.Integer': true,
            '$data.Int32': true,
            '$data.Guid': function $dataGuid() {
                return _core2.default.createGuid();
            }
        }
    },

    _getObjectStoreDefinition: function _getObjectStoreDefinition(setDefinition) {
        var contextStore = {
            storeName: setDefinition.TableName
        };
        var keyFields = setDefinition.PhysicalType.memberDefinitions.getKeyProperties();

        if (0 == keyFields.length) {
            var error = new Error("Entity must have a key field: " + contextStore.storeName);
            error.name = "KeyNotFoundError";
            throw error;
        }
        /*for (var i = 0; i < keyFields.length; i++) {
             if (keyFields[i].computed === true &&
                ("$data.Integer" !== Container.resolveName(keyFields[i].type))) {
                var error = new Error("Computed key field must be of integer type: " + contextStore.storeName);
                error.name = "ComputedKeyFieldError";
                throw error;
            }
            if (keyFields.length > 2 && keyFields[i].computed) {
                var error = new Error("With multiple keys the computed field is not allowed: " + contextStore.storeName);
                error.name = "MultipleComputedKeyFieldError";
                throw error;
            }
        }*/
        if (keyFields.length > 2 && keyFields.some(function (memDef) {
            return memDef.computed;
        })) {
            _core.Guard.raise("With multiple keys the computed field is not allowed: " + contextStore.storeName, "MultipleComputedKeyFieldError");
        }

        for (var i = 0; i < keyFields.length; i++) {
            var typeName = _core.Container.resolveName(keyFields[i].type);
            if (keyFields[i].computed && !this.supportedAutoincrementKeys[typeName]) {
                console.log("WARRNING! '" + typeName + "' not supported as computed Key!");
            }
        }

        contextStore.indices = setDefinition.indices;
        contextStore.keyFields = keyFields;
        return contextStore;
    },
    _getObjectStoreDefinitions: function _getObjectStoreDefinitions() {
        var objectStoreDefinitions = [];
        var self = this;
        self.context._storageModel.forEach(function (memDef) {
            var objectStoreDefinition = self._getObjectStoreDefinition(memDef);
            objectStoreDefinitions.push(objectStoreDefinition);
        });
        return objectStoreDefinitions;
    },

    _oldCreateDB: function _oldCreateDB(setVersionTran, definitions, callBack) {
        var self = this;
        setVersionTran.db.onversionchange = function (event) {
            return event.target.close();
        };

        self._createDB(setVersionTran.db, definitions, {
            success: function success() {
                setVersionTran.oncomplete = callBack.success;
            },
            error: callBack.error
        });
    },
    _createDB: function _createDB(db, definitions, callBack) {
        for (var i = 0; i < definitions.length; i++) {
            if (definitions[i].dropIfExists && db.objectStoreNames.contains(definitions[i].storeName)) {
                db.deleteObjectStore(definitions[i].storeName);
            }
        }
        if (this.context._deleteTables) {
            var keys = Object.keys(this.context._deleteTables);
            for (var i = 0; i < keys.length; i++) {
                if (this.context._deleteTables[keys[i]] && db.objectStoreNames.contains(keys[i])) {
                    db.deleteObjectStore(keys[i]);
                }
            }
        }
        try {
            for (var i = 0; i < definitions.length; i++) {
                var storeDef = definitions[i];

                if (!db.objectStoreNames.contains(storeDef.storeName)) {
                    var settings = {};
                    if (storeDef.keyFields.length == 1) {
                        settings = {
                            keyPath: storeDef.keyFields[0].name
                            //autoIncrement: storeDef.keyFields[0].computed
                        };
                        var typeName = _core.Container.resolveName(storeDef.keyFields[0].type);
                        settings.autoIncrement = this.supportedAutoincrementKeys[typeName] ? true : false;
                    } else {
                        settings.key = [];
                        for (var i = 0; i < storeDef.keyFields.length; i++) {
                            settings.key.push(storeDef.keyFields[i].name);
                        }
                    }
                    var objStore = db.createObjectStore(storeDef.storeName, settings);
                    if (storeDef.indices && storeDef.indices.length > 0) {
                        for (var idx = 0; idx < storeDef.indices.length; idx++) {
                            var idx_name = storeDef.indices[idx].name;
                            var idx_keys = storeDef.indices[idx].keys;
                            var idx_unique = storeDef.indices[idx].unique;
                            if (!idx_keys || idx_keys && idx_keys.length < 1) {
                                _core.Guard.raise(new _core.Exception("Index create error: Keys field is required!"));
                            }
                            if (typeof idx_keys[0] !== "string") {
                                idx_keys = idx_keys.map(function (k) {
                                    return k.fieldName;
                                });
                            }
                            if (typeof idx_keys[0] !== "string") {
                                _core.Guard.raise(new _core.Exception("Index create error: type of fieldName property must be string!"));
                            }

                            try {
                                objStore.createIndex(idx_name, idx_keys, { unique: idx_unique });
                            } catch (e) {
                                _core.Guard.raise(new _core.Exception('Index create error', 'Exception', e));
                            }
                        }
                    }
                }
            }
        } catch (e) {
            callBack.error(e);
            return;
        }
        callBack.success();
    },
    _hasDbChanges: function _hasDbChanges(db, transaction, definitions, dropTabes, callBack) {
        var isOriginal = true;
        var tran = transaction;
        if (!tran && db.objectStoreNames.length > 0) {
            tran = db.transaction(db.objectStoreNames, this.IDBTransactionType.READ_ONLY);
        }
        for (var i = 0; i < definitions.length; i++) {
            if (!dropTabes && db.objectStoreNames.contains(definitions[i].storeName)) {
                //check pk change

                var os = tran.objectStore(definitions[i].storeName);
                var keyPath = [].concat(os.keyPath).sort(function (a, b) {
                    return a == b ? 0 : a > b ? 1 : -1;
                });
                var defKeyPath = definitions[i].keyFields.map(function (memDef) {
                    return memDef.name;
                }).sort(function (a, b) {
                    return a == b ? 0 : a > b ? 1 : -1;
                });
                if (keyPath.length === defKeyPath.length) {
                    for (var j = 0; j < keyPath.length; j++) {
                        isOriginal = isOriginal && keyPath[j] === defKeyPath[j];
                    }
                    definitions[i].changed = !isOriginal;
                } else {
                    isOriginal = false;
                    definitions[i].changed = true;
                }

                isOriginal = isOriginal && true;
            } else {

                isOriginal = false;
                definitions[i].changed = true;
                definitions[i].dropIfExists = dropTabes;
            }

            if (this.context._deleteTables && this.context._deleteTables[definitions[i].storeName]) {
                isOriginal = false;
                definitions[i].changed = true;
                definitions[i].dropIfExists = true;
            }
        }

        callBack.success(!isOriginal);
    },
    onupgradeneeded: function onupgradeneeded(objectStoreDefinitions, callBack) {
        var self = this;
        return function (e) {
            self.db = e.target.result;
            self.db.onversionchange = function (event) {
                return event.target.close();
            };
            self._hasDbChanges(self.db, e.target.transaction, objectStoreDefinitions, self.providerConfiguration.dbCreation == _core2.default.storageProviders.DbCreationType.DropAllExistingTables, {
                success: function success(hasTableChanges) {
                    self.___hasTableChanges = hasTableChanges;
                    if (hasTableChanges) self._createDB(self.db, objectStoreDefinitions, callBack);
                },
                error: callBack.error
            });
        };
    },

    initializeStore: function initializeStore(_callBack) {
        var self = this;
        var callBack;
        callBack = _core2.default.typeSystem.createCallbackSetting({
            success: _callBack.success,
            error: function error(e) {
                console.log(e);
                if (self.db && self.db.close) {
                    self.db.close();
                }
                if (callBack.hasError) return;

                callBack.hasError = true;
                _callBack.error.apply(this, arguments);
            }
        });
        callBack.hasError = false;

        var objectStoreDefinitions;
        try {
            objectStoreDefinitions = this._getObjectStoreDefinitions();
        } catch (e) {
            _core2.default.Trace.log(objectStoreDefinitions);
            callBack.error(e);
            return;
        }
        this.indexedDB.open(this.providerConfiguration.databaseName).setCallbacks({
            onsuccess: function onsuccess(e) {
                if (callBack.hasError) return;

                var db = e.target.result;
                db.onversionchange = function (event) {
                    return event.target.close();
                };
                self.db = db;

                self._hasDbChanges(self.db, e.target.transaction, objectStoreDefinitions, self.providerConfiguration.dbCreation == _core2.default.storageProviders.DbCreationType.DropAllExistingTables, {
                    success: function success(hasTableChanges) {
                        //oldAPI
                        if (db.setVersion) {
                            if (db.version === "" || hasTableChanges) {
                                db.setVersion((parseInt(db.version) || 0) + 1).setCallbacks({
                                    onsuccess: function onsuccess(e) {
                                        self.db = e.target.result;
                                        self._oldCreateDB(self.db /*setVerTran*/, objectStoreDefinitions, {
                                            success: function success(e) {
                                                if (callBack.hasError) return;
                                                //self.db = e.target.db;
                                                //callBack.success(self.context);
                                                self.storeInitialized(e.target.db, callBack);
                                            },
                                            error: callBack.error
                                        });
                                    },
                                    onerror: callBack.error
                                    //onblocked: callBack.error
                                });
                                return;
                            };
                        } else if (hasTableChanges) {
                            //newVersionAPI
                            db.close();
                            setTimeout(function () {
                                var version = parseInt(db.version) + 1;
                                self.indexedDB.open(self.providerConfiguration.databaseName, version).setCallbacks({
                                    onsuccess: function onsuccess(e) {
                                        if (callBack.hasError) return;
                                        //self.db = e.target.result;
                                        //callBack.success(self.context);
                                        self.storeInitialized(e.target.result, callBack);
                                    },
                                    onupgradeneeded: self.onupgradeneeded(objectStoreDefinitions, {
                                        success: function success() {},
                                        error: callBack.error
                                    }),
                                    onerror: callBack.error,
                                    onabort: callBack.error
                                    //onblocked: callBack.error
                                });
                            }, 0);
                            return;
                        }

                        //callBack.success(self.context);
                        self.storeInitialized(self.db, callBack);
                    },
                    error: _callBack.error
                });
            },
            //newVersionAPI
            onupgradeneeded: this.onupgradeneeded(objectStoreDefinitions, {
                success: function success() {},
                error: callBack.error
            }),
            onerror: callBack.error,
            onabort: callBack.error
            //onblocked: callBack.error
        });
    },
    storeInitialized: function storeInitialized(db, callBack) {
        var self = this;
        self.db = db;
        if (self.___hasTableChanges && typeof self.providerConfiguration.onUpdated === 'function') {
            delete self.___hasTableChanges;
            self.providerConfiguration.onUpdated(self.context, {
                success: function success() {
                    callBack.success(self.context);
                },
                error: function error(ex) {
                    if (!ex || ex instanceof _core2.default.EntityContext) {
                        ex = new _core.Exception('onUpdated failed');
                    }

                    callBack.error.call(this, ex);
                }
            });
        } else {
            delete self.___hasTableChanges;
            callBack.success(self.context);
        }
    },

    buildDbType_modifyInstanceDefinition: function buildDbType_modifyInstanceDefinition() {
        return;
    },
    buildDbType_generateConvertToFunction: function buildDbType_generateConvertToFunction(storageModel, context) {
        return function (logicalEntity, provider) {
            var dbInstance = new storageModel.PhysicalType();
            dbInstance.entityState = logicalEntity.entityState;

            storageModel.PhysicalType.memberDefinitions.getPublicMappedProperties().forEach(function (memDef) {
                var typeName = _core.Container.resolveName(memDef.type);
                var value = logicalEntity[memDef.name];

                if (memDef.key && memDef.computed && value === undefined) {
                    if (typeof provider.supportedAutoincrementKeys[typeName] === 'function') {
                        value = provider.supportedAutoincrementKeys[typeName]();
                    } else {
                        // Autogenerated fields for new items should not be present in the physicalData
                        return;
                    }
                }

                if (provider.fieldConverter.toDb[typeName]) {
                    dbInstance.initData[memDef.name] = provider.fieldConverter.toDb[typeName](value);
                } else {
                    console.log('WARN!!!');
                    dbInstance.initData[memDef.name] = value;
                }
            }, this);

            if (storageModel.ComplexTypes) {
                storageModel.ComplexTypes.forEach(function (cmpType) {
                    var value = logicalEntity[cmpType.FromPropertyName];
                    if (value !== undefined) {
                        value = JSON.parse(JSON.stringify(value));
                    }
                    dbInstance.initData[cmpType.FromPropertyName] = value;
                }, this);
            }
            return dbInstance;
        };
    },
    _compile: function _compile(query, callback) {
        var compiler = _core2.default.storageProviders.IndexedDBPro.IndexedDBCompiler.create(this);
        var compiledQuery = compiler.compile(query, {
            success: function success(compiledQuery) {
                callback.success(compiledQuery);
            }
        });

        return compiledQuery;
    },
    getTraceString: function getTraceString(query) {
        var compiledExpression = this._compile(query);
        var executor = _core2.default.storageProviders.IndexedDBPro.IndexedDBExpressionExecutor.create(this);
        executor.runQuery(compiledExpression);
        return compiledExpression;
    },
    executeQuery: function executeQuery(query, callBack) {
        var self = this;
        var start = new Date().getTime();
        callBack = _core2.default.typeSystem.createCallbackSetting(callBack);

        var doQuery = function doQuery() {
            self._compile(query, {
                success: function success(expression) {
                    var executor = _core2.default.storageProviders.IndexedDBPro.IndexedDBExpressionExecutor.create(self, query.transaction);
                    executor.runQuery(expression, {
                        success: function success(result) {
                            var modelBinderCompiler = _core.Container.createModelBinderConfigCompiler(query, []);
                            modelBinderCompiler.Visit(query.expression);
                            query.rawDataList = result;
                            _core2.default.Trace.log("execute Query in milliseconds:", new Date().getTime() - start);
                            callBack.success(query);
                        }
                    });
                }
            });
        };

        doQuery();
    },
    loadRawData: function loadRawData(tableName, callBack) {
        var self = this;
        callBack = _core2.default.typeSystem.createCallbackSetting(callBack);

        this.indexedDB.open(this.providerConfiguration.databaseName).setCallbacks({
            onsuccess: function onsuccess(e) {

                var rawData = [];
                if (!e.target.result.objectStoreNames.contains(tableName)) {
                    if (e.target.result.close) e.target.result.close();

                    setTimeout(function () {
                        callBack.success(rawData);
                    }, 0);
                    return;
                }

                var tran = e.target.result.transaction([tableName], self.IDBTransactionType.READ_ONLY);
                tran.onerror = callBack.error;
                tran.onabort = callBack.error;
                tran.oncomplete = function () {
                    if (e.target.result.close) e.target.result.close();

                    setTimeout(function () {
                        callBack.success(rawData);
                    }, 0);
                };

                var store = tran.objectStore(tableName);
                store.openCursor().onsuccess = function (event) {
                    var cursor = event.target.result;
                    if (cursor) {
                        rawData.push(cursor.value);
                        cursor['continue']();
                    }
                };
            },
            onupgradeneeded: function onupgradeneeded() {},
            onerror: callBack.error,
            onabort: callBack.error
        });
    },
    _getKeySettings: function _getKeySettings(memDef) {
        /// <summary>
        /// Gets key settings for item type's member definition
        /// </summary>
        /// <param name="memDef">memDef of item</param>
        /// <returns>KeySettings object</returns>
        var self = this;
        var settings = { autoIncrement: false };
        var keys = [];
        memDef.PhysicalType.memberDefinitions.getPublicMappedProperties().forEach(function (item) {
            if (item.key) {
                // We found a key
                keys.push(item.name);
            }
            if (item.computed) {
                // AutoIncrement field, must be key
                if (!item.key) _core.Guard.raise(new _core.Exception('Only key field can be a computed field!'));

                var typeName = _core.Container.resolveName(item.type);
                if (self.supportedAutoincrementKeys[typeName] === true) {
                    settings.autoIncrement = true;
                }
            }
        });
        if (keys.length > 1) {
            if (settings.autoIncrement) _core.Guard.raise(new _core.Exception('Auto increment is only valid for a single key!'));
            // Setting key fields (composite key)
            settings.keys = keys;
        } else if (keys.length == 1) {
            // Simple key
            settings.keyPath = keys[0];
        } else {
            _core.Guard.raise(new _core.Exception('No valid key found!'));
        }
        return settings;
    },

    _beginTran: function _beginTran(tableList, isWrite, callBack) {
        var self = this;
        setTimeout(function () {
            callBack = _core2.default.typeSystem.createCallbackSetting(callBack);
            try {
                var transaction = new _core2.default.storageProviders.IndexedDBPro.IndexedDBTransaction();
                var tran = self.db.transaction(tableList ? tableList : self.db.objectStoreNames, isWrite ? self.IDBTransactionType.READ_WRITE : self.IDBTransactionType.READ_ONLY);

                tran.oncomplete = function () {
                    _core2.default.Trace.log("oncomplete: ", transaction._objectId);
                    if (transaction.oncomplete) {
                        transaction.oncomplete.fire(arguments, transaction);
                    }
                };
                tran.onerror = function () {
                    _core2.default.Trace.log("onerror: ", transaction._objectId);
                    transaction.aborted = true;
                    if (transaction.onerror) {
                        transaction.onerror.fire(arguments, transaction);
                    }
                };
                tran.onabort = function () {
                    _core2.default.Trace.log("onabort: ", transaction._objectId);
                    if (!transaction.aborted) {
                        if (transaction.onerror) {
                            transaction.onerror.fire(arguments, transaction);
                        }
                    }
                };
                tran.onblocked = function () {
                    _core2.default.Trace.log("onblocked: ", transaction._objectId);
                    if (transaction.onabort) {
                        transaction.onabort.fire(arguments, transaction);
                    }
                };

                transaction.transaction = tran;

                callBack.success(transaction);
            } catch (e) {
                callBack.error(e);
            }
        }, 0);
    },

    saveChanges: function saveChanges(callBack, changedItems, tran) {
        var self = this;
        // Building independent blocks and processing them sequentially
        var independentBlocks = this.buildIndependentBlocks(changedItems);
        var objectStoreNames = [];
        for (var i = 0; i < independentBlocks.length; i++) {
            for (var j = 0; j < independentBlocks[i].length; j++) {
                if (objectStoreNames.indexOf(independentBlocks[i][j].entitySet.tableName) < 0) {
                    objectStoreNames.push(independentBlocks[i][j].entitySet.tableName);
                }
            }
        }
        if (objectStoreNames.length < 1) {
            callBack.success(tran);return;
        }

        this._saveChangesWithTran(callBack, independentBlocks, tran);
    },
    _saveChangesWithTran: function _saveChangesWithTran(callBack, independentBlocks, transaction) {
        var self = this;

        var doSave = function doSave() {
            if (independentBlocks.length == 0) {
                transaction.onerror.detach(t1);
                //transaction.onabort.detach(t2);
                callBack.success(transaction);
            } else {
                var currentBlock = independentBlocks.shift();
                var itemCount = currentBlock.length;
                var hasError = false;
                self._saveIndependentBlock(currentBlock, transaction, {
                    success: function success() {
                        if (--itemCount < 1 && !hasError) {
                            doSave();
                        }
                    },
                    error: function error(_error) {
                        hasError = true;
                        itemCount = 0;
                        transaction.hasError = true;
                        transaction.error = _error;
                        transaction.abort();
                    }
                });
            }
        };
        var t1 = null;
        var tranError = function tranError(sender, event) {
            this.onerror.detach(t1);
            callBack.error(transaction.error, transaction);
        };
        t1 = tranError;
        transaction.onerror.attach(tranError);
        doSave();
    },
    _saveIndependentBlock: function _saveIndependentBlock(items, tran, callBack) {
        var self = this;
        callBack = _core2.default.typeSystem.createCallbackSetting(callBack);
        var saveItem = function saveItem(item) {
            //var item = items[i];
            var physicalType = self.context._storageModel.getStorageModel(item.data.getType()).PhysicalType;
            item.physicalData = physicalType.convertTo(item.data, self);

            var keyValues = physicalType.memberDefinitions.getKeyProperties().map(function (memDef) {
                return item.physicalData[memDef.name];
            });

            if (keyValues.length == 1) {
                keyValues = keyValues[0];
            }
            if (tran.hasError) {
                callBack.success();
                return;
            }
            try {
                var store = tran.transaction.objectStore(item.entitySet.tableName);
                switch (item.data.entityState) {
                    case _core2.default.EntityState.Added:
                        var request = null;
                        if (keyValues instanceof _core2.default.Array) {
                            request = store.add(item.physicalData.initData, keyValues);
                        } else {
                            request = store.add(item.physicalData.initData);
                        }
                        request.setCallbacks({
                            onerror: function onerror(ex) {
                                callBack.error(ex);
                                return true;
                            },
                            onsuccess: function onsuccess(event) {
                                var newKey = event.target.result;
                                if (newKey instanceof _core2.default.Array) {
                                    physicalType.memberDefinitions.getKeyProperties().forEach(function (k, idx) {
                                        item.data[k.name] = newKey[idx];
                                    });
                                } else {
                                    item.data[physicalType.memberDefinitions.getKeyProperties()[0].name] = newKey;
                                }

                                callBack.success();
                            }
                        });
                        break;
                    case _core2.default.EntityState.Deleted:
                        store.openCursor(self.IDBKeyRange.only(keyValues)).setCallbacks({
                            onerror: function onerror(ex) {
                                callBack.error(ex);
                                return true;
                            },
                            onsuccess: function onsuccess(event) {
                                var cursor = event.target.result;
                                if (cursor) {
                                    cursor['delete']();
                                    callBack.success();
                                } else {
                                    callBack.error(new _core.Exception('Object not found'));
                                }
                            }
                        });
                        break;
                    case _core2.default.EntityState.Modified:
                        store.openCursor(self.IDBKeyRange.only(keyValues)).setCallbacks(_defineProperty({
                            onsuccess: function onsuccess(ex) {
                                callBack.error(ex);
                                return true;
                            }
                        }, 'onsuccess', function onsuccess(event) {
                            var cursor = event.target.result;
                            if (cursor) {
                                cursor.update(_core2.default.typeSystem.extend(cursor.value, physicalType.convertTo(item.data, self).initData));
                                callBack.success();
                            } else {
                                callBack.error(new _core.Exception('Object not found'));
                            }
                        }));
                        break;
                    case _core2.default.EntityState.Unchanged:
                        callBack.success();
                        break;
                    default:
                        _core.Guard.raise(new _core.Exception('Not supported entity state', null, item));
                }
            } catch (ex) {
                callBack.error(ex);
            }
        };
        for (var i = 0; i < items.length; i++) {
            saveItem(items[i]);
        }
    }
}, {
    isSupported: {
        get: function get() {
            return window.indexedDB || window.webkitIndexedDB || window.mozIndexedDB || window.msIndexedDB ? true : false;
        },
        set: function set() {}
    }
});

if (_core2.default.storageProviders.indexedDbPro.IndexedDBStorageProvider.isSupported) _core2.default.StorageProviderBase.registerProvider('indexedDb', _core2.default.storageProviders.indexedDbPro.IndexedDBStorageProvider);

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}],9:[function(_dereq_,module,exports){
'use strict';

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

_core2.default.Class.define('$data.storageProviders.IndexedDBPro.IndexedDBTransaction', _core2.default.Transaction, null, {
    abort: function abort() {
        _core2.default.Trace.log("onabort: ", this._objectId);
        this.transaction.abort();
    }
}, null);

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}],10:[function(_dereq_,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

var _IndexedDBConverter = _dereq_('./IndexedDBConverter.js');

var _IndexedDBConverter2 = _interopRequireDefault(_IndexedDBConverter);

var _IndexedDBCompiler = _dereq_('./IndexedDBCompiler.js');

var _IndexedDBCompiler2 = _interopRequireDefault(_IndexedDBCompiler);

var _IndexedDBExpressionExecutor = _dereq_('./IndexedDBExpressionExecutor.js');

var _IndexedDBExpressionExecutor2 = _interopRequireDefault(_IndexedDBExpressionExecutor);

var _IndexedDBLogicalAndFilterExpression = _dereq_('./IndexedDBLogicalAndFilterExpression.js');

var _IndexedDBLogicalAndFilterExpression2 = _interopRequireDefault(_IndexedDBLogicalAndFilterExpression);

var _IndexedDBLogicalInFilterExpression = _dereq_('./IndexedDBLogicalInFilterExpression.js');

var _IndexedDBLogicalInFilterExpression2 = _interopRequireDefault(_IndexedDBLogicalInFilterExpression);

var _IndexedDBLogicalOrFilterExpression = _dereq_('./IndexedDBLogicalOrFilterExpression.js');

var _IndexedDBLogicalOrFilterExpression2 = _interopRequireDefault(_IndexedDBLogicalOrFilterExpression);

var _IndexedDBPhysicalAndFilterExpression = _dereq_('./IndexedDBPhysicalAndFilterExpression.js');

var _IndexedDBPhysicalAndFilterExpression2 = _interopRequireDefault(_IndexedDBPhysicalAndFilterExpression);

var _IndexedDBTransaction = _dereq_('./IndexedDBTransaction.js');

var _IndexedDBTransaction2 = _interopRequireDefault(_IndexedDBTransaction);

var _IndexedDBStorageProvider = _dereq_('./IndexedDBStorageProvider.js');

var _IndexedDBStorageProvider2 = _interopRequireDefault(_IndexedDBStorageProvider);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _core2.default;
module.exports = exports['default'];

},{"./IndexedDBCompiler.js":1,"./IndexedDBConverter.js":2,"./IndexedDBExpressionExecutor.js":3,"./IndexedDBLogicalAndFilterExpression.js":4,"./IndexedDBLogicalInFilterExpression.js":5,"./IndexedDBLogicalOrFilterExpression.js":6,"./IndexedDBPhysicalAndFilterExpression.js":7,"./IndexedDBStorageProvider.js":8,"./IndexedDBTransaction.js":9,"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}]},{},[10])(10)
});

