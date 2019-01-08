(function () {
    'use strict';

    window.sgpFixedValues = {
        getByValue: function (fixedValueName, value) {
            var normalizedValue = value === undefined || value === null ? '' : value.toString().toLowerCase();

            var fixedValue = this[fixedValueName];

            if (fixedValue == undefined) {
                return null;
            }

            return fixedValue.filter(function (obj) {
                return obj.normalizedValue == normalizedValue;
            })[0];
        },
        getByDescription: function (fixedValueName, description) {
            var normalizedDescription = description.toLowerCase().removeAccents();

            var fixedValue = this[fixedValueName];

            if (fixedValue == undefined) {
                return null;
            }

            return fixedValue.filter(function (obj) {
                return obj.normalizedDescription == normalizedDescription;
            })[0];
        },
        _normalize: function (items) {
            for (var index in items) {
                var i = items[index];

                if (typeof i !== 'function') {
                    i.normalizedValue = i.value === undefined || i.value === null ? '' : i.value.toString().toLowerCase();

                    var descriptionNoAccents = i.description.removeAccents();
                    i.normalizedDescription = descriptionNoAccents.toLowerCase();

                    items[descriptionNoAccents] = i.value;
                    items[descriptionNoAccents.substring(0, 1).toLowerCase() + descriptionNoAccents.substring(1)] = i.value;
                }
            }
        },
        init: function () {
            this._normalize(this.auditKind);
            this._normalize(this.inventarioStatus);
            this._normalize(this.inventarioAgendamentoStatus);
            this._normalize(this.tipoRelacionamento);
            this._normalize(this.tipoUnidadeMedida);
            this._normalize(this.tipoStatusItem);
            this._normalize(this.fornecedorStatus);
            this._normalize(this.tipoOrigemSugestao);
            this._normalize(this.tipoReabastecimento);
            this._normalize(this.valorTipoReabastecimento);
            this._normalize(this.tipoSemana);
            this._normalize(this.tipoIntervalo);
            this._normalize(this.tipoPedidoMinimo);
            this._normalize(this.tipoCD);
            this._normalize(this.tipoDetalhamentoReviewDate);
            this._normalize(this.uf);
            this._normalize(this.statusNotaFiscalItem);
            this._normalize(this.tipoSistema);
            this._normalize(this.tipoFiltroItemInventario);
            this._normalize(this.tipoAtivo);
            this._normalize(this.bandeiraStatus);
            this._normalize(this.tipoCarga);
            this._normalize(this.tipoStatus);
            this._normalize(this.tipoSimNao);
            this._normalize(this.tipoCaixaFornecedor);
            this._normalize(this.tipoSimVariavelNaoFixo);
            this._normalize(this.tipoCaixaFornecedorFixoVariavel);
            this._normalize(this.processOrderState);
            this._normalize(this.tipoOrigemImportacao);
            this._normalize(this.tipoCaixaFornecedorShort);
        },
        auditKind: [
            { value: 1, description: globalization.getText("AuditKindFixedValue1") },
            { value: 2, description: globalization.getText("AuditKindFixedValue2") },
            { value: 3, description: globalization.getText("AuditKindFixedValue3") }
        ],
        tipoFiltroItemInventario: [
            { value: 0, description: globalization.getText("TipoFiltroItemInventarioFixedValue0") },
            { value: 1, description: globalization.getText("TipoFiltroItemInventarioFixedValue1") },
            { value: 2, description: globalization.getText("TipoFiltroItemInventarioFixedValue2") },
            { value: 3, description: globalization.getText("TipoFiltroItemInventarioFixedValue3") },
            { value: 4, description: globalization.getText("TipoFiltroItemInventarioFixedValue4") },
            { value: 5, description: globalization.getText("TipoFiltroItemInventarioFixedValue5") },
            { value: 6, description: globalization.getText("TipoFiltroItemInventarioFixedValue6") }
        ],
        tipoSistema: [
            { value: 1, description: globalization.getText("TipoSistemaFixedValue1") },
            { value: 2, description: globalization.getText("TipoSistemaFixedValue2") }
        ],
        inventarioStatus: [
           { value: 0, description: globalization.getText("InventarioStatusFixedValue0") },
           { value: 1, description: globalization.getText("InventarioStatusFixedValue1") },
           { value: 2, description: globalization.getText("InventarioStatusFixedValue2") },
           { value: 3, description: globalization.getText("InventarioStatusFixedValue3") },
           { value: 4, description: globalization.getText("InventarioStatusFixedValue4") },
           { value: 5, description: globalization.getText("InventarioStatusFixedValue5") }
        ],
        inventarioAgendamentoStatus: [
           { value: 0, description: globalization.getText("InventarioAgendamentoStatusFixedValue0") },
           { value: 1, description: globalization.getText("InventarioAgendamentoStatusFixedValue1") },
           { value: 2, description: globalization.getText("InventarioAgendamentoStatusFixedValue2") },
        ],
        tipoRelacionamento: [
            { value: 1, description: globalization.getText("TipoRelacionamentoFixedValue1") },
            { value: 2, description: globalization.getText("TipoRelacionamentoFixedValue2") },
            { value: 3, description: globalization.getText("TipoRelacionamentoFixedValue3") }
        ],
        tipoUnidadeMedida: [
            { value: 'Q', description: globalization.getText("TipoUnidadeMedidaFixedValueQ") },
            { value: 'U', description: globalization.getText("TipoUnidadeMedidaFixedValueU") }
        ],
        tipoStatusItem: [
            { value: 'A', description: globalization.getText("TipoStatusItemFixedValueA") },
            { value: 'I', description: globalization.getText("TipoStatusItemFixedValueI") },
            { value: 'D', description: globalization.getText("TipoStatusItemFixedValueD") }
        ],
        fornecedorStatus: [
            { value: 'A', description: globalization.getText("FornecedorStatusFixedValueA") },
            { value: 'I', description: globalization.getText("FornecedorStatusFixedValueI") },
            { value: 'D', description: globalization.getText("FornecedorStatusFixedValueD") }
        ],
        tipoOrigemSugestao: [
            { value: 'T', description: globalization.getText("TipoOrigemSugestaoFixedValueT") },
            { value: 'I', description: globalization.getText("TipoOrigemSugestaoFixedValueI") },
            { value: 'S', description: globalization.getText("TipoOrigemSugestaoFixedValueS") },
            { value: 'G', description: globalization.getText("TipoOrigemSugestaoFixedValueG") },
            { value: 'M', description: globalization.getText("TipoOrigemSugestaoFixedValueM") },
        ],
        tipoReabastecimento: [
            { value: 'C', description: globalization.getText("TipoReabastecimentoFixedValueC") },
            { value: 'S', description: globalization.getText("TipoReabastecimentoFixedValueS") }
        ],
        valorTipoReabastecimento: [
            { value: null, description: globalization.getText("valorTipoReabastecimentoFixedValue") },
            { value: 20, description: globalization.getText("valorTipoReabastecimentoFixedValue20") },
            { value: 22, description: globalization.getText("valorTipoReabastecimentoFixedValue22") },
            { value: 40, description: globalization.getText("valorTipoReabastecimentoFixedValue40") },
            { value: 42, description: globalization.getText("valorTipoReabastecimentoFixedValue42") },
            { value: 43, description: globalization.getText("valorTipoReabastecimentoFixedValue43") },
            { value: 81, description: globalization.getText("valorTipoReabastecimentoFixedValue81") },
            { value: 3, description: globalization.getText("valorTipoReabastecimentoFixedValue3") },
            { value: 33, description: globalization.getText("valorTipoReabastecimentoFixedValue33") },
            { value: 94, description: globalization.getText("valorTipoReabastecimentoFixedValue94") },
            { value: 7, description: globalization.getText("valorTipoReabastecimentoFixedValue7") },
            { value: 37, description: globalization.getText("valorTipoReabastecimentoFixedValue37") },
            { value: 97, description: globalization.getText("valorTipoReabastecimentoFixedValue97") }
        ],
        tipoSemana: [
            { value: null, description: globalization.texts.select },
            { value: 1, description: globalization.getText("TipoSemanaFixedValue1") },
            { value: 2, description: globalization.getText("TipoSemanaFixedValue2") }
        ],
        tipoIntervalo: [
            { value: null, description: globalization.texts.select },
            { value: 1, description: globalization.getText("TipoIntervaloFixedValue1") },
            { value: 2, description: globalization.getText("TipoIntervaloFixedValue2") }
        ],
        tipoPedidoMinimo: [
            { value: null, description: globalization.texts.select },
            { value: '$', description: globalization.getText("TipoPedidoMinimoFixedValueDinheiro") },
            { value: 'C', description: globalization.getText("TipoPedidoMinimoFixedValueC") }
        ],
        tipoAtivo: [
            { value: null, description: globalization.texts.select },
            { value: 'S', description: globalization.getText("TipoAtivoFixedValueS") },
            { value: 'N', description: globalization.getText("TipoAtivoFixedValueN") }
        ],
        bandeiraStatus: [
            { value: null, description: globalization.texts.select },
            { value: 'S', description: globalization.getText("BandeiraStatusFixedValueS") },
            { value: 'N', description: globalization.getText("BandeiraStatusFixedValueN") }
        ],
        tipoDetalhamentoReviewDate: [
            { value: 'Todos', description: globalization.getText("TipoDetalhamentoReviewDateFixedValueTodos") },
            { value: 'Loja', description: globalization.getText("TipoDetalhamentoReviewDateFixedValueLoja") },
            { value: 'CD', description: globalization.getText("TipoDetalhamentoReviewDateFixedValueCD") }
        ],
        uf: [
            { value: 'AC', description: 'AC' },
            { value: 'AL', description: 'AL' },
            { value: 'AP', description: 'AP' },
            { value: 'AM', description: 'AM' },
            { value: 'BA', description: 'BA' },
            { value: 'CE', description: 'CE' },
            { value: 'DF', description: 'DF' },
            { value: 'ES', description: 'ES' },
            { value: 'GO', description: 'GO' },
            { value: 'MA', description: 'MA' },
            { value: 'MT', description: 'MT' },
            { value: 'MS', description: 'MS' },
            { value: 'MG', description: 'MG' },
            { value: 'PA', description: 'PA' },
            { value: 'PB', description: 'PB' },
            { value: 'PR', description: 'PR' },
            { value: 'PE', description: 'PE' },
            { value: 'PI', description: 'PI' },
            { value: 'RJ', description: 'RJ' },
            { value: 'RN', description: 'RN' },
            { value: 'RS', description: 'RS' },
            { value: 'RO', description: 'RO' },
            { value: 'RR', description: 'RR' },
            { value: 'SC', description: 'SC' },
            { value: 'SP', description: 'SP' },
            { value: 'SE', description: 'SE' },
            { value: 'TO', description: 'TO' }
        ],
        statusNotaFiscalItem: [
            { value: 1, description: globalization.getText("IdConforme") },
            { value: 2, description: globalization.getText("IdAlterado") },
            { value: 3, description: globalization.getText("IdPendente") },
            { value: 4, description: globalization.getText("IdRevisado") }
        ],
        tipoArquivoInventario: [
            //{ value: 0, description: globalization.getText("TipoArquivoInventarioFixedValue0") },
            { value: 1, description: globalization.getText("TipoArquivoInventarioFixedValue1") },
            { value: 2, description: globalization.getText("TipoArquivoInventarioFixedValue2") }
        ],
        tipoCarga: [
            { value: "true", description: globalization.getText("TipoCargaFixedValueTrue") },
            { value: "false", description: globalization.getText("TipoCargaFixedValueFalse") },
        ],
        tipoStatus: [
            { value: 1, description: globalization.getText("yes") },
            { value: 0, description: globalization.getText("no") },
            { value: 2, description: globalization.getText("all") }
        ],
        tipoCaixaFornecedor: [
            { value: "V", description: globalization.getText("TipoCaixaFornecedorFixedValueV") },
            { value: "F", description: globalization.getText("TipoCaixaFornecedorFixedValueF") }
        ],
        tipoSimNao: [
            { value: true, description: globalization.getText("tpYesNoFixedValuetrue") },
            { value: false, description: globalization.getText("tpYesNoFixedValuefalse") }
        ],
        tipoSimVariavelNaoFixo: [
            { value: true, description: globalization.getText("yesVariable") },
            { value: false, description: globalization.getText("noFixed") }
        ],
        tipoCaixaFornecedorFixoVariavel: [
            { value: "V", description: globalization.getText("TipoCaixaFornecedorFixedValueVariavel") },
            { value: "F", description: globalization.getText("TipoCaixaFornecedorFixedValueFixo") }
        ],
        tipoOrigemImportacao: [
            { value: 0, description: globalization.getText("TipoOrigemImportacaoFixedValue0") },
            { value: 1, description: globalization.getText("TipoOrigemImportacaoFixedValue1") },
        ],
        processOrderState: [
            { value: 0, description: globalization.getText("ProcessOrderStateFixedValueCreated") },
            { value: 1, description: globalization.getText("ProcessOrderStateFixedValueError") },
            { value: 2, description: globalization.getText("ProcessOrderStateFixedValueQueued") },
            { value: 3, description: globalization.getText("ProcessOrderStateFixedValueIsExecuting") },
            { value: 4, description: globalization.getText("ProcessOrderStateFixedValueFailed") },
            { value: 5, description: globalization.getText("ProcessOrderStateFixedValueFinished") },
            { value: 6, description: globalization.getText("ProcessOrderStateFixedValueResultsAvailable") }/*.
            { value: 7, description: globalization.getText("ProcessOrderStateFixedValueResultsExpunged") },*/
        ],
        tipoCaixaFornecedorShort: [
            { value: "V", description: globalization.getText("QtVendorPackageUnit") },
            { value: "F", description: globalization.getText("QtVendorPackageBox") }
        ]
    };

    window.sgpFixedValues.init();

})();
