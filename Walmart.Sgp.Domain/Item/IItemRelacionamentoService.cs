using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Define a interface de um serviço de relacionamento de itens.
    /// </summary>
    public interface IItemRelacionamentoService : IDomainService<RelacionamentoItemPrincipal>
    {
        /// <summary>
        /// Pesquisa relacionamentos por informações dos itens, departamento e sistema.
        /// </summary>
        /// <param name="tipoRelacionamento">O tipo de relacionamento.</param>
        /// <param name="dsItem">Descrição do item.</param>
        /// <param name="cdItem">O código do item (cdItem).</param>
        /// <param name="cdFineLine">O código do fineline.</param>
        /// <param name="cdSubcategoria">O código da subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os relacionamentos (item principal e itens secundários).</returns>
        IEnumerable<RelacionamentoItemPrincipal> PesquisarPorTipoRelacionamento(TipoRelacionamento tipoRelacionamento, string dsItem, int? cdItem, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int cdSistema, int? idRegiaoCompra, Paging paging);

        /// <summary>
        /// Determina o percentual de rendimento transformado.
        /// </summary>
        /// <param name="idItemDetalhe">Id do item detalhe.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O percentual.</returns>
        /// <remarks>Determinado a partir da tabela RelacionamentoItemPrincipal.</remarks>
        decimal? ObterPercentualRendimentoTransformado(int idItemDetalhe, byte cdSistema);

        /// <summary>
        /// Determina o percentual de rendimento derivado.
        /// </summary>
        /// <param name="idItemDetalhe">Id do item detalhe.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O percentual.</returns>
        /// <remarks>Determinado a partir da tabela RelacionamentoItemSecundario.</remarks>
        decimal? ObterPercentualRendimentoDerivado(int idItemDetalhe, byte cdSistema);

        /// <summary>
        /// Conta o número de vezes que um item detalhe foi utilizado como saída em outros relacionamentos.
        /// </summary>
        /// <param name="idRelacionamentoItemPrincipalCorrente">O id do relacionamento item principal corrente.</param>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>A contagem.</returns>
        int ContarItemDetalheComoSaidaEmOutrosRelacionamentos(int idRelacionamentoItemPrincipalCorrente, int idItemDetalhe);

        /// <summary>
        /// Pesquisa informações sobre itens relacionados ao item informado.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>Informações sobre itens relacionados.</returns>
        ItensRelacionadosResponse ObterItensRelacionados(int cdItem, int? idLoja);
    }
}
