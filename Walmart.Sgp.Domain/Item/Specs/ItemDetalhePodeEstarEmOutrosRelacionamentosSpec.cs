using System;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Item.Specs
{
    /// <summary>
    /// Especificação referente a se o item detalhe já tem outros relacionamentos.
    /// </summary>
    public class ItemDetalhePodeEstarEmOutrosRelacionamentosSpec : SpecBase<ItemDetalhe>
    {
        private readonly RelacionamentoItemPrincipal m_relacionamentoItemPrincipal;
        private readonly IItemRelacionamentoService m_service;
        private bool m_utilizadoComoSaida;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemDetalhePodeEstarEmOutrosRelacionamentosSpec"/>.
        /// </summary>
        /// <param name="service">O serviço de item relacionamento.</param>
        /// <param name="relacionamentoItemPrincipal">Entidade de relacionamento principal.</param>
        /// <param name="utilizadoComoSaida">Se será utilizado como saída no relacionamento.</param>
        public ItemDetalhePodeEstarEmOutrosRelacionamentosSpec(IItemRelacionamentoService service, RelacionamentoItemPrincipal relacionamentoItemPrincipal, bool utilizadoComoSaida)
        {
            m_service = service;
            m_relacionamentoItemPrincipal = relacionamentoItemPrincipal;
            m_utilizadoComoSaida = utilizadoComoSaida;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ItemDetalhe target)
        {
            // Bug 4638:Sistema não permite relacionar o item.
            if (!m_utilizadoComoSaida && m_relacionamentoItemPrincipal.TipoRelacionamento == TipoRelacionamento.Receituario)
            {
                return Satisfied();
            }

            if (m_service.ContarItemDetalheComoSaidaEmOutrosRelacionamentos(m_relacionamentoItemPrincipal.Id, target.IDItemDetalhe) > 0)
            {
                return NotSatisfied(Texts.ItemIsInAnotherRelationship.With(m_utilizadoComoSaida ? Texts.OutputItem : Texts.InputItem));
            }

            return Satisfied();
        }
    }
}