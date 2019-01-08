/// <reference path='./_references.js' />
(function () {
    'use strict';

    angular
      .module('common')
      .service('ToastService', ['toaster', '$timeout', '$sanitize', '$log',
        function (toaster, $timeout, $sanitize, $log) {
            var ToastService = (function () {
                function ToastService() {
                    this.ignoreSameMessageWindow = 10000;
                    this.lastMessage = '';
                    this.lastMessageTime = new Date();
                }

                return ToastService;
            })();

            ToastService.prototype.success = function (message) {
                this._sendMessage('success', message);
            };

            ToastService.prototype.error = function (message) {
                $log.debug("Mensagem retornada pelo servidor: {0}".format(message));
                this._sendMessage('error', message);
            };

            ToastService.prototype.warning = function (message) {
                this._sendMessage('warning', message);
            };

            ToastService.prototype.dismissAll = function () {
                toaster.clear();
            };

            ToastService.prototype._sendMessage = function (kind, message) {
                if (message === undefined) {
                    $log.warn("ToastService chamado sem mensagem definida.");
                    return;
                }

                var $this = this;
                if (this._canShowMessage(message)) {
                    toaster.pop({
                        type: kind,
                        timeout: 10000,
                        body: $sanitize(message.replace(/\n/gi, '<br />')),
                        bodyOutputType: 'trustedHtml',
                        onHideCallback: function () {
                            $this.lastMessage = '';
                        }
                    });
                    this.lastMessage = message;
                    this.lastMessageTime = new Date();
                }
            };

            ToastService.prototype._canShowMessage = function (message) {
                // Evita que mensagens iguais enviadas num curto espaço de tempo sejam mostradas repetidademente ao usuário.
                return (message !== undefined && message !== this.lastMessage) || (new Date() - this.lastMessageTime) > this.ignoreSameMessageWindow;
            };

            return new ToastService();
        }
      ]);
})();

