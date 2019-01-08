using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Classe de service do RelacaoItemLojaCD
    /// </summary>
    public class RelacaoItemLojaCDService : EntityDomainServiceBase<RelacaoItemLojaCD, IRelacaoItemLojaCDGateway>, IRelacaoItemLojaCDService
    {
        private readonly ILojaCdParametroGateway m_lojaCdParametroGateway;

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RelacaoItemLojaCDService"/>
        /// </summary>
        /// <param name="mainGateway">O table data gateway principal.</param>
        /// <param name="lojaCdParametroGateway">O table data gateway para LojaCdParametro.</param>
        public RelacaoItemLojaCDService(IRelacaoItemLojaCDGateway mainGateway, ILojaCdParametroGateway lojaCdParametroGateway)
            : base(mainGateway)
        {
            this.m_lojaCdParametroGateway = lojaCdParametroGateway;
        }
        #endregion

        /// <summary>
        /// Verifica se uma loja é atendida por um CD.
        /// </summary>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se uma loja é atendida por um CD.</returns>
        public bool VerificaLojaAtendeCD(long cdLoja, long cdCD, long cdSistema)
        {
            Assert(new { Store = cdLoja, CD = cdCD, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.VerificaLojaAtendeCD(cdLoja, cdCD, cdSistema);
        }

        /// <summary>
        /// Verifica se um CD existe e é ativo.
        /// </summary>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se o CD existe e está ativo.</returns>
        public bool VerificaCDExistente(long cdCD, long cdSistema)
        {
            Assert(new { CD = cdCD, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.VerificaCDExistente(cdCD, cdSistema);
        }

        /// <summary>
        /// Verifica se uma loja existe e é ativa.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Se a loja existe e está ativa.</returns>
        public bool VerificaLojaExistente(long cdLoja, long cdSistema)
        {
            Assert(new { Store = cdLoja, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.VerificaLojaExistente(cdLoja, cdSistema);
        }

        /// <summary>
        /// Obter a lista de RElacaoItemLojaCD
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">Paginação do resultado</param>
        /// <returns>relacao item loja cd</returns>
        public IEnumerable<RelacaoItemLojaCDConsolidado> ObterPorFiltro(RelacaoItemLojaCDFiltro filtro, Paging paging)
        {
            Assert(new { ItemCode = filtro.cdItemSaida }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorFiltro(filtro, paging);
        }

        /// <summary>
        /// Salva a entidade informada
        /// </summary>
        /// <param name="entidade">A entidade a ser salva.</param>
        public override void Salvar(RelacaoItemLojaCD entidade)
        {
            var oldEntity = this.MainGateway.FindById(entidade.Id);
            RegistrarLogRelacaoItemLojaCD(oldEntity, entidade, 26, null); // Id 26 = Cadastro Manual de Reabastecimento Item/Loja

            entidade.Stamp();

            this.MainGateway.Update(@"idItemEntrada = @idItemEntrada, cdUsuarioAtualizacao = @cdUsuarioAtualizacao, dhAtualizacao = @dhAtualizacao, vlTipoReabastecimento = @vlTipoReabastecimento, cdCrossRef = @cdCrossRef", entidade);
        }

        /// <summary>
        /// Salva uma lista de RelacaoItemLojaCD.
        /// </summary>
        /// <param name="entidades">Lista de RelacaoItemLojaCD.</param>
        /// <param name="idUsuario">Id do usuário logado.</param>
        public void SalvarVinculos(IEnumerable<RelacaoItemLojaCDVinculo> entidades, int idUsuario)
        {
            foreach (var item in entidades)
            {
                var relacaoItemLojaCD = this.MainGateway.ObterPorFiltroConsiderandoXRef(item.CdCD, item.CdLoja, item.CdItemDetalheEntrada, item.CdItemDetalheSaida);

                if (relacaoItemLojaCD != null && relacaoItemLojaCD.IDRelacaoItemLojaCD != 0)
                {
                    var oldEntity = this.MainGateway.FindById(relacaoItemLojaCD.IDRelacaoItemLojaCD);

                    relacaoItemLojaCD.DhAtualizacao = DateTime.Now;
                    relacaoItemLojaCD.CdUsuarioAtualizacao = idUsuario;
                    relacaoItemLojaCD.blAtivo = true;

                    this.MainGateway.Update(@"idItemEntrada = @IdItemEntrada, vlTipoReabastecimento = @VlTipoReabastecimento, cdCrossRef = @CdCrossRef, dhAtualizacao = @DhAtualizacao, cdUsuarioAtualizacao = @CdUsuarioAtualizacao, blAtivo = @blAtivo", relacaoItemLojaCD);

                    RegistrarLogRelacaoItemLojaCD(oldEntity, relacaoItemLojaCD, 24, null);
                }
            }
        }

        /// <summary>
        /// Remove o relacionamento de um item de entrada (se existir) e loga as alterações.
        /// </summary>
        /// <param name="idItemEntrada">O id do item a ser removido.</param>
        /// <param name="exclusaoCompraCasada">Indica se esta excluindo uma compra casada.</param>
        public void RemoverRelacionamentoPorItemEntrada(int idItemEntrada, bool exclusaoCompraCasada)
        {
            Assert(new { InputItemOnly = idItemEntrada }, new AllMustBeInformedSpec());

            var relacionamentos = this.MainGateway.Find(
                "idItemEntrada = @idItemEntrada",
                new { idItemEntrada = idItemEntrada });

            foreach (var relacionamento in relacionamentos)
            {
                var oldEntity = this.MainGateway.FindById(relacionamento.IDRelacaoItemLojaCD);

                relacionamento.VlTipoReabastecimento = null;
                relacionamento.CdCrossRef = null;
                relacionamento.IdItemEntrada = null;

                this.MainGateway.Update(relacionamento);

                RegistrarLogRelacaoItemLojaCD(oldEntity, relacionamento, 27, exclusaoCompraCasada ? Texts.InputItemIsntParentCCAnymore : Texts.InputItemChildCC);
            }
        }

        /// <summary>
        /// Verifica se um item de saida possui loja cadastrada.
        /// </summary>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se já possui cadastro.</returns>
        public bool ItemSaidaPossuiCadastro(long cdItemSaida, long cdCD, long cdLoja, long cdSistema)
        {
            Assert(new { Item = cdItemSaida, CD = cdCD, Store = cdLoja, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ItemSaidaPossuiCadastro(cdItemSaida, cdCD, cdLoja, cdSistema);
        }

        /// <summary>
        /// Verifica se a loja e o CD possuem cadastro para o item de saída.
        /// </summary>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se já possuem cadastro.</returns>
        public bool LojaCDPossuiCadastroItemControleEstoque(long cdItemSaida, long cdCD, long cdLoja, long cdSistema)
        {
            Assert(new { Item = cdItemSaida, CD = cdCD, Store = cdLoja, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.LojaCDPossuiCadastroItemControleEstoque(cdItemSaida, cdCD, cdLoja, cdSistema);
        }

        /// <summary>
        /// Verifica se item faz parte de uma XREF, é um item prime e existe item staple secundário na mesma XREF
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do Sistema.</param>
        /// <returns>Retorna true se o item faz parte de uma XREF, é um item prime e existe item staple secundário na mesma XREF </returns>
        public bool ItemPossuiItensXrefSecundarios(long cdItem, long cdCD, long cdLoja, long cdSistema)
        {
            Assert(new { Item = cdItem, CD = cdCD, Store = cdLoja, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ItemPossuiItensXrefSecundarios(cdItem, cdCD, cdLoja, cdSistema);
        }

        /// <summary>
        /// Verifica se item que faz parte de uma XREF, não é um item prime
        /// </summary>
        /// <param name="cdItem">O codigo do item.</param>
        /// <param name="cdCD">O codigo do CD.</param>
        /// <param name="cdLoja">O codigo da Loja.</param>
        /// <param name="cdSistema">O codigo do Sistema.</param>
        /// <returns>Retorna entidade contendo as informações de item prime.</returns>
        public RelacaoItemLojaCDXrefItemPrime ItemXrefPrime(long cdItem, long cdCD, long cdLoja, long cdSistema)
        {
            Assert(new { Item = cdItem, CD = cdCD, Store = cdLoja, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ItemXrefPrime(cdItem, cdCD, cdLoja, cdSistema);
        }

        /// <summary>
        /// Obtém os RelacaoItemLojaCD por dados de vinculo.
        /// </summary>
        /// <param name="cdLoja">O codigo da loja.</param>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>Os RelacaoItemLojaCD.</returns>
        public IEnumerable<RelacaoItemLojaCD> ObterPorVinculo(long cdLoja, long cdItemSaida, long cdSistema)
        {
            Assert(new { OutputItem = cdItemSaida, Store = cdLoja, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterPorVinculo(cdLoja, cdItemSaida, cdSistema);
        }

        /// <summary>
        /// Desvincular uma lista de RelacaoItemLojaCD.
        /// </summary>
        /// <param name="desvinculos">Lista de RelacaoItemLojaCD.</param>
        /// <param name="idUsuario">Id do usuário logado.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        public void Desvincular(IEnumerable<RelacaoItemLojaCDVinculo> desvinculos, int idUsuario, long cdSistema)
        {
            Assert(new { User = idUsuario }, new AllMustBeInformedSpec());

            foreach (var desvinculo in desvinculos)
            {
                DesvincularRelacionamentos(cdSistema, idUsuario, desvinculo);
            }
        }

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
        public IEnumerable<ProcessOrderModel> ObterProcessamentosImportacao(int? createdUserId, string processName, ProcessOrderState? state, Paging paging)
        {
            Assert(new { processName }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterProcessamentosImportacao(RuntimeContext.Current.User.Id, RuntimeContext.Current.User.IsAdministrator, createdUserId, processName, state, paging);
        }

        /// <summary>
        /// Verifica se dois itens possuem relacionamento.
        /// </summary>
        /// <param name="cdItemEntrada">O codigo do item de entrada.</param>
        /// <param name="cdItemSaida">O codigo do item de saida.</param>
        /// <param name="cdSistema">O codigo do sistema.</param>
        /// <returns>True se os itens possuem relacionamento.</returns>
        public bool PossuiRelacionamentoSGP(long cdItemEntrada, long cdItemSaida, long cdSistema)
        {
            Assert(new { Item = cdItemEntrada, OutputItem = cdItemSaida, System = cdSistema }, new AllMustBeInformedSpec());

            return this.MainGateway.PossuiRelacionamentoSGP(cdItemEntrada, cdItemSaida, cdSistema);
        }

        /// <summary>
        /// Obtém o tipo reabastecimento de um item vinculado a uma Xref, prime, no caso de cd convertido.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdcd">O código do CD.</param>
        /// <returns>O tipo de reabastecimento caso o item atenda ao cenário, ou null caso contrário.</returns>
        public ValorTipoReabastecimento ObterTipoReabastecimentoItemVinculadoXrefPrime(long cdItem, int cdLoja, int cdSistema, int cdcd)
        {
            Assert(new { cdItem, cdLoja, cdSistema, cdcd }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterTipoReabastecimentoItemVinculadoXrefPrime(cdItem, cdLoja, cdSistema, cdcd);
        }

        /// <summary>
        /// Verifica se o item é de saída (tpVinculado=S) e se pode ser vinculado (deve ser Staple, Prime primario, e possuir itens secundarios na mesma xref).
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdcd">O código do CD.</param>
        /// <returns>Se o item é de saída e se pode ser vinculado, ou null caso não seja de saída.</returns>
        public bool? ObterItemSaidaAtendeRequisitos(long cdItem, int cdLoja, int cdSistema, int cdcd)
        {
            Assert(new { cdItem, cdLoja, cdSistema, cdcd }, new AllMustBeInformedSpec());

            return this.MainGateway.ObterItemSaidaAtendeRequisitos(cdItem, cdLoja, cdSistema, cdcd);
        }

        private void DesvincularRelacionamentos(long cdSistema, int idUsuario, RelacaoItemLojaCDVinculo desvinculo)
        {
            foreach (var relacao in this.ObterPorVinculo(desvinculo.CdLoja, desvinculo.CdItemDetalheSaida, cdSistema).Where(item => item.IdItemEntrada != null))
            {
                var oldEntity = this.MainGateway.FindById(relacao.IDRelacaoItemLojaCD);

                relacao.IdItemEntrada = null;
                relacao.CdCrossRef = null;
                relacao.VlTipoReabastecimento = null;
                relacao.DhAtualizacao = DateTime.Now;
                relacao.CdUsuarioAtualizacao = idUsuario;

                this.MainGateway.Update(relacao);

                RegistrarLogRelacaoItemLojaCD(oldEntity, relacao, 25, null);
            }
        }

        private void RegistrarLogRelacaoItemLojaCD(RelacaoItemLojaCD original, RelacaoItemLojaCD modificado, int idProcesso, string observacao)
        {
            var lojaCdParametro = this.m_lojaCdParametroGateway.FindById((int)modificado.IDLojaCDParametro);

            var log = new LogRelacaoItemLojaCD()
            {
                IDLogTipoProcesso = idProcesso,
                IdAuditUser = RuntimeContext.Current.User.Id,
                cdCrossRefAnterior = original.CdCrossRef,
                cdCrossRefNovo = modificado.CdCrossRef,
                IDItemDetalheEntradaAnterior = original.IdItemEntrada,
                IDItemDetalheEntradaNovo = modificado.IdItemEntrada,
                IDItemDetalheSaida = modificado.IDItem,
                vlTipoReabastecimento = modificado.VlTipoReabastecimento,
                IDLoja = lojaCdParametro.IDLoja,
                IDCD = lojaCdParametro.IDCD,
                Observacao = observacao
            };

            this.MainGateway.RegistraLogRelacaoItemLojaCD(log);
        }
    }
}
