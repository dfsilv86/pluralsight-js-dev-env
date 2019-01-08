using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.MultisourcingVendor
{
    /// <summary>
    ///  Serviço de log de Multisourcing.
    /// </summary>
    public class LogMultiSourcingService : EntityDomainServiceBase<LogMultiSourcing, ILogMultiSourcingGateway>, ILogMultiSourcingService
    {
        private readonly IMultisourcingGateway m_multisourcingGateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LogMultiSourcingService"/>.
        /// </summary>
        /// <param name="mainGateway">O gateway.</param>
        /// <param name="multisourcingGateway">O gateway Multisourcing.</param>
        public LogMultiSourcingService(ILogMultiSourcingGateway mainGateway, IMultisourcingGateway multisourcingGateway)
            : base(mainGateway)
        {
            m_multisourcingGateway = multisourcingGateway;
        }

        /// <summary>
        /// Logar evento de multisourcing
        /// </summary>
        /// <param name="acao">A ação (Inserir/Excluir/Alterar).</param>
        /// <param name="msAnterior">O multisourcing original.</param>
        /// <param name="msPosterior">O multisourcing alterado.</param>
        /// <param name="observacao">Uma observação.</param>
        public void Logar(TpOperacao acao, Multisourcing msAnterior, Multisourcing msPosterior, string observacao)
        {
            Assert(new { Acao = acao }, new AllMustBeInformedSpec());

            Assert(new { MsAnterior = msAnterior, MsPosterior = msPosterior }, new AtLeastOneMustBeInformedSpec());

            var log = new LogMultiSourcing();

            if (msAnterior != null)
            {
                log.PercAnterior = msAnterior.vlPercentual;
                log.IdCd = msAnterior.IDCD;
                log.IdUsuario = msAnterior.cdUsuarioAlteracao;
            }

            if (msPosterior != null)
            {
                log.PercPosterior = msPosterior.vlPercentual;
                log.IdCd = msPosterior.IDCD;
                log.IdUsuario = msPosterior.cdUsuarioAlteracao;
            }

            log.Data = DateTime.Now;
            log.Observacao = observacao;
            log.TpOperacao = ((char)acao).ToString();

            PopulaParametrosLog(msAnterior, msPosterior, log);

            this.MainGateway.Insert(log);
        }

        private void PopulaParametrosLog(Multisourcing msAnterior, Multisourcing msPosterior, LogMultiSourcing log)
        {
            if (msPosterior != null)
            {
                BuscaParametros(msPosterior, log);
            }
            else if (msAnterior != null)
            {
                BuscaParametros(msAnterior, log);
            }
        }

        private void BuscaParametros(Multisourcing ms, LogMultiSourcing log)
        {
            if (ms.ItemDetalheEntrada == null && ms.ItemDetalheSaida == null)
            {
                var m = this.m_multisourcingGateway.ObterComItemDetalhesEFp(ms.IDMultisourcing);

                var fp = m.Fornecedor.Parametros.FirstOrDefault();
                if (fp != null)
                {
                    log.IdFornecedorParametro = fp.IDFornecedorParametro;
                }

                log.IdItemDetalheEntrada = m.ItemDetalheEntrada.IDItemDetalhe;
                log.IdItemDetalheSaida = m.ItemDetalheSaida.IDItemDetalhe;
            }
            else
            {
                log.IdFornecedorParametro = ms.Fornecedor.Parametros.First().IDFornecedorParametro;
                log.IdItemDetalheEntrada = ms.ItemDetalheEntrada.IDItemDetalhe;
                log.IdItemDetalheSaida = ms.ItemDetalheSaida.IDItemDetalhe;
            }
        }
    }
}
