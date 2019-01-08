(function () {
    'use strict';

    if (!window.console) window.console = {};
    if (!window.console.log) window.console.log = function () { };

    angular
        .module('SGP', ['ngSanitize', 'common', 'ui', 'ui.bootstrap', 'ui.select2', 'ui.router', 'storage', 'angularMoment', 'ui.utils.masks', 'toaster', 'mwl.confirm', 'ngDialog', 'angular-cache', 'ngMask'])
        .constant('angularMomentConfig', { timezone: 'America/Sao_Paulo' })
        .constant('loaderConfig', { isLateTimeoutInMs: 300 })
        .value('$datepickerSuppressError', true)
        .config(SGPConfig)
        .run(SetupDiagnosticsConsole)
        .run(SGPApp);

    SGPConfig.$inject = ['$urlRouterProvider', '$locationProvider', '$translateProvider', '$httpProvider', 'CacheFactoryProvider'];

    function SGPConfig($urlRouterProvider, $locationProvider, $translateProvider, $httpProvider, CacheFactoryProvider) {

        $urlRouterProvider.otherwise('/404');
        $locationProvider.html5Mode(true);
        $translateProvider.useSanitizeValueStrategy('escape');
        $httpProvider.useLegacyPromiseExtensions(false);

        $httpProvider.defaults.paramSerializer = '$httpCustomParamSerializer';
        $httpProvider.interceptors.push('LoaderInterceptor');
        $httpProvider.interceptors.push('ExceptionHandlingInterceptor');
        $httpProvider.interceptors.push('AuthInterceptor');
        $httpProvider.interceptors.push('WebApiVersionInterceptor');
        $httpProvider.interceptors.push('PagingResultsInterceptor');
        $httpProvider.interceptors.push('FileVaultInterceptor');
        $httpProvider.interceptors.push('ProcessingInterceptor');

        angular.extend(CacheFactoryProvider.defaults, {
            maxAge: 1500,
            deleteOnExpire: 'aggressive'
        });
    }

    SGPApp.$inject = ['SpaConfig', '$rootScope', '$state', '$timeout', '$location', 'UserSessionService', 'StackableModalService', 'confirmationPopoverDefaults', 'ToastService', '$log', 'WebApiService', '$http', 'CacheFactory', 'uiSelect2Config', 'loaderConfig'];

    function SGPApp(spaConfig, $rootScope, $state, $timeout, $location, userSession, stackableModalService, confirmationPopoverDefaults, toastService, $log, webApiService, $http, CacheFactory, uiSelect2Config, loaderConfig) {

        // TODO: cache foi desabilitado em razão deste bug: Bug 4345:Combo Bandeira não preenchido - Tela Ajuste de Estoque
        // pois percebemos que se a mesma requisição levar menos do que o tempo em maxAge, então o cache retorna vazio.
        // $http.defaults.cache = CacheFactory('defaultCache', { maxAge: 1500, deleteOnExpire: 'aggressive', cacheFlushInterval: 3600000, storageMode: 'memory' });

        function goLogin() {
            $timeout(function () { // suppress-validator
                $state.go('home');
            });
        }

        function stateChangeStart(event, toState, toParams, fromState, fromParams, options) {
            stackableModalService.abort();

            var toStateRoute = $state.href(toState, toParams);

            var guestStates = ['home', 'login', 'alterarSenha', 'notFound'];

            if (guestStates.contains(toState.name)) {

                if (!userSession.hasStarted() && toState.name !== 'notFound' && toState.name !== 'alterarSenha') {
                    userSession.end();
                }

            } else if (!userSession.hasStarted()) {

                userSession.pushSessionToResume(toState.name, toParams);
                event.preventDefault();
                $state.go('home');

            } else if (!userSession.canAccessMenu(toState.url, toStateRoute)) {

                $log.warn("Usuário não tem permissão de acesso em {0}.".format(toState.url));
                event.preventDefault();
                toastService.warning(globalization.texts.noPermissionToAccessThisResource);

            } else {

                toastService.dismissAll();
            }

            $('#container').toggleClass('loading', true);
        }

        function showContainer() {
            $timeout(function () { // suppress-validator
                $('#container').toggleClass('loading', false);
            }, loaderConfig.isLateTimeoutInMs || 500, false);
        }

        $rootScope.$on('session-end', goLogin);

        $rootScope.$on('$stateChangeStart', stateChangeStart);
        $rootScope.$on('$stateChangeError', showContainer);
        $rootScope.$on('$stateNotFound', showContainer);
        $rootScope.$on('$viewContentLoaded', showContainer);

        confirmationPopoverDefaults.confirmText = globalization.texts.confirm;
        confirmationPopoverDefaults.cancelText = globalization.texts.cancel;
        confirmationPopoverDefaults.placement = 'right';
            
        uiSelect2Config.language = "pt-BR";

        webApiService.getRemoteVersion().then(function (remoteVersion) {

            var localVersion = webApiService.getLocalVersion();

            if (localVersion !== 'DEV') {
                console.log('<SGP>');
                console.log('   API host: ' + spaConfig.apiHost);
                console.log('   API local version: ' + localVersion);
                console.log('   API remote version: ' + remoteVersion);
                console.log('   APP version: ' + spaConfig.appVersion);
                console.log('</SGP>');
            }

            webApiService.setLocalVersion(remoteVersion);
        });

        userSession.initialize();
    }

    moment.locale('pt-br');

    SetupDiagnosticsConsole.$inject = ['$rootScope', 'SpaConfig', '$log', '$timeout', 'UserSessionService'];
    function SetupDiagnosticsConsole($rootScope, spaConfig, $log, $timeout, userSession) {

        window.console._error = window.console.error;
        window.console._warn = window.console.warn;
        window.console._info = window.console.info;
        window.console._debug = window.console.debug;
        window.console._log = window.console.log;

        $rootScope.diagnostics = {
            consoleError: function (args) {
                if (spaConfig.showConsoleErrorsAsToast) {
                    $timeout(function () { // suppress-validator
                        try {
                            var message = globalization.texts.errorMessageGeneric + '\n\nArgs: \n\n' + args;
                            toastService.error(message);
                        } catch (ex) { }
                    });
                }
                else if (spaConfig.notifyOfConsoleErrors) {
                    $timeout(function () { // suppress-validator
                        try {
                            var message = globalization.texts.errorMessageGeneric;
                            toastService.error(message);
                        } catch (ex) { }
                    });
                }
                if (spaConfig.enableConsole) {
                    window.console._error(args);
                }
            },
            consoleWarn: function (args) {
                if (spaConfig.enableConsole) {
                    window.console._warn(args);
                }
            },
            consoleInfo: function (args) {
                if (spaConfig.enableConsole) {
                    window.console._info(args);
                }
            },
            consoleDebug: function (args) {
                if (spaConfig.enableConsole) {
                    window.console._debug(args);
                }
            },
            consoleLog: function (args) {
                if (spaConfig.enableConsole) {
                    window.console._log(args);
                }
            },
            enableConsole: function () {
                $timeout(function () { // suppress-validator
                    spaConfig.enableConsole = true;
                    spaConfig.notifyOfConsoleErrors = true;
                    $log.info('Console habilitado.');
                });
            },
            showConfig: function () {
                return spaConfig;
            },
            clearProcessingLastCheck: function () {
                userSession.saveUserPreference('processing', 'notificationArea', 'lastCheck', null);
            }
        };

        window.console.error = function (args) {
            $rootScope.diagnostics.consoleError(args);
        };
        window.console.warn = function (args) {
            $rootScope.diagnostics.consoleWarn(args);
        };
        window.console.info = function (args) {
            $rootScope.diagnostics.consoleInfo(args);
        };
        window.console.debug = function (args) {
            $rootScope.diagnostics.consoleDebug(args);
        };
        window.console.log = function (args) {
            $rootScope.diagnostics.consoleLog(args);
        };
    }

})();