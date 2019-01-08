(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('securityAction', SecurityAction);

    SecurityAction.$inject = ['UserSessionService'];

    function SecurityAction(userSession) {
        return {
            restrict: 'A',
            link: function ($scope, element, attrs) {                
                var actionId = attrs.securityAction;

                if (!userSession.canAccessAction(actionId)) {
                    element.hide();
                }
            }
        };
    }
})();