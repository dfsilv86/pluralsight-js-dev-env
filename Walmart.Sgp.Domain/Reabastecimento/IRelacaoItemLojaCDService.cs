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
        /// Verifica se uma loja � atendida por um CD.
        /// </summary>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se uma loja � atendida por um CD.</returns>
        bool VerificaLojaAtendeCD(long cdLoja, long cdCD, long cdSistema);

        /// <summary>
        /// Verifica se um CD existe e � ativo.
        /// </summary>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se o CD existe e est� ativo.</returns>
        bool VerificaCDExistente(long cdCD, long cdSistema);

        /// <summary>
        /// Verifica se uma loja existe e � ativa.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se a loja existe e est� ativa.</returns>
        bool VerificaLojaExistente(long cdLoja, long cdSistema);

        /// <summary>
        /// Obt�m os RelacaoItemLojaCD por dados de vinculo.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja.</param>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Os RelacaoItemLojaCD.</returns>
        IEnumerable<RelacaoItemLojaCD> ObterPorVinculo(long cdLoja, long cdItemSaida, long cdSistema);

        /// <summary>
        /// Verifica se item que faz parte de uma XREF, n�o � um item prime
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do Sistema.</param>
        /// <returns>Retorna entidade contendo as informa��es de item prime.</returns>
        RelacaoItemLojaCDXrefItemPrime ItemXrefPrime(long cdItem, long cdCD, long cdLoja, long cdSistema);

        /// <summary>
        /// Verifica se item faz parte de uma XREF, � um item prime e existe item staple secund�rio na mesma XREF
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do Sistema.</param>
        /// <returns>Retorna true se o item faz parte de uma XREF, � um item prime e existe item staple secund�rio na mesma XREF </returns>
        bool ItemPossuiItensXrefSecundarios(long cdItem, long cdCD, long cdLoja, long cdSistema);

        /// <summary>
        /// Metodo para Obter a listagem de RelacaoItemLojaCD
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">Pagina��o do resultado</param>
        /// <returns>Lista de relacao item loja cd</returns>
        IEnumerable<RelacaoItemLojaCDConsolidado> ObterPorFiltro(RelacaoItemLojaCDFiltro filtro, Paging paging);

        /// <summary>
        /// Remove o relacionamento de um item de entrada (se existir) e loga as altera��es.
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
        /// <returns>True se j� possui cadastro.</returns>
        bool ItemSaidaPossuiCadastro(long cdItemSaida, long cdCD, long cdLoja, long cdSistema);

        /// <summary>
        /// Verifica se a loja e o CD possuem cadastro para o item de sa�da.
        /// </summary>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se j� possuem cadastro.</returns>
        bool LojaCDPossuiCadastroItemControleEstoque(long cdItemSaida, long cdCD, long cdLoja, long cdSistema);

        /// <summary>
        /// Salva uma lista de RelacaoItemLojaCD.
        /// </summary>
        /// <param name="entidades">Lista de RelacaoItemLojaCD.</param>
        /// <param name="idUsuario">Id do usu�rio logado.</param>
        void SalvarVinculos(IEnumerable<RelacaoItemLojaCDVinculo> entidades, int idUsuario);

        /// <summary>
        /// Desvincular uma lista de RelacaoItemLojaCD.
        /// </summary>
        /// <param name="desvinculos">Lista de RelacaoItemLojaCD.</param>
        /// <param name="idUsuario">Id do usu�rio logado.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        void Desvincular(IEnumerable<RelacaoItemLojaCDVinculo> desvinculos, int idUsuario, long cdSistema);

        /// <summary>
        /// Obt�m todos os processamentos de importacao de vinculo/desvinculo.
        /// </summary>
        /// <param name="createdUserId">O id do usu�rio.</param>
        /// <param name="processName">O nome do processo.</param>
        /// <param name="state">A situa��o do processamento.</param>
        /// <param name="paging">A pagina��o.</param>
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
        /// Obt�m o tipo reabastecimento de um item vinculado a uma Xref, prime, no caso de cd convertido.
        /// </summary>
        /// <param name="cdItem">O c�digo do item.</param>
        /// <param name="cdLoja">O c�digo da loja.</param>
        /// <param name="cdSistema">O c�digo do sistema.</param>
        /// <param name="cdcd">O c�digo do CD.</param>
        /// <returns>O tipo de reabastecimento caso o item atenda ao cen�rio, ou null caso contr�rio.</returns>
        ValorTipoReabastecimento ObterTipoReabastecimentoItemVinculadoXrefPrime(long cdItem, int cdLoja, int cdSistema, int cdcd);

        /// <summary>
        /// Verifica se o item � de sa�da (tpVinculado=S) e se pode ser vinculado (deve ser Staple, Prime primario, e possuir itens secundarios na mesma xref).
        /// </summary>
        /// <param name="cdItem">O c�digo do item.</param>
        /// <param name="cdLoja">O c�digo da loja.</param>
        /// <param name="cdSistema">O c�digo do sistema.</param>
        /// <param name="cdcd">O c�digo do CD.</param>
        /// <returns>Se o item � de sa�da e se pode ser vinculado, ou null caso n�o seja de sa�da.</returns>
        bool? ObterItemSaidaAtendeRequisitos(long cdItem, int cdLoja, int cdSistema, int cdcd);
    }
}
