(function () {
    'use strict';

    angular
        .module('common')
        .provider('$httpCustomParamSerializer', $httpCustomParamSerializerProvider);

    function serializeDate(v) {
        return moment(v).format('YYYY-MM-DD HH:mm');
    }

    function $httpCustomParamSerializerProvider() {
        /**
         * @ngdoc service
         * @name $httpParamSerializer
         * @description
         *
         * Default {@link $http `$http`} params serializer that converts objects to strings
         * according to the following rules:
         *
         * * `{'foo': 'bar'}` results in `foo=bar`
         * * `{'foo': Date.now()}` results in `foo=2015-04-01T09%3A50%3A49.262Z` (`toISOString()` and encoded representation of a Date object)
         * * `{'foo': ['bar', 'baz']}` results in `foo=bar&foo=baz` (repeated key for each array element)
         * * `{'foo': {'bar':'baz'}}` results in `foo=%7B%22bar%22%3A%22baz%22%7D"` (stringified and encoded representation of an object)
         *
         * Note that serializer will sort the request parameters alphabetically.
         * */

        this.$get = function () {
            return function ngCustomParamSerializer(params) {
                if (!params) return '';
                var parts = [];
                forEachSorted(params, function (value, key) {
                    if (value === null || isUndefined(value)) return;
                    if (isArray(value)) {
                        forEach(value, function (v, k) {
                            parts.push(encodeUriQuery(key) + '=' + encodeUriQuery(serializeValue(v)));
                        });
                    } else {
                        parts.push(encodeUriQuery(key) + '=' + encodeUriQuery(serializeValue(value)));
                    }
                });

                return parts.join('&');
            };
        };
    }

    function isDate(value) {
        return toString.call(value) === '[object Date]';
    }

    function sortedKeys(obj) {
        return Object.keys(obj).sort();
    }

    function forEachSorted(obj, iterator, context) {
        var keys = sortedKeys(obj);
        for (var i = 0; i < keys.length; i++) {
            iterator.call(context, obj[keys[i]], keys[i]);
        }
        return keys;
    }

    function encodeUriQuery(val, pctEncodeSpaces) {
        return encodeURIComponent(val).
                   replace(/%40/gi, '@').
                   replace(/%3A/gi, ':').
                   replace(/%24/g, '$').
                   replace(/%2C/gi, ',').
                   replace(/%3B/gi, ';').
                   replace(/%20/g, (pctEncodeSpaces ? '%20' : '+'));
    }

    function serializeValue(v) {
        if (isObject(v)) {
            return isDate(v) ? serializeDate(v) : toJson(v);
        }
        return v;
    }

    function isUndefined(value) { return typeof value === 'undefined'; }

    var isArray = Array.isArray;

    function isObject(value) {
        // http://jsperf.com/isobject4
        return value !== null && typeof value === 'object';
    }
})();