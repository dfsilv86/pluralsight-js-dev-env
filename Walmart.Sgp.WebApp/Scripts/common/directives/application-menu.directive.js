(function () {
    'use strict';

    angular
        .module('common')
        .directive('applicationMenu', ApplicationMenu);

    ApplicationMenu.$inject = ['UserSessionService', 'ToastService', '$state', '$window', '$timeout'];

    function ApplicationMenu(userSession, toast, $state, $window, $timeout) {
        return {
            restrict: 'E',
            templateUrl: 'Scripts/common/directives/application-menu.template.html',
            link: function ($scope, elem) {              
                var userMenus = userSession.getCurrentUser().menus;
               
                var filterMenuItem = function (menuItem) {
                    // Tem elementos filhos?
                    if (Array.isArray(menuItem.children)) {

                        // Então filtra 
                        menuItem.availableChildren = menuItem.children.filter(filterMenuItem);

                        return menuItem.availableChildren !== null && menuItem.availableChildren.length > 0;
                    } else if (menuItem.route) {
                        // Se o item possuir uma action especifica, verifica se o usuário tem permissão para ela
                        return userMenus.containsByProperty("route", menuItem.route);
                    }

                    return false;
                };
                
                // Realiza o slice(0) para clonar o array e assim evitar que seja alterado o original,
                // pois existe a possibilidade do usuário realizar logout e login novamente com outro papel ou outro username
                // nessa situação o sgpMenus precisa estar no estado original.
                var allMenusClone = window.sgpMenus.slice(0);
                $scope.menus = allMenusClone.filter(filterMenuItem);
                $scope.openMenu = function () {
                    toast.dismissAll();
                };

                var resizeTimeout = null;

                $($window).on('resize.applicationMenu', function () {
                    if (!resizeTimeout) {
                        resizeTimeout = $timeout(checkMenus, 100, false); // suppress-validator
                    }
                });

                function checkMenus() {
                    resizeTimeout = null;
                    var $ul = elem.find('ul');
                    if ($ul.length > 0) {
                        var availableWidth = $ul.css('width').replace('px', '') * 1;
                        var firstItem = elem.find('ul > li:first-child');
                        if (firstItem.length > 0) {
                            var menuTop = firstItem.offset().top;
                            elem.find('ul > li').show().filter(function (index, li) {
                                var $li = $(li);
                                var liOffset = $li.offset();
                                if (liOffset.top > menuTop) return true;
                                var liRight = (($li.css('width').replace('px', '') * 1) + liOffset.left);
                                return liRight > availableWidth;
                            }).hide();
                        }
                    }
                }

                $scope.$on('$destroy', function () {
                    if (!!resizeTimeout) $timeout.cancel(resizeTimeout); // suppress-validator
                    $($window).off('resize.applicationMenu');
                });

                $timeout(checkMenus, 20, false); // suppress-validator
            }
        };
    }
})();