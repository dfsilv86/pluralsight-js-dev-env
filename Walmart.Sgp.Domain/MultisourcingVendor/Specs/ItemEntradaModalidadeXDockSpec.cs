using System;
using System.Linq;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: Item de Entrada não é modalidade XDock.
    /// </summary>
    public class ItemEntradaModalidadeXDockSpec : MultisourcingSpecBase<long>
    {
        /// <summary>
        /// O delegate que obtém o TipoReabastecimento da tabela ReabastecimentoItemFornecedorCD.
        /// </summary>
        private Func<Multisourcing, ValorTipoReabastecimento> m_obterValorTipoReabastecimento;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemEntradaModalidadeXDockSpec"/>.
        /// </summary>
        /// <param name="obterValorTipoReabastecimento">O delegate que obtém o ValorTipoReabastecimento.</param>
        public ItemEntradaModalidadeXDockSpec(Func<Multisourcing, ValorTipoReabastecimento> obterValorTipoReabastecimento)
        {
            m_obterValorTipoReabastecimento = obterValorTipoReabastecimento;
        }

        /// <summary>
        /// A chave de agrupamento.
        /// </summary>
        protected override Func<Multisourcing, long> Key
        {
            get { return m => m.CdItemDetalheEntrada; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InputItemIsNotModalityXdock; }
        }

        /// <summary>
        /// Verifica se o grupo está ok.
        /// </summary>
        /// <param name="group">O grupo de multisourcings.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<long, Multisourcing> group)
        {
            var multisourcing = group.First();

            if (!ValidarParametros(multisourcing))
            {
                return true;
            }

            var valorTipoReabastecimento = m_obterValorTipoReabastecimento(multisourcing);

            if (valorTipoReabastecimento == ValorTipoReabastecimento.CrossDocking3
                || valorTipoReabastecimento == ValorTipoReabastecimento.CrossDocking33
                || valorTipoReabastecimento == ValorTipoReabastecimento.CrossDocking94)
            {
                return true;
            }

            return false;
        }

        private static bool ValidarParametros(Multisourcing multisourcing)
        {
            return multisourcing.ItemDetalheEntrada != null 
                && multisourcing.ItemDetalheEntrada.IDItemDetalhe != 0
                && multisourcing.IDCD != 0;
        }
    }
}