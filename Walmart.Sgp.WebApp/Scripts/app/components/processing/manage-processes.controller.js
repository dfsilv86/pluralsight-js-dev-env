(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ManageProcessesController', ManageProcessesController);

    ManageProcessesController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'ProcessingService', 'currentUser'];

    function ManageProcessesController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, processingService, currentUser) {

        $validation.prepare($scope);

        $scope.filters = { createdUserId: currentUser.id, processName: '', state: null };
        $scope.temp = { userName: currentUser.userName, user: currentUser, isAdmin: currentUser.role.isAdmin || false };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: 'ModifiedDate DESC' };

        $scope.search = search;
        $scope.clear = clear;
        $scope.detail = detail;

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var createdUserId = $scope.filters.createdUserId;
            var processName = $scope.filters.processName;
            var state = $scope.filters.state;

            var deferred = $q
                .when(processingService.findAllByUser(createdUserId, processName, state, $scope.data.paging))
                .then(applyValue);
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function clear() {
            $scope.filters = { createdUserId: currentUser.id, processName: '', state: null };
            $scope.temp = { userName: currentUser.userName, user: currentUser, isAdmin: currentUser.role.isAdmin || false };
            $scope.data.values = [];
            $scope.data.paging = { offset: 0, limit: 10, orderBy: 'ModifiedDate DESC' };
        }

        function detail(item) {
            return processingService.viewDetails(item);
        }

        $scope.$on('processing-notify', function () { search(); });

        search(1);
    }
})();