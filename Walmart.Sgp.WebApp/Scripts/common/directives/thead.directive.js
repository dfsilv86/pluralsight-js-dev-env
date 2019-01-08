(function () {
    'use strict';

    // Here be dragons.

    angular
        .module('common')
        .directive('thead', FixedTableHeaderDirective);

    FixedTableHeaderDirective.$inject = ['$compile', '$timeout', '$interval'];
    function FixedTableHeaderDirective($compile, $timeout, $interval) {
        return {
            restrict: 'E',
            scope: false,
            link: function FixedTableHeaderLink($scope, elem, attrs) {

                var $thead = elem;
                var $table = $thead.parent('table');
                var tableParent = $table.parent()[0];

                if (tableParent.nodeName === 'TD' || !$table.is('.table')) {
                    return;
                }

                var intervalPromise = null, scrollPromise = null, resizePromise = null;

                var linkFn = $compile(elem.html());

                var $clone = null, $cloneParent = null;

                linkFn($scope, function (clonedElem, scope2) {

                    $clone = $(clonedElem)
                        .wrap("<div><table><thead></thead></table></div>")
                        .parent()
                        .parent()
                        .attr('class', $table.attr('class'))
                        .css('border-top', 'solid 1px white')
                        .css('border-bottom', 'solid 1px white')
                        .css('position', 'relative')
                        .css('table-layout', 'fixed');

                    $cloneParent = $clone
                        .parent()
                        .css('position', 'fixed')
                        .css('z-index', '1029')
                        .css('overflow-x', 'hidden')
                        .css('top', getTheTop());
                });

                var clearHeaderColumnsWidth = function () {
                    angular.forEach($clone.find('thead tr th'), function (clonedth, index) {
                        var $clonedth = $(clonedth);
                        $clonedth.css('width', '');
                    });
                    $clone.css('width', '');
                };

                var setHeaderColumnsWidth = function () {
                    var $clonedths = $clone.find('thead tr:first-of-type th');
                    var $theadths = $thead.find('tr:first-of-type th');

                    if ($theadths.length < 2) return;

                    for (var i = 0; i < $theadths.length; i++) {
                        var $theadth = $($theadths[i]);
                        var $clonedth = $($clonedths[i]);

                        forcarLargura($clonedth, $theadth.css('width')); //, i == $theadths.length - 1);
                    }
                    forcarLargura($clone, $table.css('width'));
                    forcarLargura($cloneParent, $table.parent().css('width'));
                };

                var pinHeader = function () {
                    // Fixa o cabeçalho no topo
                    isPinned = true;
                    resetColumnsWidth();

                    $cloneParent.insertBefore($table);

                    if (!!intervalPromise) {
                        $interval.cancel(intervalPromise);  // suppress-validator
                        intervalPromise = null;
                    }

                    intervalPromise = $interval(checkLeft, 250, 0, false);  // suppress-validator
                };

                var unpinHeader = function () {
                    // Desafixa o cabeçalho do topo
                    $cloneParent.detach();
                    clearHeaderColumnsWidth();
                    isPinned = false;

                    if (!!intervalPromise) {
                        $interval.cancel(intervalPromise);  // suppress-validator
                        intervalPromise = null;
                    }
                };

                var resetColumnsWidth = function () {
                    // Refaz tudo
                    // Primeiro limpa as larguras
                    clearHeaderColumnsWidth();
                    if (isPinned) {
                        // Se estiver pinado, então refaz as larguras
                        setHeaderColumnsWidth();
                    }

                    resizePromise = null;
                };

                // ATENÇÃO:
                // existe uma chamada no grid-pager.directive.js para $emit deste evento
                // Evento chamado sempre que os itens da grid são modificados
                // Permite à tabela se reajustar conforme o conteúdo
                $scope.$on('grid-adjust-columns', onResize);

                // Fixa o header
                $scope.$on('grid-pin-header', pinHeader);

                // Desafixa o header
                $scope.$on('grid-unpin-header', unpinHeader);

                // Inicio e fim da sessão afetam o cabeçalho/menu/migalhas da página, e consequentemente a posição onde o cabeçalho pode ser fixado
                $scope.$on('session-start', sessionChanged);
                $scope.$on('session-end', sessionChanged);

                var isPinned = false;

                var sessionChanged = function () {
                    cachedTop = null;
                    checkPinning();
                };

                var checkPinning = function () {

                    var theOffset = $table.offset();

                    if (!$table || !theOffset) {
                        // A tabela está sendo removida.
                        return;
                    }

                    // Verifica se o cabeçalho deve ser fixo ou não conforme o scroll da página
                    var thePos = theOffset.top;
                    var theSize = $table.css('height').replace('px', '') * 1;
                    var lastRowSize = $table.find('tbody:first-of-type tr:last-of-type').css('height').replace('px', '') * 1;

                    theSize = theSize - lastRowSize;

                    var theScroll = $(window).scrollTop();

                    var theTop = getTheTop();
                    thePos = thePos - theTop;

                    if (thePos < theScroll && theScroll < (thePos + theSize)) {
                        if (!isPinned) pinHeader();
                    } else {
                        if (!!isPinned) unpinHeader();
                    }

                    scrollPromise = null;
                };

                var checkLeft = function () {
                    try {
                        $clone.css('left', $table.offset().left - $cloneParent.offset().left);
                    } catch (e) {

                        if (!!intervalPromise) {
                            $interval.cancel(intervalPromise);  // suppress-validator
                            intervalPromise = null;
                        }

                        $log.error('Ocorreu um erro ao reposicionar o cabeçalho fixo em relação ao offset lateral da tabela: ' + e.toString());
                    }
                };

                // ATENÇÃO: eventos scroll podem disparar múltiplas vezes por segundo;
                // tentar fazer o mínimo possível dentro do handler.
                var onScroll = function (evt) {

                    // é melhor apenas agendar (sem cancelar o agendamento) para parecer mais responsivo (nao espera o fim do scroll)
                    // Ainda assim é mais rápido que cancelar o agendamento e reagendar a acada disparo do evento.
                    if (!scrollPromise) {
                        // pelo menos 150ms para o browser não ficar lento; talvez precise ser mais
                        scrollPromise = $timeout(checkPinning, 150, false); // suppress-validator
                    }
                };

                // ATENÇÃO: eventos resize podem disparar múltiplas vezes por segundo;
                // tentar fazer o mínimo possível dentro do handler.
                var onResize = function (evt) {

                    // é melhor apenas agendar (sem cancelar o agendamento) para parecer mais responsivo (nao espera o fim do scroll)
                    // Ainda assim é mais rápido que cancelar o agendamento e reagendar a acada disparo do evento.
                    if (!resizePromise) {
                        // pelo menos 150ms para o browser não ficar lento; talvez precise ser mais
                        resizePromise = $timeout(resetColumnsWidth, 150, false); // suppress-validator
                    }
                };

                $scope.$on('$destroy', function () {

                    if (!!intervalPromise) {
                        $interval.cancel(intervalPromise);  // suppress-validator
                        intervalPromise = null;
                    }

                    $(document).off('scroll.grid.fixedHeaders', onScroll);

                    $(window).off('resize.grid.fixedHeaders', onResize);

                    if (!!scrollPromise) {
                        $timeout.cancel(scrollPromise); // suppress-validator
                        scrollPromise = null;
                    }
                    if (!!resizePromise) {
                        $timeout.cancel(resizePromise); // suppress-validator
                        resizePromise = null;
                    }

                    $cloneParent.remove();

                    $thead = $table = tableParent = $clone = $cloneParent = clearHeaderColumnsWidth = setHeaderColumnsWidth = pinHeader = unpinHeader = checkPinning = checkLeft = null;
                });

                $(document).on('scroll.grid.fixedHeaders', onScroll);

                $(window).on('resize.grid.fixedHeaders', onResize);
            }
        };
    }

    var cachedTop = null;

    function getTheTop() {

        if (!!cachedTop) return cachedTop;

        var $crumbs = $('.crumbs');

        if ($crumbs.css('position') !== 'fixed') return 0;

        var crumbsPosition = $crumbs.length > 0 ? $crumbs.position() : { top: 0 };
        var crumbsHeight = ($crumbs.css('height') || '0px').replace('px', '') * 1;

        return ((crumbsPosition.top || 0) + crumbsHeight);
    }

    function forcarLargura($x, larg, isLast) {

        larg = larg || '';
        if (larg === '') {
            $x.css(width, '');
            return;
        }

        // Sugestao de pedido (e possivelmente outras grids maiores e complexas) bugam
        // posivelmente por causa da borda branca (foi adicionada da maneira errada no tema)
        // ou pq o thead com position fixed fica com 1px sobrando na direita do elemento
        if (isLast && $x.is('.action')) {
            var tmpwidth = larg.replace('px', '') * 1;
            tmpwidth--;
            larg = tmpwidth.toString() + 'px';
        }

        larg += ' !important'

        var styleAttr = $x.attr('style') || '';
        var theStyles = styleAttr.split(';');
        var found = false;
        for (var i = 0; i < theStyles.length && !found; i++) {
            var keyValue = theStyles[i].split(':');
            if (keyValue[0] === 'width') {
                keyValue[1] = larg;
                found = true;
            }
            theStyles[i] = keyValue.join(':');
        }
        if (!found) {
            if (theStyles.length === 1 && theStyles[0] === '') theStyles = [];
            theStyles.push('width: ' + larg);
        }
        
        styleAttr = theStyles.join(';');
        $x.attr('style', styleAttr);
    }

})();