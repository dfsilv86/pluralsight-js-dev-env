(function () {
    'use strict';

    angular
        .module('SGP')
        .constant('ProcessingConfig', { idleInterval: 60000, activeInterval: 10000, notificationInterval: 2000 })
        .service('ProcessingService', ProcessingService);

    ProcessingService.$inject = ['$q', '$http', 'ApiEndpoints', '$rootScope', 'UserSessionService', '$timeout', 'ChangeTrackerFactory', 'ToastService', '$state', 'ProcessingConfig', 'StackableModalService', '$log'];

    function ProcessingService($q, $http, ApiEndpoints, $rootScope, userSession, $timeout, changeTrackerFactory, toastService, $state, processingConfig, stackableModalService, $log) {

        var self = this, totalCount = null;

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(['firstSeen'], function (a, b) { return a.ticket == b.ticket; });

        function success(response) {
            return response.data;
        }

        function check(itemOrArray, isTicketNotification) {

            var data = angular.isArray(itemOrArray) ? itemOrArray : [itemOrArray];
            var lastCheck = userSession.readUserPreference('processing', 'notificationArea', 'lastCheck', null);
            var hasNewTicketNotification = false;

            if (angular.isDefined(data.totalCount)) {
                totalCount = data.totalCount;
            } else if (isTicketNotification && data[0].modifiedDate > lastCheck) {
                hasNewTicketNotification = true;
                totalCount++;
            }

            var successfull = [], failure = [], queued = [], hasNew = false, hasExecuting = false;

            angular.forEach(data, function (item) {

                var tracked = changeTracker.getTrackedItem(item);

                hasNew = hasNew || !tracked;

                if (null !== tracked && tracked.state != item.state) {
                    if (item.state == 'Finished' || item.state == 'ResultsAvailable') {
                        successfull.push(tracked);
                    } else if (item.state == 'Error' || item.state == 'Failed') {
                        failure.push(tracked);
                    }
                } else {
                    if (item.state == 'Queued' && isTicketNotification) {
                        queued.push(item);
                    }
                }

                if (null !== tracked) {
                    tracked.currentProgress = item.currentProgress;
                    tracked.totalProgress = item.totalProgress;
                    tracked.state = item.state;
                    tracked.message = item.message;
                    tracked.processPercentage = item.processPercentage;
                    tracked.createdDate = item.createdDate || tracked.createdDate;
                    tracked.modifiedDate = item.modifiedDate || tracked.modifiedDate;
                    tracked.startDate = item.startDate || tracked.startDate;
                    tracked.endDate = item.endDate || tracked.endDate;
                }
            });

            var tracked = changeTracker.track(data);

            var oldest = null, tenMinutesPast = new Date();
            tenMinutesPast.setMinutes(tenMinutesPast.getMinutes() - 10);

            angular.forEach(tracked, function (item) {

                hasExecuting = hasExecuting || (item.state == 'IsExecuting') || (item.state == 'Queued');

                if (!item.firstSeen) {
                    item.firstSeen = new Date();
                } else {
                    if (null === oldest || oldest > item.firstSeen) {
                        oldest = item.firstSeen;
                    }
                    if (item.firstSeen < tenMinutesPast) {
                        delete item.firstSeen;
                    }
                }
            });

            if (!!oldest && oldest < tenMinutesPast) {
                userSession.saveUserPreference('processing', 'notificationArea', 'lastCheck', oldest);
            }

            var call = toastService.success;
            if (failure.length > 0) {
                call = successfull.length > 0 ? toastService.warning : toastService.error;
            }

            var msg = [];
            if (queued.length > 0) {
                msg.push(globalization.texts.processQueuedNotification);
                msg.push('<ul>');
                angular.forEach(queued, function (item) {
                    msg.push('<li>');
                    msg.push(globalization.getText('process' + item.processName, true));
                    msg.push('</li>');
                });
                msg.push('</ul>');
            }
            if (successfull.length > 0) {
                if (queued.length > 0) {
                    msg.push('<br />');
                }
                msg.push(globalization.texts.processFinishedNotification);
                msg.push('<ul>');
                angular.forEach(successfull, function (item) {
                    msg.push('<li>');
                    msg.push(globalization.getText('process' + item.processName, true));
                    msg.push('</li>');
                });
                msg.push('</ul>');
            }
            if (failure.length > 0) {
                if (queued.length > 0 || successfull.length > 0) {
                    msg.push('<br />');
                }
                msg.push(globalization.texts.processFailedNotification);
                msg.push('<ul>');
                angular.forEach(failure, function (item) {
                    msg.push('<li>');
                    msg.push(globalization.getText('process' + item.processName, true));
                    msg.push('</li>');
                });
                msg.push('</ul>');
            }

            if (msg.length > 0) {
                call.call(toastService, msg.join(''));
            }

            var theList = changeTracker.getChangedItems();

            theList.totalCount = totalCount;

            $rootScope.$broadcast('processing-update', { trackedProcesses: theList, lastCheck: lastCheck });

            if ((hasNew && !isTicketNotification) || hasNewTicketNotification || successfull.length > 0 || failure.length > 0) {
                $rootScope.$broadcast('processing-notify');
            }

            if (null != self.checkTimeout) {
                $timeout.cancel(self.checkTimeout); // suppress-validator
                self.checkTimeout = null;
            }

            var timeout = processingConfig.idleInterval;

            if (hasExecuting) {
                timeout = processingConfig.activeInterval;
            }

            for (var i = 0; i < successfull.length; i++) {
                if (successfull[i].state == 'Finished') { // pro caso de ainda nao ter disponibilizado os resultados.
                    timeout = 1000;
                    break;
                }
            }

            self.checkTimeout = $timeout(self.checkNotifications, timeout); // suppress-validator
        }

        function cancelTimeout() {
            if (null != self.checkTimeout) {
                $timeout.cancel(self.checkTimeout); // suppress-validator
                self.checkTimeout = null;
            }
        }

        function stopNotifications() {

            cancelTimeout();

            changeTracker.reset();
        }

        self.getProcessNames = function (userId) {

            return $http
                .get(ApiEndpoints.sgp.processing + 'ProcessNames/', { params: { createdUserId: userId || '' } })
                .then(success);
        };

        self.findAllByUser = function (userId, processName, state, paging) {

            var params = ApiEndpoints.createParams(paging, { userId: userId || '', processName: processName || '', state: state || '' });

            return $http
                .get(ApiEndpoints.sgp.processing + 'AllUsers/', { params: params })
                .then(success);
        };

        self.getTrackedProcesses = function () {
            return changeTracker.getChangedItems();
        };

        self.getTicketNotifications = function (lastCheck) {
            return $http
                .get(ApiEndpoints.sgp.processing + 'Notifications/', { params: { lastCheck: lastCheck || '' } })
                .then(success);
        };

        self.getTicket = function (ticket) {
            return $http
                .get(ApiEndpoints.sgp.processing + 'Ticket/{0}'.format(ticket))
                .then(success);
        };

        self.getTicketDetails = function (ticket) {
            return $http
                .get(ApiEndpoints.sgp.processing + 'Ticket/{0}/Details'.format(ticket))
                .then(success);
        };

        self.getTicketLogs = function (ticket, paging) {

            var params = ApiEndpoints.createParams(paging, {});

            return $http
                .get(ApiEndpoints.sgp.processing + 'Ticket/{0}/Logs'.format(ticket), { params: params })
                .then(success);
        };

        self.getTicketResults = function (ticket) {
            return $http
                .get(ApiEndpoints.sgp.processing + 'Ticket/{0}/Results'.format(ticket))
                .then(success);
        };

        self.checkTimeout = null;

        self.checkNotifications = function () {

            cancelTimeout();

            var lastCheck = userSession.readUserPreference('processing', 'notificationArea', 'lastCheck', null);
            self.getTicketNotifications(lastCheck).then(check, function (reason) {
                self.checkTimeout = $timeout(self.checkNotifications, processingConfig.idleInterval); // suppress-validator
            });
        };

        self.viewManager = function (order) {
            return stackableModalService.open({
                templateUrl: 'Scripts/app/components/processing/manage-processes.view.html',
                controller: 'ManageProcessesController',
                resolve: {
                    currentUser: userSession.getCurrentUser()
                }
            });
        };

        self.viewDetails = function (order) {
            self.getTicketDetails(order.ticket).then(function (processOrder) {
                if (null !== processOrder) {
                    return stackableModalService.open({
                        templateUrl: 'Scripts/app/components/processing/manage-process.view.html',
                        controller: 'ManageProcessController',
                        resolve: {
                            processOrder: processOrder
                        }
                    });
                } else {
                    // TODO: talvez exibir algo como "Não encontrado" para podermos usar o botão de visualizar log?
                    toastService.warning(globalization.texts.couldNotFindProcessOrder.format(order.ticket))
                }
            });
        };

        self.viewLogs = function (order) {
            self.getTicketDetails(order.ticket).then(function (processOrder) {
                if (null !== processOrder) {
                    return stackableModalService.open({
                        templateUrl: 'Scripts/app/components/processing/view-process-logs.view.html',
                        controller: 'ViewProcessLogsController',
                        resolve: {
                            processOrder: processOrder
                        }
                    });
                } else {
                    // TODO: na verdade o log tem informações suficientes para exibir a tela, mas ainda não está funcionando assim.
                    toastService.warning(globalization.texts.couldNotFindProcessOrder.format(order.ticket))
                }
            });
        };

        self.viewResults = function (order) {

            var stateName1 = order.processName[0].toLowerCase() + order.processName.substr(1) + 'Results';
            var stateName2 = order.processName + 'Results';
            var stateName = stateName1;

            if (null == $state.get(stateName)) {
                stateName = stateName2;
                $log.warn('Não foi possível encontrar o estado para visualização de resultados do processo "' + order.processName + '" ("' + stateName1 + '"), tentando utilizar nome alternativo ("' + stateName2 + '")...');
            }

            if (null == $state.get(stateName)) {
                $log.error('Não foi possível encontrar o estado para visualização de resultados do processo "' + order.processName + '" ("' + stateName1 + '" ou "' + stateName2 + '")');
            }

            $state.go(stateName, { ticket: order.ticket }, { rebuild: true });
        };

        $rootScope.$on('processing-ticket-notification', function (event, processOrder) {
            check(processOrder, true);

            cancelTimeout();

            self.checkTimeout = $timeout(self.checkNotifications, processingConfig.notificationInterval); // suppress-validator
        });

        $rootScope.$on('session-start', self.checkNotifications);
        $rootScope.$on('session-end', stopNotifications);
        $rootScope.$on('lock-screen', stopNotifications);

        if (userSession.hasStarted()) {
            self.checkNotifications();
        }
    }
})();