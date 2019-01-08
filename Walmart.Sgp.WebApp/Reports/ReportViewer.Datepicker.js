(function () {
    'use strict';

    // Baseado em http://www.rajbandi.net/fixing-ssrs-report-viewer-control-date-picker-in-google-chrome/
    // Sobre formatos de data para <input type="date" /> no HTML5: http://stackoverflow.com/a/9519493

    if (!detectIE()) {
        Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initializeRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);

        $(document).ready(endRequestHandler);
    }

    var theTimeout = null, hasConverted = false;
    
    function initializeRequestHandler(sender, args) {

        if (hasConverted) {
            console.log('Desfazendo datepickers...');
            enumerateDatepickers(toInputText);
            console.log('Concluído.');

            hasConverted = false;
        }
    }

    function endRequestHandler(sender, args) {

        if (!hasConverted) {

            if (null !== theTimeout) {
                window.clearTimeout(theTimeout);
            }

            theTimeout = window.setTimeout(function () {
                console.log('Criando datepickers...');
                enumerateDatepickers(toDatepicker);
                console.log('Concluído.');

                hasConverted = true;
            }, 100);
        }
    }

    // Detecta se é IE ou não
    function detectIE() {
        var ua = window.navigator.userAgent;

        // Test values; Uncomment to check result …

        // IE 10
        // ua = 'Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)';

        // IE 11
        // ua = 'Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko';

        // Edge 12 (Spartan)
        // ua = 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36 Edge/12.0';

        // Edge 13
        // ua = 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586';

        var msie = ua.indexOf('MSIE ');
        if (msie > 0) {
            // IE 10 or older => return version number
            return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
        }

        var trident = ua.indexOf('Trident/');
        if (trident > 0) {
            // IE 11 => return version number
            var rv = ua.indexOf('rv:');
            return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
        }

        var edge = ua.indexOf('Edge/');
        if (edge > 0) {
            // Edge (IE 12+) => return version number
            return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
        }

        // other browser
        return 0;
    }

    // Enumera os campos que realmente devem ser transformados
    function enumerateDatepickers(actionFn) {

        var datePickers = $(":hidden[id*='DatePickers']").val().split(",");

        $(datePickers).each(function (i, item) {
            var td = $("table[id*='ParametersGrid'] span").filter(function (i) {
                var v = "[" + $(this).text() + "]";
                return (v != null && v.indexOf(item) >= 0);
            }).parents("td:first").next("td");

            td.find('input[type="text"]').each(function (index, elem) {
                actionFn(index, elem, td);
            });
        });

    }

    // Transforma em datepicker do jquery ui
    function toDatepicker(index, elem, td) {

        try {
            // O input que vai virar datepicker
            var $elem = $(elem);

            $elem.datepicker({
                showOn: "button",
                buttonImage: "images/calendar.gif",
                buttonImageOnly: true,
                buttonText: "Selecione a data",
                dateFormat: 'dd/mm/yy'
            });

            td.find('input[src]').hide();

        } catch (e) {
            console.error('Não foi converter o campo em datepicker: ' + (e || '').toString());
        }
    }

    // Transforma em campo texto
    function toInputText(index, elem, td) {

        try {
            // O input que vai deixar de ser datepicker
            var $elem = $(elem);

            $elem.datepicker('destroy');

            td.find('.ui-datepicker-trigger').remove();

            td.find('input[src]').show();

        } catch (e) {
            console.error('Não foi converter o campo em text: ' + (e || '').toString());
        }
    }


    // Antigos comentarios (pra nao perder o historico)

    // DZ - 2016-05-24 - A primeira requisicao carrega o cabeçalho com o número de páginas e não possui Reserved_AsyncLoadTarget em seu body
    // Neste caso convertemos os input date de volta em texts com valor no formato brasileiro
    // Quando a primeira requisicao termina, uma segunda req é realizada para buscar o conteúdo do relatório em si.
    // Esta segunda requisição possui Reserved_AsyncLoadTarget em seu body
    // Ao terminar a segunda requisição, temos o relatório em tela, então convertemos os inputs em html5 dates denovo.

    // Warning: possivelmente, se o relatório tem mais de uma página, apenas a segunda requisição é realizada.
    // Para este caso, fazemos a conversão para text em ambas requisições.
    // Como a conversão depende do atributo type ser date, a segunda chamada não vai estragar a requisição.

    // refaz a mudança do datepicker no retorno do postback
    // warning: devido a postback em alguns controles, deve realizar o ajuste sempre.
    // vide Relatório de Itens Com Variação de Custo que tem postback no dropdown de bandeira

    //timeout pro caso da sequencia de duas requisicoes, refazer no inicio do postback nao é o suficiente para mandar a data certa.

})();