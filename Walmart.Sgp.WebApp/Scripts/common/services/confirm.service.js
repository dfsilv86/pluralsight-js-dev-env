(function () {
    'use strict';

    // confirm.ask("Foo bar?").then(funcaoSim).catch(funcaoNao)
    // confirm.open({opcoes}).then(funcaoSim).catch(funcaoNao)

    angular
        .module('common')
        .service('ConfirmService', ConfirmService);

    ConfirmService.$inject = ['$q', 'ngDialog', 'ToastService'];

    function ConfirmService($q, ngDialog, toast) {

        var self = this;
        self.open = function (options) {
            toast.dismissAll();

            var deferred = $q.defer();

            var defaults = {
                yes: function () { deferred.resolve(); },
                no: function () { deferred.reject(); }
            };

            var optionsUsed = angular.extend(defaults, options);

            ngDialog.open({
                template: 'Scripts/common/services/confirm.template.html',
                controller: 'ConfirmController',
                disableAnimation: true,
                resolve: {
                    options: function () {
                        return {
                            message: options.message,
                            messageTemplateUrl: options.messageTemplateUrl,
                            messageScope: options.messageScope,
                            yesClicked: optionsUsed.yes,
                            noClicked: optionsUsed.no
                        };
                    }

                }
            });

            return deferred.promise;
        };
        self.ask = ask;

        function ask(message) {
            return self.open({ message: message });
        }
    }
})();