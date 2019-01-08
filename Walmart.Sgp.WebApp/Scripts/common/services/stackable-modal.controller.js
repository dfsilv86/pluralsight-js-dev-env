(function () {
    'use strict';

    angular
        .module('common')
        .controller('StackableModalController', StackableModalController);

    StackableModalController.$inject = ['$scope', '$q', '$log', '$templateRequest', 'initialOptions', '$uibModalInstance', '$rootScope', '$compile', '$controller', 'closed', 'opened'];

    function StackableModalController($scope, $q, $log, $templateRequest, initialOptions, $uibModalInstance, $rootScope, $compile, $controller, closed, opened) {
        var lastOpenedController = [];

        $scope.$on('$destroy', function () {
            $log.debug('Stackable modal controller scope destroyed.');
        });

        function getTemplate(template, templateUrl) {
            var deferred = $q.defer();
            if (template) {
                deferred.resolve(template);
            } else if (templateUrl) {
                $templateRequest(templateUrl, true)
                  .then(function (template) {
                      deferred.resolve(template);
                  }, function (error) {
                      deferred.reject(error);
                  });
            } else {
                deferred.reject("No template or templateUrl has been specified.");
            }
            return deferred.promise;
        }

        $scope.features = [];

        $scope.push = push;
        $scope.pop = pop;
        $scope.dismiss = dismiss;

        $scope.push(initialOptions);

        function push(options, theDeferred) {
            if (lastOpenedController[lastOpenedController.length - 1] == options.controller) {
                return;
            }
            
            //lastOpenedController = options.controller;
            lastOpenedController.push(options.controller);

            getTemplate(options.template, options.templateUrl)
                .then(function (template) {

                    var theScope = $rootScope.$new();

                    theScope.pop = function (result) {
                        $scope.pop(result);
                    };

                    theScope.$on('$destroy', function () {
                        $log.debug('Stackable modal child scope destroyed.');
                    });

                    var context = {
                        $scope: theScope,
                        $uibModalInstance: {
                            dismiss: function () {
                                $scope.pop();
                            },
                            close: function (result) {
                                $scope.pop(result);
                            }
                        }
                    };

                    angular.extend(context, options.resolve);

                    var linkFn = $compile(template);

                    var theController = $controller(options.controller, context);

                    var theElement = linkFn(theScope);

                    $('.stackable-container').append(theElement);

                    if ($scope.features.length > 0) {
                        var previous = $scope.features.pop();
                        previous.$element.hide();
                        $scope.features.push(previous);
                    }

                    $scope.features.push({
                        $cleanup: function () {
                            theScope.$destroy();
                            context.$scope = null;
                            context.$uibModalInstance = null;
                            context = null;
                            theScope = null;
                            linkFn = null;
                            theElement = null;
                            theController = null;
                        },
                        $element: theElement,
                        $promise: theDeferred
                    });

                    opened({
                        push: function (options, theDeferred) {
                            $scope.push(options, theDeferred);
                        },
                        pop: function (result) {
                            $scope.pop(result);
                        },
                        getCurrentStackSize: function () {
                            return $scope.features.length;
                        },
                        dismissed: function () {
                            $scope.dismiss();
                        }
                    }, context.$scope);
                }, function (reason) {
                    opened({
                        push: function (options, theDeferred) {
                            $scope.push(options, theDeferred);
                        },
                        pop: function (result) {
                            $scope.pop(result);
                        },
                        getCurrentStackSize: function () {
                            return $scope.features.length;
                        },
                        dismissed: function () {
                            $scope.dismiss();
                        }
                    }, null);
                });
        }

        function pop(result) {

            var theEnd = $scope.features.pop();

            theEnd.$element.detach();

            if ($scope.features.length === 0) {
                $uibModalInstance.close(result);
                closed(); // TODO: remover aqui?
            } else {
                theEnd.$promise.resolve(result);

                var previous = $scope.features.pop();

                previous.$element.show();

                $scope.features.push(previous);
            }

            theEnd.$cleanup();
            theEnd.$promise = null;
            theEnd.$element = null;

            lastOpenedController.pop();
        }

        function dismiss() {
            var stackSize = $scope.features.length;
            for (var i = 0; i < stackSize; i++) {
                pop();
            }
        }
    }

})();