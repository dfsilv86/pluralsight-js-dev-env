// From https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/keys
if (!Object.keys) {
    Object.keys = (function () {
        'use strict';
        var hasOwnProperty = Object.prototype.hasOwnProperty,
            hasDontEnumBug = !({ toString: null }).propertyIsEnumerable('toString'),
            dontEnums = [
              'toString',
              'toLocaleString',
              'valueOf',
              'hasOwnProperty',
              'isPrototypeOf',
              'propertyIsEnumerable',
              'constructor'
            ],
            dontEnumsLength = dontEnums.length;

        return function (obj) {
            if (typeof obj !== 'object' && (typeof obj !== 'function' || obj === null)) {
                throw new TypeError('Object.keys called on non-object');
            }

            var result = [], prop, i;

            for (prop in obj) {
                if (hasOwnProperty.call(obj, prop)) {
                    result.push(prop);
                }
            }

            if (hasDontEnumBug) {
                for (i = 0; i < dontEnumsLength; i++) {
                    if (hasOwnProperty.call(obj, dontEnums[i])) {
                        result.push(dontEnums[i]);
                    }
                }
            }
            return result;
        };
    }());
}

Array.prototype.equals = function (other) {
    if (angular.isArray(other) && other.length == this.length) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] != other[i]) {
                return false;
            }
        }
        return true;
    }
    return false;
};

if (!String.prototype.endsWith) {
    String.prototype.endsWith = function (searchString, position) {
        var subjectString = this.toString();
        if (typeof position !== 'number' || !isFinite(position) || Math.floor(position) !== position || position > subjectString.length) {
            position = subjectString.length;
        }
        position -= searchString.length;
        var lastIndex = subjectString.indexOf(searchString, position);
        return lastIndex !== -1 && lastIndex === position;
    };
}

if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (searchString, position) {
        position = position || 0;
        return this.substr(position, searchString.length) === searchString;
    };
}

window.zeroIsValid = function (value) {
    if (!!value || value === 0 || value === '0') {
        return value;
    }
    return '';
};

if (!String.prototype.capitalize) {
    String.prototype.capitalize = function () {
        return this.substr(0, 1).toUpperCase() + this.substr(1);
    };
}

if (!Math.sign) {
    Math.sign = Math.sign || function (x) {
        x = +x; // convert to a number
        if (x === 0 || isNaN(x)) {
            return x;
        }
        return x > 0 ? 1 : -1;
    };
}

// JSON.parse() que devolve propriedades de data como objeto Date() ao invés de string no formato ISO
// originalmente em http://stackoverflow.com/a/32542278
// modificado para usar o moment.js
(function () {
    if (JSON && !JSON.parseWithDate) {
        JSON.parseWithoutDate = JSON.parse; //Store the original JSON.parse function

        var reISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/;
        var reMsAjax = /^\/Date\((d|-|.*)\)[\/|\\]$/;

        JSON.parseWithDate = function (json) {
            /// <summary>  
            /// parses a JSON string and turns ISO or MSAJAX date strings  
            /// into native JS date objects  
            /// </summary>      
            /// <param name="json" type="var">json with dates to parse</param>          
            /// </param>  
            /// <returns type="value, array or object" />  
            try {
                var res = JSON.parseWithoutDate(json,
                function (key, value) {
                    if (typeof value === 'string') {
                        var a = reISO.exec(value);
                        if (a)
                            return moment(value).toDate();
                        a = reMsAjax.exec(value);
                        if (a) {
                            var b = a[1].split(/[-+,.]/);
                            return new Date(b[0] ? +b[0] : 0 - +b[1]);
                        }
                    }
                    return value;
                });
                return res;
            } catch (e) {
                // orignal error thrown has no error message so rethrow with message  
                throw new Error("JSON content could not be parsed");
            }
        };

        //Make Date parsing the default
        JSON.parse = JSON.parseWithDate;
    }

    var _toJson = Date.prototype.toJSON;

    Date.prototype.toJSON = function (key) {
        //return this.getFullYear().toString() + '-' + (this.getMonth() + 1).toString() + '-' + this.getDay().toString() + ' ' + this.getHours().toString() + ':' + this.getMinutes().toString() + ':' + this.getSeconds().toString();
        return moment(this).format('YYYY-MM-DD HH:mm:ss');
    };
})();
