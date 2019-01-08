(function () {

    angular
        .module('common')
        .constant('StateHierarchyConfig', new StateHierarchyConfig())
        .config(StateDecorator);

    StateDecorator.$inject = ['$provide'];

    function StateDecorator($provide) {

        var $log = { debug: function (msg) { console.debug(msg); } };

        // Decora $state para gerenciar pilha de navegação dentro do app
        $provide.decorator('$state', ['$delegate', 'capitalizeFilter', 'StateHierarchyConfig', 'SpaConfig', '$q', function ($delegate, capitalizeFilter, hierarchy, spaConfig, $q) {

            var $state = $delegate;

            $state._transitionTo = $state.transitionTo;

            var stack = [];

            // Modifica o transitionTo de forma a interceptar navegação direta via href (ao abrir um link diretamente para uma página, ou dar reload),
            // ou a navegação 'normal' via $state.go dentro de controllers (da pagina de pesquisa para um detalhamento, por exemplo)
            $state.transitionTo = function (to, toParams, options) {

                if (spaConfig.logDiagnosticMessagesToConsole) {
                    $log.debug('Interceptando navegação...');
                }

                var stateName = to.toString();
                var stateHref = null;
                var undo = angular.copy(stack);

                if (stack.length > 0 && stack[stack.length - 1].name == stateName) {
                    if (spaConfig.logDiagnosticMessagesToConsole) {
                        $log.debug('  Recarregando estado.');
                        $log.debug('  Desempilhando estado anterior...');
                    }
                    // voltando pro estado anterior
                    stack.pop();
                    if (spaConfig.logDiagnosticMessagesToConsole) {
                        $log.debug('  Navegando para o mesmo estado...');
                    }
                } if (stack.length > 1 && stack[stack.length - 2].name == stateName) {
                    // 1. Caso esteja indo para um estado presente na pilha de navegação, diretamente anterior ao estado atual (voltando a página)
                    if (spaConfig.logDiagnosticMessagesToConsole) {
                        $log.debug('  Desempilhando estado anterior...');
                    }
                    // voltando pro estado anterior
                    stack.pop();
                    // Pegamos os parametros salvos (que são os parametros usados para entrar no estado anterior, possivelmente modificados com $state.update)
                    //   e atualizamos com parametros informados agora, caso exista algum.
                    toParams = angular.extend({}, stack[stack.length - 1].params, toParams);
                    // tiramos o estado anterior da pilha, pois será reinserido ao terminar a navegação.
                    stack.pop();
                } else if (stack.length > 0 && ((stack[stack.length - 1].name.endsWith('Edit') && stateName.endsWith('New')) || (stack[stack.length - 1].name.endsWith('New') && stateName.endsWith('Edit')))) {
                    // 2. Caso esteja em um estado FooEdit e indo para um estado FooNew (ex. clicar botao Novo dentro de uma pagina de edição de item)
                    if (spaConfig.logDiagnosticMessagesToConsole) {
                        $log.debug('  Navegando para estado paralelo...');
                    }
                    // Tiramos o estado atual da pilha; o novo estado será reinserido
                    stack.pop();
                } else if (typeof (to) === 'object') {
                    // 3. Parametro to é um objeto (ao invés da string com o nome do estado destino)
                    //    É o que acontece quando se modifica a url do browser num link href, por exemplo, nos menus da aplicação
                    //    Neste caso é seguro dizer que o usuário está começando um novo fluxo (via menu), zeramos a pilha
                    if (spaConfig.logDiagnosticMessagesToConsole) {
                        $log.debug('  Novo fluxo de navegação.');
                    }
                    $state.resetAndRebuildStack(stateName, toParams);
                    stateHref = to.self.url;
                } else if (!!options && !!options.rebuild) {
                    // 4. Solicitado explicitamente para refazer a pilha de navegação (por ex. visualizar resultado de processamento)
                    $state.resetAndRebuildStack(stateName, toParams);
                }

                stateHref = stateHref || $state.href(to, toParams, options);

                // Realizamos a transição...
                return $q
                    .when($state._transitionTo(to, toParams, options))
                    .then(function (args) {
                        if (spaConfig.logDiagnosticMessagesToConsole) {
                            $log.debug('  Alteração do estado concluída.');
                        }
                        if (!!options && !!options.reset) {
                            // $state.go solicitou explicitamente para resetar a navegação.
                            if (spaConfig.logDiagnosticMessagesToConsole) {
                                $log.debug('  Novo fluxo de navegação.');
                            }
                            stack.splice(0, stack.length);
                        }
                        if (stateName == 'home' || stateName == 'login') {
                            // Navegamos para a home ou login, que ficam fora da pilha.
                            stack.splice(0, stack.length);
                        } else {
                            // Transição com sucesso, colocamos o novo estado no topo da pilha
                            if (spaConfig.logDiagnosticMessagesToConsole) {
                                $log.debug('  Empilhando novo estado...');
                            }
                            var titleGlobalizationKey = 'State' + stateName.capitalize() + 'Name';
                            var theTitle = capitalizeFilter(globalization.getText(titleGlobalizationKey, true));
                            stack.push({ name: stateName, titleKey: titleGlobalizationKey, title: theTitle, href: stateHref, params: toParams });
                        }
                        if (spaConfig.logDiagnosticMessagesToConsole) {
                            $log.debug('  Ajuste da pilha concluído.');
                        }
                        undo = null;
                        return args;
                    }).catch(function (reason) {
                        if (spaConfig.logDiagnosticMessagesToConsole) {
                            if (!!reason) {
                                $log.debug('Navegação cancelada: ' + reason);
                            } else {
                                $log.debug('Navegação cancelada de maneira inesperada.');
                            }
                        }
                        // Navegação falhou por algum motivo, desfaz alterações
                        stack.splice(0, stack.length);
                        stack.push.apply(stack, undo);
                        undo = null;
                        return $q.reject(reason);
                    });
            };

            $state.goBack = function (levels) {
                // volta a navegação 1 ou mais níveis
                if (levels < 1) return;
                if (levels >= stack.length) {
                    if (spaConfig.logDiagnosticMessagesToConsole) {
                        $log.error("Tentativa de desempilhar mais estados de navegação do que o disponível.");
                    }
                    return;
                }

                var undo = angular.copy(stack);

                for (var i = 0; i < levels; i++) {
                    stack.pop();
                }
                var target = stack[stack.length - 1];
                stack.pop();
                return $q
                    .when($state.go(target.name, target.params))
                    .then(function (args) {
                        undo = null;
                        return args;
                    }).catch(function (reason) {
                        stack.splice(0, stack.length);
                        stack.push.apply(stack, undo);
                        undo = null;
                        return $q.reject(reason);
                    });
            };

            $state.update = function (params) {
                // Atualiza os parametros presentes no estado no topo da pilha, de forma a permitir voltar para ele com filtros ou paginação preenchidos
                if (spaConfig.logDiagnosticMessagesToConsole) {
                    $log.debug('Atualizando parâmetros do estado atual:');
                }
                stack[stack.length - 1].params = angular.extend({}, stack[stack.length - 1].params, params);
                if (spaConfig.logDiagnosticMessagesToConsole) {
                    $log.debug('  Concluído.');
                }
            };

            $state.getNavigationStack = function () {
                // Retorna a pilha
                return stack;
            };

            $state.resetAndRebuildStack = function (stateName, toParams) {
                // zera e refaz a pilha baseando-se na hierarquia
                stack.splice(0, stack.length);
                var previousNodes = hierarchy.getNodesForState(stateName);
                for (var i = 0; i < previousNodes.length; i++) {
                    var titleGlobalizationKey = 'State' + previousNodes[i].capitalize() + 'Name';
                    var theTitle = capitalizeFilter(globalization.getText(titleGlobalizationKey, true));
                    stack.push({ name: previousNodes[i], titleKey: titleGlobalizationKey, title: theTitle, href: null, params: toParams })
                }
            }

            return $state;
        }]);
    }

    function StateHierarchyConfig() {
        this.nodes = {};
    }
    StateHierarchyConfig.prototype.addNodes = function (nodes) {
        this.nodes = angular.merge(this.nodes, nodes);
    };
    StateHierarchyConfig.prototype.getNodesForState = function (name) {

        var queue = [{ path: [], nodes: this.nodes }];

        while (queue.length > 0) {

            var workItem = queue.pop();
            
            var nodeKeys = Object.keys(workItem.nodes);

            for (var i = 0; i < nodeKeys.length; i++) {

                var nodeKey = nodeKeys[i];

                if (nodeKey == name) {
                    return workItem.path;
                }

                var children = workItem.nodes[nodeKey];

                if (null != children) {
                    var path = angular.copy(workItem.path);
                    path.push(nodeKey);
                    queue.push({ path: path, nodes: children });
                }
            }
        }
        return [];
    };
})();