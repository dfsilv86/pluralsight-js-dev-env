(function () {
    'use strict';

    window.globalization = {
        getText: function (key, markTextNotFound) {

            if (angular.isUndefined(key) || null === key) {
                throw new "globalization.getText(): undefined or null key.";
            }

            var result = null;

            markTextNotFound = markTextNotFound === undefined ? true : markTextNotFound;

            $.each(globalization.texts, function (index, value) {
                if (result === null && index.toLowerCase() == key.toString().toLowerCase()) {
                    result = value;
                    return false;
                }
            });

            if (result === null && markTextNotFound) {
                result = '[TEXT NOT FOUND] ' + key;
            }

            return result;
        },
        getKey: function (text) {
            var result = null;

            $.each(globalization.texts, function (index, value) {
                if (result === null && value.toLowerCase().decode() == text.toLowerCase()) {
                    result = index;
                    return false;
                }
            });

            return result;
        },
        join: function (valuesArray) {

            var result = valuesArray.join(', ');

            if (valuesArray.length > 1) {
                result = result.substring(0, result.lastIndexOf(', ')) + ' ' + globalization.texts.and + ' ' + result.substring(result.lastIndexOf(', ') + 2);
            }

            return result;
        }
    };

    angular.module('common').config(['$translateProvider', function ($translateProvider) {
        $translateProvider.translations('pt-br', window.globalization.texts);

        $translateProvider.preferredLanguage('pt-br');
    }]);
})();
