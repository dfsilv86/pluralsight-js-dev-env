using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para relacionamento item principal em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryItemRelacionamentoGateway : MemoryDataGateway<RelacionamentoItemPrincipal>, IItemRelacionamentoGateway
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
        public IEnumerable<RelacionamentoItemPrincipal> PesquisarPorTipoRelacionamento(TipoRelacionamento tipoRelacionamento, string dsItem, int? cdItem, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int cdSistema, int? idRegiaoCompra, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determina o percentual de rendimento transformado.
        /// </summary>
        /// <param name="idItemDetalhe">Id do item detalhe.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O percentual.</returns>
        /// <remarks>Determinado a partir da tabela RelacionamentoItemPrincipal.</remarks>
        public decimal? ObterPercentualRendimentoTransformado(int idItemDetalhe, byte cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determina o percentual de rendimento derivado.
        /// </summary>
        /// <param name="idItemDetalhe">Id do item detalhe.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O percentual.</returns>
        /// <remarks>Determinado a partir da tabela RelacionamentoItemSecundario.</remarks>
        public decimal? ObterPercentualRendimentoDerivado(int idItemDetalhe, byte cdSistema)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Conta o número de vezes que um item detalhe foi utilizado como saída em outros relacionamentos.
        /// </summary>
        /// <param name="idRelacionamentoItemPrincipalCorrente">O id do relacionamento item principal corrente.</param>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>A contagem.</returns>
        public int ContarItemDetalheComoSaidaEmOutrosRelacionamentos(int idRelacionamentoItemPrincipalCorrente, int idItemDetalhe)
        {
            return 0;
        }

        /// <summary>
        /// Obtém os relacionamentos onde o item participa como principal.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>
        /// Os relacionamentos principais onde o item é o principal, e seus secundários.
        /// </returns>
        public virtual IEnumerable<RelacionamentoItemPrincipal> ObterPrincipaisPorItem(int idItemDetalhe)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Conta o número de vezes que um item detalhe foi utilizado em outros relacionamentos.
        /// </summary>
        /// <param name="idRelacionamentoItemPrincipalCorrente">O id do relacionamento item principal corrente.</param>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="tipoRelacionamento">O tipo de relacionamento a ser considerado.</param>
        /// <returns>A contagem.</returns>
        public int ContarItemDetalheEmOutrosRelacionamentos(int idRelacionamentoItemPrincipalCorrente, int idItemDetalhe, TipoRelacionamento tipoRelacionamento)
        {
            return 0;
        }

        /// <summary>
        /// Pesquisa informações sobre itens relacionados ao item informado.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>Informações sobre itens relacionados.</returns>
        public ItensRelacionadosResponse ObterItensRelacionados(int cdItem, int? idLoja)
        {
            throw new NotImplementedException();
        }
    }
}