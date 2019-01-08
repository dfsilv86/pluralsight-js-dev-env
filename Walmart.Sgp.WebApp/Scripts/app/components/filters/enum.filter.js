(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('enum', theFilter);

    function theFilter() {
        return enumFilter;
    }

    function enumFilter(input, enumName) {
        if (null === input || angular.isUndefined(input)) return input;

        switch (enumName) {
            case 'enTpVinculado':
                switch (input) {
                    case 'S':
                        return 'outbound';
                    case 'E':
                        return 'inbound';
                    default:
                        return 'notDefined';
                }
            case 'enTpReceituario':
                switch (input) {
                    case 'T':
                        return 'transformed';
                    case 'I':
                        return 'input';
                    default:
                        return 'notDefined';
                }
            case 'enTpManipulado':
                switch (input) {
                    case 'P':
                        return 'parent';
                    case 'D':
                        return 'derived';
                    default:
                        return 'notDefined';
                }
            case 'enTipoArquivoInventario':
                switch (input.toString()) {
                    case '1':
                        return 'final';
                    case '2':
                        return 'partial';
                    default:
                        return 'Error: invalid enTipoArquivoInventario';
                }
            case 'enTpStatusItemHost':
                switch (input) {
                    case 'A':
                        return 'active';
                    case 'I':
                        return 'inactive';
                    case 'D':
                        return 'deleted';
                    default:
                        return 'Error: invalid enTpStatusItemHost';
                }
            case 'enYesNo':
                switch (input) {
                    case 'S': return 'yes';
                    case 'Y': return 'yes';
                    case 'N': return 'no';
                    default:
                        return 'Error: invalid enYesNo';
                }
            case 'enYesOnly':
                switch (input) {
                    case 'S': return 'yes';
                    case 'Y': return 'yes';
                    default:
                        return '';
                }
            case 'enTipoItem':
                switch (input.toString()) {
                    case '0':
                    case 'PesoFixo':
                        return 'fixedWeight';
                    case '1':
                    case 'PesoVariavel':
                        return 'variableWeight';
                    default:
                        return 'Error: invalid enTipoItem';
                }
            case 'enTipoCaixaFornecedor':
                switch (input.toString()) {
                    case 'F':
                        return 'tipoCaixaFornecedorFixedValueF';
                    case 'V':
                        return 'tipoCaixaFornecedorFixedValueV';
                    default:
                        return 'Error: invalid enTipoCaixaFornecedor';
                }
            case 'enTipoProduto':
                switch (input.toString()) {
                    case 'F':
                        return 'fixed2';
                    case 'V':
                        return 'variable';
                    default:
                        return 'Error: invalid enTipoProduto';
                }
            case 'enTipoCarga':
                switch (input.toString()) {
                    case "true":
                        return 'tipoCargaFixedValueTrue';
                    case "false":
                        return 'tipoCargaFixedValueFalse';
                    default:
                        return 'Error: invalid enTipoCarga';
                }
            case 'qtVendorPackage':
                return (input.toString() == '1') ? 'qtVendorPackageUnit' : 'qtVendorPackageBox';
            case 'tpUnidadeMedida':
                if (input.toUpperCase() !== 'Q' && input.toUpperCase() !== 'U') return 'Error: invalid tpUnidadeMedida';
                return 'tipoUnidadeMedidaFixedValue' + input.toString();
            default:
                return enumName.toString() + 'FixedValue' + input.toString();
        }
    }
})();
