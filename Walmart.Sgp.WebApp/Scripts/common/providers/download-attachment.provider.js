(function () {
    'use strict';

    angular
        .module('common')
        .provider('DownloadAttachment', DownloadAttachmentProvider);

    // injetar DownloadAttachment na controller ou serviço, usar com:
    // $http.get('url', ...).then(downloadAttachment);

    var hiddenElement = null;

    function makeRandomId() {
        var result = [];
        for (var i = 0; i < 4; i++) {
            result.push(Math.floor(Math.random() * 15).toString(16));
        }
        return result.join('');
    }

    function DownloadAttachmentProvider() {

        return {
            $get: ['ToastService', function (toastService) {

                function DownloadAttachment(response) {

                    // data: url no chrome parece estar limitado antes dos 0.5mb, ao contrário do que dizem
                    // se o arquivo for maior, deve ser usado outro mecanismo; perguntar no Slack.
                    // se ja funcionou com arquivo maior e isso aqui bloqueou, avisem (será que o problema eram os xlsx?)
                    if (response.data.length > (512 * 1024)) {

                        var msg = 'Tamanho do arquivo excede limite permitido.';
                        toastService.error(msg);
                        throw msg;
                    }

                    var cdh = response.headers('content-disposition');
                    var cdhs = cdh.split(';');
                    var fileName = null;
                    for (var i = 0; i < cdhs.length; i++) {
                        var t = cdhs[i].trim();
                        if (t.toLowerCase().startsWith('filename=')) {
                            fileName = t.substring(9);
                            break;
                        }
                    }
                    if (null === fileName) {
                        fileName = makeRandomId();
                    }
                    var cth = response.headers('content-type');

                    if (null === hiddenElement) {
                        hiddenElement = document.createElement('a');
                    }

                    var buffer = [];
                    buffer.push('data:');
                    buffer.push(cth);
                    buffer.push(',');
                    for (i = 0; i < response.data.length; i++) {
                        var c = response.data.charCodeAt(i);
                        var byte = c & 0xff;
                        buffer.push('%' + ('0' + byte.toString(16)).substr(-2));
                    }

                    hiddenElement.href = buffer.join('');
                    hiddenElement.target = '_blank';
                    hiddenElement.download = fileName;
                    hiddenElement.click();
                    hiddenElement.href = '';
                    buffer = [];
                }

                return DownloadAttachment;
            }]
        };
    }
})();