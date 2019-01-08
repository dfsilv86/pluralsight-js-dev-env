(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ManageProcessController', ManageProcessController);

    ManageProcessController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'ProcessingService', 'StackableModalService', '$filter', '$injector', 'SistemaService', 'processOrder', 'ToastService'];

    function ManageProcessController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, processingService, stackableModalService, $filter, $injector, sistemaService, processOrder, toastService) {

        $validation.prepare($scope);

        function apply(order) {

            var paoff = $scope.data.pagedArguments.offset;
            var palim = $scope.data.pagedArguments.limit;
            var paord = $scope.data.pagedArguments.orderBy;

            $scope.data = order;
            $scope.data.pagedArguments = [];
            $scope.data.pagedArguments.offset = paoff;
            $scope.data.pagedArguments.limit = palim;
            $scope.data.pagedArguments.orderBy = paord;
            $scope.data.logs = [];
            $scope.searchArguments();
        }

        function convert(order) {
            var promises = [];
            for (var i = 0; i < order.arguments.length; i++) {
                promises.push((function () {
                    var argument = order.arguments[i];
                    var argumentName = argument.argumentName = $filter('capitalize')($filter('argumentName')(argument.name), true, false);

                    if (argumentName.toLowerCase().startsWith('id')) {
                        var theName = $filter('capitalize')(argumentName.replace(/id/gi, ''), true, true);
                        argument.argumentName = !!globalization.texts['id' + theName] ? 'id' + theName : (!!globalization.texts['Id' + theName] ? 'id' + theName : 'iD' + theName);
                        if (angular.isUndefined(argument.value) || null === argument.value) {
                            return argument.argumentValue = null;
                        }
                        var svcName = theName + 'Service';
                        var svcImpl = $injector.get(svcName);
                        var getByIdImpl = svcImpl['findById'] || svcImpl['getById'] || svcImpl['obterEstruturadoPorId'] || svcImpl['obterPorId'] || svcImpl['obter'];

                        if (null != getByIdImpl) {
                            var result = getByIdImpl.call(svcImpl, argument.value).then(function (value) {
                                argument.argumentValue = value['name'] || value['nome'] || value['description'] || value['descricao'] || value['ds' + theName] || value['nm' + theName] || argument.value;
                                return value;
                            });
                            return result;
                        }
                    } else if (argumentName.toLowerCase() == 'cdsistema') {
                        argument.argumentName = 'marketingStructure';
                        return sistemaService.obterPorUsuario(userSession.getCurrentUser().id).then(function (lista) {
                            for (var j = 0; j < lista.length; j++) {
                                if (lista[j].cdSistema == argument.value) {
                                    argument.argumentValue = lista[j].text;
                                    break;
                                }
                            }
                            return lista;
                        });
                    } else if (!!sgpFixedValues[argumentName]) {
                        return argument.argumentValue = sgpFixedValues.getByValue(argumentName, argument.value).description;
                    } else {
                        return argument.argumentValue = $filter('fileName')(argument.value);
                    }
                })());
            }

            return $q.all(promises).then(function () { return order; });
        }

        function loadArguments(order) {
            if (null !== order) {
                return convert(order).then(apply);
            } else {
                toastService.warning(globalization.texts.couldNotFindProcessOrder.format(processOrder.ticket))
            }
        }

        $scope.data = processOrder;
        $scope.data.pagedArguments = [];
        $scope.data.pagedArguments.offset = 0;
        $scope.data.pagedArguments.limit = 10;
        $scope.data.pagedArguments.orderBy = 'Name ASC';
        $scope.data.logs = [];

        convert(processOrder).then(apply);

        $scope.searchArguments = function searchArguments(pageNumber) {
            pagingService.inMemoryPaging(pageNumber, processOrder.arguments, $scope.data.pagedArguments);
        };

        $scope.viewLogs = function viewLogs() {
            processingService.viewLogs(processOrder);
        };

        $scope.viewResults = function viewResults() {
            processingService.viewResults(processOrder);
        };

        $scope.refresh = function refresh() {
            processingService.getTicketDetails(processOrder.ticket).then(loadArguments);
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

        $scope.searchArguments(1);
    }
})();