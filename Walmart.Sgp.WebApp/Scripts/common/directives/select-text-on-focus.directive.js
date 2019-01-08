(function () {
    'use strict';

    // Solicitado pelo pessoal da sugestao de pedido
    // Bug #4746
    // 1. Quando o campo ganhar foco, selecionar todo o texto.
    //      o usuário pode modificar a seleção com o teclado (setas movem cursor para inicio ou fim do campo, digitar substitui o texto)
    //      OU clicando novamente no campo
    // 2. Quando o campo ganhou foco, tem o texto selecionado, e o usuário clica no campo, o cursor deve ir para o final do campo.
    //      o usuário pode mover o cursor com as setas, ou modificar o texto digitando
    // 3. Enquanto o foco não sair do campo, clicks subsequentes posicionam o cursor no caractere desejado.

    angular
        .module('common')
        .directive('selectTextOnFocus', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                scope: false,
                link: function ($scope, elem, attrs) {

                    var $elem = $(elem);

                    var flag1 = false, flag2 = false;

                    if ($elem[0].nodeName !== 'INPUT') return;

                    var doSelectText = function () {

                        $elem.select();
                        flag2 = false;
                        flag1 = true;
                        $timeout(function() {  // suppress-validator
                            flag1 = false;
                        }, 100);
                    };

                    var clickHandler = function () {

                        if (flag1 || flag2) {
                            return;
                        }

                        if (getSelectionText() == $elem.val()) {
                            placeAtEnd();
                        }

                        // http://stackoverflow.com/a/5379408
                        function getSelectionText() {
                            var text = "";
                            if (window.getSelection) {
                                text = window.getSelection().toString();
                            } else if (document.selection && document.selection.type != "Control") {
                                text = document.selection.createRange().text;
                            }
                            return text;
                        }

                        // http://stackoverflow.com/a/8631903
                        function placeAtEnd() {
                            var tmp = $elem.val();
                            $elem.val('');
                            $elem.val(tmp);
                            flag2 = true;
                        }
                    };

                    var blurHandler = function () {

                        flag1 = false;
                        flag2 = false;
                    };

                    $elem.on('focus.selectTextOnFocus', doSelectText);
                    $elem.on('click.selectTextOnFocus', clickHandler);
                    $elem.on('blur.selectTextOnFocus', blurHandler);

                    $scope.$on('$destroy', function () {
                        $elem.off('focus.selectTextOnFocus', doSelectText);
                        $elem.off('click.selectTextOnFocus', clickHandler);
                        $elem.off('blur.selectTextOnFocus', blurHandler);
                    });
                }
            };
        }]);
})();