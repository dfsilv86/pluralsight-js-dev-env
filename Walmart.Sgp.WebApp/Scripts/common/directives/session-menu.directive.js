(function () {
    'use strict';

    angular
        .module('common')
        .directive('sessionMenu', SessionMenu);

    SessionMenu.$inject = ['UserSessionService', 'WebApiService', '$state', '$interval', '$timeout' ];

    function SessionMenu(userSession, webApi, $state, $interval, $timeout) {
        return {
            restrict: 'E',
            templateUrl: 'Scripts/common/directives/session-menu.template.html',
            link: function ($scope, elem) {
                $scope.user = userSession.getCurrentUser();
                $scope.webApi = {
                    version: webApi.getLocalVersion()
                };

                $scope.lastRequestDate = moment();
                $interval(function() {
                    $scope.lastRequestDate = moment();
                }, 60000);

                webApi.getServerName()
                    .then(function (serverName) {
                        var shortName = serverName.substring(serverName.length - 3);
                        $scope.serverName = shortName;
                });

                $scope.logout = function () {
                    userSession.end();
                };

                $scope.lockScreen = function () {
                    userSession.lock();
                }

                $scope.alterarSenha = function () {
                    userSession.changePassword();
                };

                var changeWidth = function () {

                    var $document = $(document);

                    var lastMenu = $('ul.nav.navbar-nav:first-of-type > li:last-child');

                    if (lastMenu.length === 0) return;

                    var lastMenuRight = Math.ceil(lastMenu.offset().left) + (lastMenu.css('width').replace('px', '') * 1);

                    var docWidth = $document.width();

                    var thisMenu = elem.find('li.dropdown.user > a');

                    var availWidth = docWidth - lastMenuRight - 60;

                    if (availWidth < 100) availWidth = 100;
                    if (availWidth > 240) availWidth = 240;

                    thisMenu.css('max-width', availWidth);

                    var loginWidth = elem.find('.user-login-name').css('width').replace('px', '') * 1;

                    elem.find('.user-full-name').css('max-width', availWidth - 24 - loginWidth);
                }

                $(window).on('resize.userSessionMenu', changeWidth);

                $timeout(changeWidth, 1000); // suppress-validator

                $scope.$on('$destroy', function () {
                    $(window).off('resize.userSessionMenu', changeWidth);
                });
            }
        };
    }
})();