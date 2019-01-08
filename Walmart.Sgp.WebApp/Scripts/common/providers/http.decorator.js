(function () {

    angular
        .module('common')
        .config(HttpDecorator);

    HttpDecorator.$inject = ['$provide'];

    function HttpDecorator($provide) {

        $provide.decorator('$http', ['$delegate', function ($delegate) {

            var $http = $delegate;

            $http._get = $http.get;

            $http.get = function (url, options) {

                if (!!window.$$backgroundRequestFactoryStack && window.$$backgroundRequestFactoryStack.length > 0) {
                    options.background = true;
                }

                return $http._get.apply($http, arguments);
            };

            return $http;
        }]);
    }

})();