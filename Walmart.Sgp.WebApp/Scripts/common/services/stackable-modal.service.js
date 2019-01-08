(function () {
    'use strict';

    angular
        .module('common')
        .service('StackableModalService', StackableModalService);

    StackableModalService.$inject = ['$uibModal', '$q', 'ToastService'];

    function StackableModalService($modal, $q, toast) {

        var self = this;
        var theModal = null;
        var controllerApi = null;

        self.getCurrentStackSize = function () {

            if (null === controllerApi) {
                return 0;
            }

            return controllerApi.getCurrentStackSize();
        };

        self.popUntilStackSize = function (index) {

            var currentStackSize = self.getCurrentStackSize();

            if (index == currentStackSize - 1) return;

            for (var i = index; i < currentStackSize - 1; i++) {
                controllerApi.pop();
            }
        };

        self.abort = function () {
            var currentStackSize = self.getCurrentStackSize();
            for (var i = 0; i < currentStackSize; i++) {
                controllerApi.pop();
            }
        };

        self.open = function (options) {
            toast.dismissAll();
            return self.push(options);
        };

        self.push = function (options) {

            if (null === theModal) {

                var deferred = $q.defer();

                theModal = $modal.open({
                    templateUrl: 'Scripts/common/services/stackable-modal.template.html',
                    controller: 'StackableModalController',
                    resolve: {
                        initialOptions: options,
                        opened: function () {
                            return function (theApi, childScope) {
                                if (options.opened) options.opened(childScope);

                                controllerApi = theApi;
                            }
                        },
                        closed: function () {
                            return function () {
                                if (options.closed) options.closed();

                                controllerApi = null;
                                theModal = null;
                            }
                        }
                    },
                    size: (options.options || {}).modalDialogClass || 'default'
                });

                theModal.result.then(function (result) {
                    return deferred.resolve(result);
                }, function (reason) {
                    if (controllerApi) controllerApi.dismissed();
                    theModal = null;
                    controllerApi = null;
                    return deferred.reject(reason);
                });

                return deferred.promise;
            } else {

                var deferred = $q.defer();

                controllerApi.push(options, deferred);

                return deferred.promise;
            }
        }
    }
})();