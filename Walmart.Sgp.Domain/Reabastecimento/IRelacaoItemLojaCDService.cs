using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Classe de interface de RelacaoItemLojaCDService
    /// </summary>
    public interface IRelacaoItemLojaCDService : IDomainService<RelacaoItemLojaCD>
    {
        /// <summary>
        /// Verifica se uma loja é atendida por um CD.
        /// </summary>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se uma loja é atendida por um CD.</returns>
        bool VerificaLojaAtendeCD(long cdLoja, long cdCD, long cdSistema);

        /// <summary>
        /// Verifica se um CD existe e é ativo.
        /// </summary>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se o CD existe e está ativo.</returns>
        bool VerificaCDExistente(long cdCD, long cdSistema);

        /// <summary>
        /// Verifica se uma loja existe e é ativa.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se a loja existe e está ativa.</returns>
        bool VerificaLojaExistente(long cdLoja, long cdSistema);

        /// <summary>
        /// Obtém os RelacaoItemLojaCD por dados de vinculo.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja.</param>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Os RelacaoItemLojaCD.</returns>
        IEnumerable<RelacaoItemLojaCD> ObterPorVinculo(long cdLoja, long cdItemSaida, long cdSistema);

        /// <summary>
        /// Verifica se item que faz parte de uma XREF, não é um item prime
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do Sistema.</param>
        /// <returns>Retorna entidade contendo as informações de item prime.</returns>
        RelacaoItemLojaCDXrefItemPrime ItemXrefPrime(long cdItem, long cdCD, long cdLoja, long cdSistema);

        /// <summary>
        /// Verifica se item faz parte de uma XREF, é um item prime e existe item staple secundário na mesma XREF
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do Sistema.</param>
        /// <returns>Retorna true se o item faz parte de uma XREF, é um item prime e existe item staple secundário na mesma XREF </returns>
        bool ItemPossuiItensXrefSecundarios(long cdItem, long cdCD, long cdLoja, long cdSistema);

        /// <summary>
        /// Metodo para Obter a listagem de RelacaoItemLojaCD
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">Paginação do resultado</param>
        /// <returns>Lista de relacao item loja cd</returns>
        IEnumerable<RelacaoItemLojaCDConsolidado> ObterPorFiltro(RelacaoItemLojaCDFiltro filtro, Paging paging);

        /// <summary>
        /// Remove o relacionamento de um item de entrada (se existir) e loga as alterações.
        /// </summary>
        /// <param name="idItemEntrada">O id do item a ser removido.</param>
        /// <param name="exclusaoCompraCasada">Indica se esta excluindo uma compra casada.</param>
        void RemoverRelacionamentoPorItemEntrada(int idItemEntrada, bool exclusaoCompraCasada);

        /// <summary>
        /// Verifica se um item de saida possui loja cadastrada.
        /// </summary>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se já possui cadastro.</returns>
        bool ItemSaidaPossuiCadastro(long cdItemSaida, long cdCD, long cdLoja, long cdSistema);

        /// <summary>
        /// Verifica se a loja e o CD possuem cadastro para o item de saída.
        /// </summary>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se já possuem cadastro.</returns>
        bool LojaCDPossuiCadastroItemControleEstoque(long cdItemSaida, long cdCD, long cdLoja, long cdSistema);

        /// <summary>
        /// Salva uma lista de RelacaoItemLojaCD.
        /// </summary>
        /// <param name="entidades">Lista de RelacaoItemLojaCD.</param>
        /// <param name="idUsuario">Id do usuário logado.</param>
        void SalvarVinculos(IEnumerable<RelacaoItemLojaCDVinculo> entidades, int idUsuario);

        /// <summary>
        /// Desvincular uma lista de RelacaoItemLojaCD.
        /// </summary>
        /// <param name="desvinculos">Lista de RelacaoItemLojaCD.</param>
        /// <param name="idUsuario">Id do usuário logado.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        void Desvincular(IEnumerable<RelacaoItemLojaCDVinculo> desvinculos, int idUsuario, long cdSistema);

        /// <summary>
        /// Obtém todos os processamentos de importacao de vinculo/desvinculo.
        /// </summary>
        /// <param name="createdUserId">O id do usuário.</param>
        /// <param name="processName">O nome do processo.</param>
        /// <param name="state">A situação do processamento.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Os processamentos registrados.
        /// </returns>
        IEnumerable<ProcessOrderModel> ObterProcessamentosImportacao(int? createdUserId, string processName, ProcessOrderState? state, Paging paging);

        /// <summary>
        /// Verifica se dois itens possuem relacionamento.
        /// </summary>
        /// <param name="cdItemEntrada">O codigo do item de entrada.</param>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se os itens possuem relacionamento.</returns>
        bool PossuiRelacionamentoSGP(long cdItemEntrada, long cdItemSaida, long cdSistema);

        /// <summary>
        /// Obtém o tipo reabastecimento de um item vinculado a uma Xref, prime, no caso de cd convertido.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdcd">O código do CD.</param>
        /// <returns>O tipo de reabastecimento caso o item atenda ao cenário, ou null caso contrário.</returns>
        ValorTipoReabastecimento ObterTipoReabastecimentoItemVinculadoXrefPrime(long cdItem, int cdLoja, int cdSistema, int cdcd);

        /// <summary>
        /// Verifica se o item é de saída (tpVinculado=S) e se pode ser vinculado (deve ser Staple, Prime primario, e possuir itens secundarios na mesma xref).
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdcd">O código do CD.</param>
        /// <returns>Se o item é de saída e se pode ser vinculado, ou null caso não seja de saída.</returns>
        bool? ObterItemSaidaAtendeRequisitos(long cdItem, int cdLoja, int cdSistema, int cdcd);
    }
}
