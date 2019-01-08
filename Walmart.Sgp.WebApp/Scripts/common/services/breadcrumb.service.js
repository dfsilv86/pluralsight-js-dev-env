(function () {
    'use strict';

    angular
        .module('common')
        .service('BreadcrumbService', BreadcrumbService);

    BreadcrumbService.$inject = ['$q'];

    function BreadcrumbService($q) {
        var self = this;
        self._elements = [];
        self.register = function (elem) {
            var exists = false;
            for (var i = 0; i < self._elements.length && !exists; i++) {
                exists = self._elements[i] === elem;
            }
            if (!exists) self._elements.push(elem);
        };
        self.unregister = function (elem) {
            var index = -1;
            for (var i = 0; i < self._elements.length && index === -1; i++) {
                if (self._elements[i] === elem) {
                    index = i;
                }
            }
            if (index !== -1) {
                self._elements.splice(index, 1);
            }
        };
        self.push = function (options) {
            var temp = [];
            temp.push('<li>');
            if (!!options.icon) {
                temp.push('<span class="icon-fix"><span class="icon-');
                temp.push(options.icon);
                temp.push('"></span></span>');
            }
            temp.push('<span class="the-title">');
            temp.push(options.title);
            temp.push('</span></li>');
            var newBreadcrumb = $(temp.join(''));
            if (options.click) {
                newBreadcrumb.click(function (evt) {
                    return options.click(evt);
                });
            }

            temp = self._elements[self._elements.length - 1];

            if (temp != null) {
                var parent = temp.find('ul.breadcrumb');
                parent.append(newBreadcrumb);
            }
        };
        self.pop = function (options) {
            var parent = self._elements[self._elements.length - 1];
            var target = parent.find('ul.breadcrumb li').last().detach();
        };
    }
})();