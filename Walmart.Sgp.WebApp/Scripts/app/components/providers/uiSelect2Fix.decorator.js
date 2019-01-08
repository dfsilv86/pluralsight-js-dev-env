(function () {

    // Ajuste para o ui-select2 responder a mudanças no binding corretamente
    // https://github.com/angular-ui/ui-select2/issues/244
    uiSelect2Fix.$inject = ['$provide'];
    function uiSelect2Fix($provide) {
        $provide.decorator("uiSelect2Directive", ['$delegate', function ($delegate) {
            var directive;
            directive = $delegate[0];
            directive.priority = 1000;
            return $delegate;
        }]);
    }

    angular
        .module('SGP')
        .config(uiSelect2Fix);
})();