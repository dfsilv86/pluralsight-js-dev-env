(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ViewProcessLogsController', ViewProcessLogsController);

    ViewProcessLogsController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'ProcessingService', 'StackableModalService', 'processOrder'];

    function ViewProcessLogsController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, processingService, stackableModalService, processOrder) {

        $validation.prepare($scope);

        $scope.data = processOrder;
        $scope.data.values = [];
        $scope.data.paging = { offset: 0, limit: 10, orderBy: 'DhAuditStamp DESC' };

        function applyValue(data) {

            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        $scope.searchLogs = function searchLogs(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            $q
                .when(processingService.getTicketLogs(processOrder.ticket, $scope.data.paging))
                .then(applyValue);
        };

        $scope.refresh = function refresh() {
            $scope.searchLogs();
        };

        $scope.$on('processing-update', function (event, data) {
            for (var i = 0; i < data.trackedProcesses.length; i++) {
                var po = data.trackedProcesses[i];
                if (po.ticket == processOrder.ticket && (po.currentProgressPercentage != processOrder.currentProgressPercentage || po.state != processOrder.state || po.message != processOrder.message)) {
                    $scope.refresh();
                    break;
                }
            }
        });

        $scope.searchLogs(1);
    }
})();