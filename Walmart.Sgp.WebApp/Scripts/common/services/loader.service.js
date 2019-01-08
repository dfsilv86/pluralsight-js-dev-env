(function () {
    'use strict';

    angular
        .module('common')
        .service('LoaderService', LoaderService);

    LoaderService.$inject = ['$timeout', '$q', 'loaderConfig'];
    function LoaderService($timeout, $q, loaderConfig) {
        var self = this;
        self._counter = 0;
        self._elements = [];
        self._timeout = null;
        self.register = function (elem) {
            var exists = false;
            for (var i = 0; i < self._elements.length && !exists; i++) {
                exists = self._elements[i] === elem;
            }
            if (!exists) self._elements.push(elem);
        };
        self.unregister = function (elem) {
            var index = -1;
            for (var i = 0; i < self._elements.length && index === -1; i++) {
                if (self._elements[i] === elem) {
                    index = i;
                }
            }
            if (index !== -1) {
                self._elements.splice(index, 1);
            }
        };
        self.push = function (url) {
            if (self._counter === 0) {
                for (var i = 0; i < self._elements.length; i++) {
                    self._elements[i].addClass('in-use');
                }
                self._timeout = $timeout(function () { self.late(); }, angular.isDefined(loaderConfig.isLateTimeoutInMs) ? loaderConfig.isLateTimeoutInMs : 250); // suppress-validator
            }
            self._counter++;
        };
        self.pop = function (url) {
            self._counter--;
            if (self._counter === 0) {
                for (var i = 0; i < self._elements.length; i++) {
                    self._elements[i].removeClass('in-use').removeClass('is-late');
                }
                if (self._timeout !== null) $timeout.cancel(self._timeout); // suppress-validator
                self._timeout = null;
            }
        };
        self.late = function () {
            for (var i = 0; i < self._elements.length; i++) {
                self._elements[i].addClass('is-late');
            }
        };
    }
})();