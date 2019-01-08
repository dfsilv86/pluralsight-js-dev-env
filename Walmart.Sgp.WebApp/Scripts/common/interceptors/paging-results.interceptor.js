(function () {
    'use strict';

    angular.module('common')
      .factory('PagingResultsInterceptor', PagingResultsInterceptor);

    PagingResultsInterceptor.$inject = ['$q', 'PagingService'];

    function PagingResultsInterceptor($q, pagingService) {
        return {
            'response': function (response) {

                if (angular.isArray(response.data)) {

                    var totalCount = response.headers('x-paging-total-count');
                    if (!!totalCount) totalCount = parseInt(totalCount, 10);
                    var offset = response.headers('x-paging-offset');
                    if (!!offset) offset = parseInt(offset, 10);
                    var limit = response.headers('x-paging-limit');
                    if (!!limit) limit = parseInt(limit, 10);
                    var orderBy = response.headers('x-paging-order-by');

                    response.data = pagingService.applyPagingInfo(response.data, totalCount, offset, limit, orderBy);

                    // Caso esteja tentando obter registros além do total disponível, tenta ir para a última página disponível.
                    // cenários: 1. estava na pesquisa, foi para última página, só tinha 1 item, removeu esse item: deve ir para a página anterior (= nova última página)
                    //           2. tinha um filtro que retorna vários registros, está em qualquer página depois da primeira, mudou o filtro e passou a retornar menos registros que os da página atual: deve ir para a última página.
                    if (offset >= totalCount && offset > 0 && response.config.method === 'GET' && !!response.config.params['paging.offset'] && !!response.config.params['paging.limit']) {

                        var newOffset = Math.floor((totalCount + 1) / limit) * limit;

                        response.config.params['paging.offset'] = newOffset;

                        var injector = $('body').injector();
                        var $http = injector.get('$http');

                        return $http.get(response.config.url, response.config);
                    }
                }

                return response;
            }
        };
    }
})();