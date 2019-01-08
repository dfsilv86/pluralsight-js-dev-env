(function () {
    angular
        .module('SGP')
        .run(MensagemReturnSheetRun);

    MensagemReturnSheetRun.$inject = ['$rootScope', 'SugestaoReturnSheetService', '$q', 'ngDialog'];

    function MensagemReturnSheetRun($rootScope, sugestaoReturnSheetService, $q, ngDialog) {
        $rootScope.$on('login-inicial-sucesso', onLogin);

        function onLogin() {
            $q.when(sugestaoReturnSheetService.possuiReturnsVigentesQuantidadesVazias())
                .then(function (data) {
                    if (data) {
                        mostrarMensagem();
                    }
                });
        }

        function mostrarMensagem() {
            var dialog = ngDialog.open({
                template: 'Scripts/app/alertas/confirmar-ok-view.html',
                controller: ['$scope', function ($scope) {
                    $scope.message = globalization.texts.thereAreReturnSheetItensAvailable;
                }]
            });

            dialog.closePromise.then(registrarLog);
        }

        function registrarLog() {
            sugestaoReturnSheetService.registrarLogAvisoReturnSheetsVigentes();
        }
    }
})();