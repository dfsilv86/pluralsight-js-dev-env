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
    /// Classe de interface de RelacoaItemLojaCDGateway
    /// </summary>
    public interface IRelacaoItemLojaCDGateway : IDataGateway<RelacaoItemLojaCD>
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
        /// Verifica se dois itens possuem relacionamento.
        /// </summary>
        /// <param name="cdItemEntrada">O codigo do item de entrada.</param>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se os itens possuem relacionamento.</returns>
        bool PossuiRelacionamentoSGP(long cdItemEntrada, long cdItemSaida, long cdSistema);

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
        /// Metodo para Obter a listagem de RelacaoItemLojaCD
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">Paginação do resultado</param>
        /// <returns>Lista RelacaoItemLojaCD</returns>
        IEnumerable<RelacaoItemLojaCDConsolidado> ObterPorFiltro(RelacaoItemLojaCDFiltro filtro, Paging paging);

        /// <summary>
        /// Metodo para obter os dados para a exportação do cadastro de reabastecimentio item/loja.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Lista de RelacaoItemLojaCD</returns>
        IEnumerable<RelacaoItemLojaCDConsolidado> ObterDadosExportacaoCadastro(RelacaoItemLojaCDFiltro filtro);

        /// <summary>
        /// Obtém uma RelacaoItemLojaCD;
        /// </summary>
        /// <param name="cdCD"> O código do CD.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="cdItemDetalheEntrada">O código do item de entrada.</param>
        /// <param name="cdItemDetalheSaida">O código do item de saída.</param>
        /// <returns>Retorna uma RelacaoItemLojaCD</returns>
        RelacaoItemLojaCD ObterPorFiltroConsiderandoXRef(long cdCD, long cdLoja, long cdItemDetalheEntrada, long cdItemDetalheSaida);

        /// <summary>
        /// Obtém os RelacaoItemLojaCD por dados de vinculo.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja.</param>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Os RelacaoItemLojaCD.</returns>
        IEnumerable<RelacaoItemLojaCD> ObterPorVinculo(long cdLoja, long cdItemSaida, long cdSistema);

        /// <summary>
        /// Obtém todos os processamentos de importacao de vinculo/desvinculo.
        /// </summary>
        /// <param name="currentUserId">O id do usuário efetuando a consulta.</param>
        /// <param name="isAdministrator">Se o usuário efetuando a consulta é administrador.</param>
        /// <param name="createdUserId">O id do usuário.</param>
        /// <param name="processName">O nome do processo.</param>
        /// <param name="state">A situação do processamento.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>
        /// Os processamentos registrados.
        /// </returns>
        IEnumerable<ProcessOrderModel> ObterProcessamentosImportacao(int currentUserId, bool isAdministrator, int? createdUserId, string processName, ProcessOrderState? state, Paging paging);

        /// <summary>
        /// Registra LOG para RelacaoItemLojaCD de acordo com os parametros informados.
        /// </summary>
        /// <param name="log">Informações para salvar no log.</param>
        void RegistraLogRelacaoItemLojaCD(LogRelacaoItemLojaCD log);

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
