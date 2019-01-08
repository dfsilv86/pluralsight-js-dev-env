using System;

namespace Walmart.Sgp.Domain.Movimentacao
{
    /// <summary>
    /// Representa um CustoNotaFiscal.
    /// </summary>
    public class CustoNotaFiscal
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CustoNotaFiscal"/>.
        /// </summary>
        public CustoNotaFiscal()
        {
            //// Bug #4740 - parte 2 - deve manter as alterações entre as paginações
            //// para isso dar certo, os itens não podem vir marcados para liberar por padrão 
            //// (pois senão cria a expectativa que todos os milhares de itens retornados sem filtro seriam carimbados de uma vez)
            //// blLiberar = true;
        }

        /// <summary>
        /// Obtém ou define IDNotaFiscalItem
        /// </summary>
        public int IDNotaFiscalItem { get; set; }

        /// <summary>
        /// Obtém ou define cdLoja
        /// </summary>
        public int cdLoja { get; set; }

        /// <summary>
        /// Obtém ou define dtRecebimento
        /// </summary>
        public DateTime dtRecebimento { get; set; }

        /// <summary>
        /// Obtém ou define dtEmissao
        /// </summary>
        public DateTime dtEmissao { get; set; }

        /// <summary>
        /// Obtém ou define nrNotaFiscal
        /// </summary>
        public long nrNotaFiscal { get; set; }

        /// <summary>
        /// Obtém ou define cdDepartamento
        /// </summary>
        public int cdDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define cddsItem
        /// </summary>
        public string cddsItem { get; set; }

        /// <summary>
        /// Obtém ou define dhLiberacao
        /// </summary>
        public DateTime dhLiberacao { get; set; }

        /// <summary>
        /// Obtém ou define vlCusto
        /// </summary>
        public decimal vlCusto { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoCompraAtual
        /// </summary>
        public decimal vlCustoCompraAtual { get; set; }

        /// <summary>
        /// Obtém ou define VariacaoUltimoCusto
        /// </summary>
        public decimal VariacaoUltimoCusto { get; set; }

        /// <summary>
        /// Obtém ou define qtItem
        /// </summary>
        public int qtItem { get; set; }

        /// <summary>
        /// Obtém ou define qtItemCorrigida
        /// </summary>
        public int qtItemCorrigida { get; set; }

        /// <summary>
        /// Obtém ou define vlMercadoria
        /// </summary>
        public decimal vlMercadoria { get; set; }

        /// <summary>
        /// Obtém ou define QtVendorPackage
        /// </summary>
        public int QtVendorPackage { get; set; }

        /// <summary>
        /// Obtém ou define DsTamanhoItem
        /// </summary>
        public string DsTamanhoItem { get; set; }

        /// <summary>
        /// Obtém ou define vlCustoUnitarioReal
        /// </summary>
        public decimal vlCustoUnitarioReal { get; set; }

        /// <summary>
        /// Obtém ou define VlCustoUnitario
        /// </summary>
        public decimal VlCustoUnitario { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao
        /// </summary>
        public DateTime dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dtLiberacao
        /// </summary>
        public DateTime dtLiberacao { get; set; }

        /// <summary>
        /// Obtém ou define usrNomeAlteracao
        /// </summary>
        public string usrNomeAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define IdNotaFiscalItemStatus
        /// </summary>
        public int IdNotaFiscalItemStatus { get; set; }

        /// <summary>
        /// Obtém ou define IDBandeira
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define IDFornecedor
        /// </summary>
        public int IDFornecedor { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define IDItemDetalhe
        /// </summary>
        public int IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento
        /// </summary>
        public int IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define dtCadastroConcentrador
        /// </summary>
        public DateTime dtCadastroConcentrador { get; set; }

        /// <summary>
        /// Obtém ou define dtAtualizacaoConcentrador
        /// </summary>
        public DateTime dtAtualizacaoConcentrador { get; set; }

        /// <summary>
        /// Obtém ou define blLiberar
        /// </summary>
        public bool blLiberar { get; set; }

        /// <summary>
        /// Obtém ou define qtAjustada
        /// </summary>
        public decimal qtAjustada { get; set; }
    }
}
