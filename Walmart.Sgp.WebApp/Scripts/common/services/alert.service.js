(function () {
    'use strict';

    angular
        .module('common')
        .service('AlertService', AlertService);

    AlertService.$inject = ['$timeout', '$q'];
    function AlertService($timeout, $q) {
        var self = this;
        var timeout = null;
        var errors = [];
        var warnings = [];
        var infos = [];
        var successes = [];

        self.error = error;
        self.warning = warning;
        self.info = info;
        self.success = success;

        function error(message) {
            errors.push(message);
            prepareAlert();
        }

        function warning(message) {
            warnings.push(message);
            prepareAlert();
        }

        function info(message) {
            infos.push(message);
            prepareAlert();
        }

        function success(message) {
            successes.push(message);
            prepareAlert();
        }

        function prepareAlert() {
            if (null !== timeout) {
                $timeout.cancel(timeout); // suppress-validator
            }
            timeout = $timeout(doIt, 500); // suppress-validator
        }

        function doIt() {

            var jerros = 'Erros:\n\n- ' + errors.join('\n- ');
            var jalertas = 'Alertas:\n\n- ' + warnings.join('\n- ');
            var jinfos = 'Informativos:\n\n- ' + infos.join('\n- ');
            var jsuccesses = 'Sucessos:\n\n- ' + successes.join('\n- ');

            var result = [];
            if (errors.length) result.push(jerros);
            if (warnings.length) result.push(jalertas);
            if (infos.length) result.push(jinfos);
            if (successes.length) result.push(jsuccesses);

            errors = [];
            warnings = [];
            infos = [];
            successes = [];

            alert(result.join('\n\n\n'));
        }
    }
})();