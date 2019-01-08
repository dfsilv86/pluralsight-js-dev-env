(function () {
    'use strict';

    if (!String.prototype.capitalize) {
        String.prototype.capitalize = function () {
            return this[0].toUpperCase() + this.slice(1);
        };
    }

    if (!String.prototype.toCamelCase) {
        String.prototype.toCamelCase = function () {
            return this.charAt(0).toLowerCase() + this.slice(1);
        };
    }

    if (!String.prototype.format) {
        String.prototype.format = function () {
            var args = arguments;
            return this.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] !== 'undefined' ? args[number] : match;
            });
        };
    }

    if (!String.prototype.contains) {
        String.prototype.contains = function (substring) {            
            return typeof(substring) === 'string' && this.toLowerCase().indexOf(substring.toLowerCase()) > -1;
        };
    }


    if (!String.prototype.removeAccents) {
        String.prototype.removeAccents = function () {
            var accents = 'ÀÁÂÃÄÅàáâãäåÒÓÔÕÕÖØòóôõöøÈÉÊËèéêëðÇçÐÌÍÎÏìíîïÙÚÛÜùúûüÑñŠšŸÿýŽž';
            var accentsOut = "AAAAAAaaaaaaOOOOOOOooooooEEEEeeeeeCcDIIIIiiiiUUUUuuuuNnSsYyyZz";
            var str = this.split('');
            var strLen = str.length;
            var i, x;
            for (i = 0; i < strLen; i++) {
                if ((x = accents.indexOf(str[i])) != -1) {
                    str[i] = accentsOut[x];
                }
            }
            return str.join('');
        };
    }   

    if (!String.prototype.decode) {
        String.prototype.decode = function () {
            return $("<div/>").html(this).text();
        };
    }


    if (!String.prototype.isEmptyDate) {
        String.prototype.isEmptyDate = function () {
            return this.indexOf('1900-01-01') > -1;
        };
    }

    if (!String.prototype.removeSpaces) {
        String.prototype.removeSpaces = function () {
            return this.replace(/\s/g, '');
        };
    }

    if (!String.prototype.removeDots) {
        String.prototype.removeDots = function () {
            return this.replace(/\./g, '');
        };
    }

    if (!String.prototype.getDiff) {
        String.prototype.getDiff = function (b) {
            var i = 0;
            var j = 0;
            var result = "";
            var a = this;

            while (j < b.length) {
                if (a[i] != b[j] || i == a.length)
                    result += b[j];
                else
                    i++;
                j++;
            }

            return result;
        };
    }

})();
