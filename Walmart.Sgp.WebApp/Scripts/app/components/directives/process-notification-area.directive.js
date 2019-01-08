(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('processNotificationArea', ProcessNotificationAreaDirective);

    function ProcessNotificationAreaDirective() {
        return {
            restrict: 'E',
            templateUrl: 'Scripts/app/components/directives/process-notification-area.view.html',
            scope: {},
            controller: ['$scope', '$timeout', 'ProcessingService', ProcessNotificationAreaController]
        };
    }

    function ProcessNotificationAreaController($scope, $timeout, processingService) {

        $scope.isVisible = false;
        $scope.isOpen = false;
        $scope.isPresentingNotification = false;
        $scope.presentingTimeout = null;

        $scope.data = {
            processOrders: processingService.getTrackedProcesses()
        };

        $scope.$on('processing-notify', function (event, processes) {
            $scope.showNotification();
        });

        $scope.$on('processing-update', function (event, data) {
            $scope.data.lastCheck = data.lastCheck || $scope.data.lastCheck;
            $scope.data.processOrders = data.trackedProcesses.filter(function (obj) {
                return $scope.data.lastCheck === undefined || $scope.data.lastCheck === null || obj.modifiedDate > $scope.data.lastCheck;
            });
        });

        $scope.$on('session-start', function () { $scope.isVisible = $scope.data.processOrders.length > 0; });
        $scope.$on('session-end', function () {
            $scope.cancelPresentingTimeout();
            $scope.isPresentingNotification = false;
            $scope.isVisible = false;
        });

        $scope.showNotification = function () {

            if ($scope.isOpen) return;

            $scope.isPresentingNotification = true;

            $scope.presentingTimeout = $timeout(function () { // suppress-validator
                $scope.isPresentingNotification = false;
                $scope.presentingTimeout = null;

                $scope.isVisible = $scope.data.processOrders.length > 0;

            }, 5000);
        };

        $scope.cancelPresentingTimeout = function () {
            if (null !== $scope.presentingTimeout) {
                $timeout.cancel($scope.presentingTimeout); // suppress-validator
                $scope.presentingTimeout = null;
            }
        };

        $scope.toggleOpen = function () {
            $scope.isOpen = !$scope.isOpen;

            if ($scope.isOpen) {
                $scope.isVisible = true;
                $scope.isPresentingNotification = false;
                $scope.cancelPresentingTimeout();
            } else {
                if ($scope.data.processOrders.length == 0) {
                    $scope.isVisible = false;
                }
            }
        };

        $scope.viewManager = function () {
            processingService.viewManager();
        }

        $scope.viewDetails = function (order) {
            processingService.viewDetails(order);
        }

        $scope.viewResults = function (order) {
            processingService.viewResults(order);
        }
    }
})();