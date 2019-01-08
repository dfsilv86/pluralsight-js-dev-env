(function () {
    'use strict';

    angular
        .module('common')
        .service('PagingService', PagingService);

    PagingService.$inject = ['$log'];
    function PagingService($log) {

        this.createPagingParams = createPagingParams;
        this.applyPagingParams = applyPagingParams;
        this.createPagingInfo = createPagingInfo;
        this.applyPagingInfo = applyPagingInfo;
        this.calculateOffset = calculateOffset;
        this.acceptPagingResults = acceptPagingResults;
        this.createInMemoryPagedArray = createInMemoryPagedArray;
        this.inMemoryPaging = inMemoryPaging;
        this.toggleSorting = toggleSorting;
        this.toggleSortingUngrouped = toggleSortingUngrouped;
        this.getSortState = getSortState;

        function calculateOffset(paging, pageNumber) {

            if (angular.isUndefined(pageNumber) || null === pageNumber) {

                // Não muda o offset - está realizando refresh
                return;
            }

            // Calcula o offset para a chamada e armazena na model de paginacao
            paging.offset = ((pageNumber) - 1) * paging.limit;
        }

        function acceptPagingResults(paging, result) {

            // Copia as informacoes retornadas pelo servidor para a model da paginacao
            paging.offset = result.offset;
            paging.limit = result.limit;
            paging.orderBy = result.orderBy;
        }

        function createPagingParams(pagingInfo) {
            return {
                'paging.offset': pagingInfo.offset || '',
                'paging.limit': pagingInfo.limit || '',
                'paging.orderBy': pagingInfo.orderBy || ''
            };
        }

        function applyPagingParams(params, pagingInfo) {

            if (angular.isUndefined(pagingInfo) || null === pagingInfo) {
                return params;
            }

            return angular.extend({}, params, createPagingParams(pagingInfo));
        }

        function createPagingInfo(count, offset, limit, orderBy) {

            limit = limit || 10;
            offset = offset || 0;
            count = count || 0;

            var temp = {
                totalCount: count,
                currentPageNumber: Math.floor(offset / limit) + 1,
                lastPageNumber: Math.floor((count - 1) / limit) + 1,
                offset: offset,
                limit: limit,
                pages: [],
                orderBy: orderBy || null
            };

            var pna = temp.currentPageNumber - 4, toab = 0;
            if (pna < 1) {
                toab = 1 - pna;
                pna = 1;
            }
            var pnb = (temp.currentPageNumber + 5) + toab, toaa = 0;
            if (pnb > temp.lastPageNumber) {
                toaa = pnb - temp.lastPageNumber;
                pnb = temp.lastPageNumber;
                pna -= toaa;
                if (pna < 1) pna = 1;
            }

            for (var pageNumber = pna; pageNumber <= pnb; pageNumber++) {
                temp.pages.push(pageNumber);
            }

            return temp;
        }

        function applyPagingInfo(result, count, offset, limit, orderBy) {

            return angular.extend(result, createPagingInfo(count, offset, limit, orderBy));
        }

        function createInMemoryPagedArray(allItems, pagingInfo) {

            $log.warn('Não utilizar createInMemoryPagedArray; Substitua por inMemoryPaging, ou pergunte no Slack.')

            // Deprecated: substituir createInMemoryPagedArray por inMemoryPaging

            var result = !allItems ? [] : allItems.slice(
                pagingInfo.offset,
                pagingInfo.offset + pagingInfo.limit);

            applyPagingInfo(result, allItems.length, pagingInfo.offset, pagingInfo.limit, pagingInfo.orderBy);

            return result;
        }

        function inMemoryPaging(pageNumber, sourceArray, pagedArray, pagingInfo) {

            var pagingInfoSource = pagingInfo || pagedArray;

            var offset = pagingInfoSource.offset, limit = pagingInfoSource.limit, orderBy = pagingInfoSource.orderBy, totalCount = sourceArray.length;

            pagedArray.splice(0, pagedArray.length);

            if (angular.isDefined(pageNumber)) {
                offset = ((pageNumber - 1) * limit);
            }

            while (offset > totalCount) {
                offset -= limit;
            }

            if (offset < 0) offset = 0;

            for (var index = offset, pageLimit = offset + limit; index < pageLimit && index < totalCount; index++) {
                pagedArray.push(sourceArray[index]);
            }

            applyPagingInfo(pagedArray, totalCount, offset, limit, orderBy);
        }

        function toggleSorting(paging, columnName) {

            if (angular.isUndefined(columnName) || null === columnName || columnName.toString().trim() === '' || angular.isUndefined(paging) || paging === null) return;

            var found = false;

            var sorts = visit(paging.orderBy, function (index, sortColumn, sortOrder) {
                if (sortColumn === columnName.toLowerCase()) {
                    found = true;
                    this[1] = (sortOrder === 'asc') ? 'desc' : 'asc';
                    return false;
                }
            });

            paging.orderBy = (!!found) ? sorts : "{0} asc".format(columnName);

            return paging.orderBy;
        }

        function toggleSortingUngrouped(paging, columnName) {

            if (angular.isUndefined(columnName) || null === columnName || columnName.toString().trim() === '' || angular.isUndefined(paging) || paging === null) return;

            var regExp = new RegExp(columnName + "\\sASC", "i");
            var order = regExp.test(paging.orderBy) ? "desc" : "asc";
            paging.orderBy = "{0} {1}".format(columnName, order);
        }

        function getSortState(paging, columnName) {

            if (angular.isUndefined(columnName) || null === columnName || columnName.toString().trim() === '' || angular.isUndefined(paging) || paging === null) return;

            var result = null;

            visit(paging.orderBy, function (index, sortColumn, sortOrder) {
                if (sortColumn === columnName.toLowerCase()) {
                    result = sortOrder;
                    return false;
                }
            });

            return result;
        }
    }

    function visit(sortBy, visitor) {
        if (typeof (visitor) !== 'function') return sortBy;
        var sorts = (sortBy || '').split(',');
        for (var i = 0; i < sorts.length; i++) {
            var sort = (sorts[i] || '').trim();
            sort = sort.split(' ');
            if (sort.length < 2) sort.push('asc');
            var result = visitor.call(sort, i, sort[0].toLowerCase(), sort[1].toLowerCase());
            sorts[i] = sort.join(' ');
            if (result === false) break;
        }
        return sorts.join(',');
    }

})();