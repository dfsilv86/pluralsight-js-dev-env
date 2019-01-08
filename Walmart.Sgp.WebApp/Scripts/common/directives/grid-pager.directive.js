(function () {
    'use strict';

    // <grid-pager ng-model="data.values" grid-name="nomeDaGrid?" page-method="search"></grid-pager>
    // onde data.values é array retornado pelo serviço que suporta paginação, 
    // e search é um método de pesquisa que aceita como primeiro parâmetro o número da página desejada
    // grid-name é opcional (usa o nome em ng-model caso não informado)

    angular
        .module('common')
        .directive('gridPager', GridPager);

    GridPager.$inject = ['$state', 'StorageService', 'UserSessionService', '$log']
    function GridPager($state, storageService, userSessionService, $log) {
        return {
            restrict: 'E',
            require: ['ngModel'],
            replace: true,
            templateUrl: 'Scripts/common/directives/grid-pager.template.html',
            scope: {
                ngModel: '=',
                pageMethod: '&',
                settings: '=?'
            },
            controller: ['$scope', '$timeout', 'StorageService', GridPagerController],
            link: function ($scope, elem, attrs) {

                $scope.readSettings = function () {

                    var storedLimit = userSessionService.readUserPreference('grid-pager-settings', ($state.current.name.toString() + '__' + (attrs.gridName || attrs.ngModel)).replace(/\./gi, '_'), 'limit', $scope.settings.limit);

                    if (typeof (storedLimit) === 'string') {
                        storedLimit = parseInt(storedLimit, 10);
                    }

                    if (storedLimit > $scope.limitOptions[$scope.limitOptions.length - 1]) {
                        storedLimit = $scope.limitOptions[$scope.limitOptions.length - 1];
                        $scope.settings.limit = storedLimit;
                        $scope.saveSettings();
                    }

                    if (storedLimit < $scope.limitOptions[0]) {
                        storedLimit = $scope.limitOptions[0];
                        $scope.settings.limit = storedLimit;
                        $scope.saveSettings();
                    }

                    $scope.settings.limit = storedLimit;
                };

                $scope.saveSettings = function () {

                    userSessionService.saveUserPreference('grid-pager-settings', ($state.current.name.toString() + '__' + (attrs.gridName || attrs.ngModel)).replace(/\./gi, '_'), 'limit', $scope.settings.limit);
                };

                $scope.readSettings();

                $scope.$watch('ngModel', function (oldVal, newVal) {
                    if (oldVal !== newVal) {
                        $scope.$emit('grid-adjust-columns');
                    }

                    var disablePrev = $scope.disablePrev = (!$scope.ngModel || $scope.isCurrentPage(1) || $scope.hasNoPagingData());
                    var disableNext = $scope.disableNext = (!$scope.ngModel || $scope.isCurrentPage($scope.ngModel.lastPageNumber || 1) || $scope.hasNoPagingData());

                    elem.find('.first,.prev').toggleClass('disabled', disablePrev);
                    elem.find('.last,.next').toggleClass('disabled', disableNext);
                });
            }
        };
    }

    GridPagerController.$inject = ['$scope', '$timeout', 'StorageService'];

    function GridPagerController($scope, $timeout, storageService) {

        // Mais do que 100 na grid provavelmente vai deixar a tela lenta.
        // Vide sugestão de pedido.
        $scope.limitOptions = [5, 10, 15, 20, 25, 40, 50, 75, 100]; // 40 está aqui por causa da sugestao de pedido; 75 e 100 deveria ser removido pois deixam o IE muito lento em algumas telas (vide correção de nota fiscal), porem pediram para adicionar novamente

        $scope.showSettings = function () {

            if (!$scope.ngModel || $scope.hasNoPagingData()) {
                return;
            }

            $scope.temp = $scope.settings.limit;

            $scope.showPagerSettingsDialog = true;
        };

        $scope.hideSettings = function () {
            $scope.showPagerSettingsDialog = false;
        };

        $scope.firstRegisterIndex = function () {
            return $scope.ngModel && $scope.ngModel.offset ? $scope.ngModel.offset : 0;
        };
        
        $scope.firstRegisterHumanIndex = function () {
            return $scope.firstRegisterIndex() + 1;
        };

        $scope.lastRegisterHumanIndex = function () {
            return $scope.firstRegisterIndex() + ($scope.ngModel && $scope.ngModel.length ? $scope.ngModel.length : 0);
        };

        $scope.page = function (number) {
            if ($scope.ngModel.currentPageNumber != number && 1 <= number && number <= $scope.ngModel.lastPageNumber) {
                $scope.pageMethod()(number);
            }
        };

        $scope.firstPage = function () {
            if (!$scope.disablePrev) $scope.page(1);
        };

        $scope.lastPage = function () {
            if (!$scope.disableNext) $scope.page($scope.ngModel.lastPageNumber);
        };

        $scope.previousPage = function () {
            if (!$scope.disablePrev) $scope.page($scope.ngModel.currentPageNumber - 1);
        };

        $scope.nextPage = function () {
            if (!$scope.disableNext) $scope.page($scope.ngModel.currentPageNumber + 1);
        };

        $scope.isCurrentPage = function (number) {
            return $scope.ngModel.currentPageNumber === number;
        };

        $scope.hasNoPagingData = function () {
            return angular.isUndefined($scope.ngModel.offset) && angular.isUndefined($scope.ngModel.limit) && angular.isUndefined($scope.ngModel.orderBy);
        };

        $scope.setLimit = function (newLimit) {

            $scope.settings.limit = newLimit;

            // Aguarda um ciclo para esperar a alteração propagar
            // TODO: talvez possa ser usado um  $apply aqui, testar
            $timeout(function () { // suppress-validator

                var promise = $scope.pageMethod()(1);

                // Se o método de pesquisa retornar uma promise, aproveitamos para persistir a configuração
                // conforme o que o servidor validar e acatar.
                if (!!promise && angular.isFunction(promise.then)) {

                    promise.then(function (data) {

                        if (!!data && !!data.limit) {
                            $scope.settings.limit = data.limit;
                            $scope.saveSettings();
                        }
                    });
                }
                else {
                    // Caso contrário, salvamos o que o usuário selecionou.
                    $scope.saveSettings();
                }
            });
        };
    }
})();