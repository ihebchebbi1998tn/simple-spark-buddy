// JayData 1.5.12 Pro - Commercial License
// Copyright JayStack Technologies (http://jaydata.org/licensing)
// Support: http://jaystack.com
(function(f){if(typeof exports==="object"&&typeof module!=="undefined"){module.exports=f()}else if(typeof define==="function"&&define.amd){define("jaydata/modules/querycache",["jaydata/core"],f)}else{var g;if(typeof window!=="undefined"){g=window}else if(typeof global!=="undefined"){g=global}else if(typeof self!=="undefined"){g=self}else{g=this}g.$data = f()}})(function(){var define,module,exports;return (function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(_dereq_,module,exports){
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _core = _dereq_('jaydata/core');

var _core2 = _interopRequireDefault(_core);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _core2.default;


_core2.default.Class.define('$data.QueryCache', null, null, {
    constructor: function constructor(contextType, cacheKey) {
        this._queryCache = {};
        this._fix_Items = [];
        this._cacheKey = cacheKey;
    },
    _queryCache: {},
    _cacheKey: {},
    _fix_Items: {},

    reset: function reset() {
        _core2.default.Trace.log('QueryCache cache reset!', this._cacheKey);
        var keys = Object.keys(this._queryCache);
        for (var i = 0; i < keys.length; i++) {
            var cache = this._queryCache[keys[i]];
            if (cache && cache.timeoutId) {
                clearTimeout(cache.timeoutId);
            }
        }

        this._fix_Items.length = 0;
        this._queryCache = {};
    },

    hasCacheForQuery: function hasCacheForQuery(query) {
        try {
            var key = this._getCacheKey(query);
            return this._queryCache[key] ? true : false;
        } catch (e) {
            console.log('cant stringify to check in cache', query);
        }
        return false;
    },

    addToCache: function addToCache(query, timeout) {
        try {
            var key = this._getCacheKey(query);
            var timeoutId;
            if (this._queryCache[key]) {
                timeoutId = this._queryCache[key].timeoutId;
                if (timeoutId) {
                    clearTimeout(timeoutId);
                }
            }

            var self = this;
            if (typeof timeout === 'number') {
                timeoutId = setTimeout(function () {
                    delete self._queryCache[key];
                    _core2.default.Trace.log('QueryCache cache timeout', key);
                }, timeout);
            } else {
                if (this._fix_Items.length >= this.__maxCacheSize()) {
                    var oldKey = this._fix_Items.shift();
                    delete self._queryCache[oldKey];
                    _core2.default.Trace.log('QueryCache max cache size limit. Last removed: ', oldKey);
                }

                this._fix_Items.push(key);
            }

            this._queryCache[key] = {
                __count: query.__count,
                rawDataList: query.rawDataList,
                modelBinderConfig: query.modelBinderConfig,
                timeoutId: timeoutId
            };

            _core2.default.Trace.log('QueryCache data inserted to cache');
        } catch (e) {
            console.log('cant stringify to cache', query);
        }
    },

    executeQuery: function executeQuery(query, callBack) {
        try {
            var key = this._getCacheKey(query);
            _core2.default.Trace.log('QueryCache load from cache', this._cacheKey, key);

            var cache = this._queryCache[key];

            query.__count = cache.__count;
            query.rawDataList = cache.rawDataList;
            query.modelBinderConfig = cache.modelBinderConfig;
        } catch (e) {
            callBack.error(e);
            return;
        }

        callBack.success(query);
    },

    _getCacheKey: function _getCacheKey(query) {
        var frameTypeName = query.expression.getType().fullName;
        var key = frameTypeName + '_';
        if (query.expression.source instanceof _core2.default.QueryCacheNS.WithCacheExpression) key += query.expression.source.source.getJSON();else key += query.expression.source.getJSON();

        return _core2.default.QueryCache._keyHashCode(key);
    },
    __maxCacheSize: function __maxCacheSize() {
        return !_core2.default.defaults.maxQueryCacheSize || _core2.default.defaults.maxQueryCacheSize < 0 ? 100 : _core2.default.defaults.maxQueryCacheSize;
    }
}, {
    isCacheable: function isCacheable(context, query) {
        ///<param name="context" type:="$data.EntityContext" />
        ///<param name="query" type:="$data.Query" optional="true" />

        try {
            var dummyKey = _core2.default.QueryCache._generateCacheKey(context);
        } catch (e) {
            console.error('Cannot create unique cacheKey for context');
            return false;
        }

        if (context.enableQueryCache === false) return false;

        var isCacheable = _core2.default.QueryCache._cacheTimeout(context, query) ? true : false;
        return isCacheable;
    },

    isInCache: function isInCache(context, query) {
        ///<param name="context" type:="$data.EntityContext" />
        ///<param name="query" type:="$data.Query" />

        if (this.isCacheable(context, query)) {
            var cf = _core2.default.QueryCache._getQueryCache(context);
            if (cf.hasCacheForQuery(query)) {
                var withCache = _core2.default.QueryCache._isForce(query);
                var forced = typeof withCache !== 'undefined';

                if (forced) {
                    return withCache;
                } else {
                    return true;
                }
            }
        }
        return false;
    },
    executeQuery: function executeQuery(context, query, callback) {
        ///	<signature>
        ///     <param name="context" type:="$data.EntityContext" />
        ///     <param name="query" type:="$data.Query" />
        ///     <param name="query" type:="Function" />
        ///	</signature>
        ///	<signature>
        ///     <param name="context" type:="$data.EntityContext" />
        ///     <param name="query" type:="$data.Query" />
        ///     <param name="query" type:="Object" />
        ///	</signature>

        var cf = _core2.default.QueryCache._getQueryCache(context);
        cf.executeQuery(query, {
            success: callback.success,
            error: function error() {
                console.error('Cache execute error');
                console.error(arguments);
                context.storageProvider.executeQuery(query, callback);
            }
        });
    },
    reset: function reset(context) {
        ///<param name="context" type:="$data.EntityContext" />

        try {
            var contextType = context.getType();
            var key = _core2.default.QueryCache._generateCacheKey(context);

            if (contextType._queryCacheValues && contextType._queryCacheValues[key]) {
                contextType._queryCacheValues[key].reset();
            }
        } catch (e) {}
    },
    addToCache: function addToCache(context, query) {
        ///<param name="context" type:="$data.EntityContext" />
        ///<param name="query" type:="$data.Query" />

        var cf = _core2.default.QueryCache._getQueryCache(context);
        if (!cf.hasCacheForQuery(query) && _core2.default.QueryCache._isForce(query) !== false) {
            cf.addToCache(query, _core2.default.QueryCache._cacheTimeout(context, query));
        }
    },

    _getQueryCache: function _getQueryCache(context) {
        var contextType = context.getType();
        //var provider = context.storageProvider.getType();
        //var key = provider.fullName + '_' + contextType.fullName + '_' + JSON.stringify(context.storageProvider.providerConfiguration);
        var key = _core2.default.QueryCache._generateCacheKey(context);

        contextType._queryCacheValues = contextType._queryCacheValues || {};
        contextType._queryCacheValues[key] = contextType._queryCacheValues[key] || new _core2.default.QueryCache(contextType, key);
        return contextType._queryCacheValues[key];
    },
    _generateCacheKey: function _generateCacheKey(context) {
        var contextType = context.getType();
        var provider = context.storageProvider.getType();
        var key = provider.fullName + '_' + contextType.fullName + '_' + JSON.stringify(context.storageProvider.providerConfiguration);
        return _core2.default.QueryCache._keyHashCode(key);
    },
    _isForce: function _isForce(query) {
        if (query && query.expression.source instanceof _core2.default.QueryCacheNS.WithCacheExpression) {
            return query.expression.source.value.value;
        }
        return undefined;
    },
    _cacheTimeout: function _cacheTimeout(context, query) {
        return _core2.default.QueryCache._isForce(query) || context.queryCache || _core2.default.defaults.queryCache;
    },
    _keyHashCode: function _keyHashCode(key) {
        if (typeof key !== 'string') return 0;

        var hash = 0,
            i,
            charC;
        if (key.length == 0) return hash;
        for (i = 0; i < key.length; i++) {
            charC = key.charCodeAt(i);
            hash = (hash << 5) - hash + charC;
            hash = hash & hash;
        }
        return hash;
    },
    _maxCacheSize: function _maxCacheSize() {
        return !_core2.default.defaults.maxQueryCacheSize || _core2.default.defaults.maxQueryCacheSize < 0 ? 100 : _core2.default.defaults.maxQueryCacheSize;
    },
    __allQuerySize: { type: 'int', value: 0 }
});

_core2.default.EntityContext.addMember('cacheReset', function () {
    _core2.default.QueryCache.reset(this);
});

(0, _core.$C)('$data.QueryCacheNS.WithCacheExpression', _core2.default.Expressions.ExpressionNode, null, {
    constructor: function constructor(source, expression) {
        this.source = source;
        this.value = expression;
    },
    nodeType: { value: 'WithCache', enumerable: true }
});

_core2.default.Expressions.EntityExpressionVisitor.addMember('VisitWithCacheExpression', function (expression, context) {
    var newExpression = this.Visit(expression.source, context);
    //var value = this.Visit(expression.value, context);
    if (newExpression !== expression.source /*|| value !== expression.value*/) {
            return new _core2.default.QueryCacheNS.WithCacheExpression(newExpression, expression.value);
        }
    return expression;
});

_core2.default.Queryable.addMember('withCache', function (isCache) {
    ///	<signature>
    ///     <param name="isCache" type:="number" />
    ///	</signature>
    ///	<signature>
    ///     <param name="isCache" type:="boolean" />
    ///	</signature>

    if (_core.Guard.isNullOrUndefined(isCache)) isCache = true;
    var cacheValue = typeof isCache === 'number' ? isCache : _core.Container.convertTo(isCache, _core2.default.Boolean),
        cacheType = typeof cacheValue === 'undefined' ? 'undefined' : _typeof(cacheValue);

    var constExp = _core.Container.createConstantExpression(cacheValue, cacheType);
    var exp = new _core2.default.QueryCacheNS.WithCacheExpression(this.expression, constExp);
    return _core.Container.createQueryable(this, exp);
});

_core2.default.Queryable.addMember('noCache', function () {
    return this.withCache(false);
});
module.exports = exports['default'];

},{"jaydata/core":!!globalThis.global ? "../jaydata" : "jaydata/core"}]},{},[1])(1)
});

