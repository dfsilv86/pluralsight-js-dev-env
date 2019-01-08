using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se um agendamento de inventário pode ser removido.
    /// </summary>
    public class InventarioAgendamentoPodeSerRemovidoSpec : SpecBase<InventarioAgendamento>
    {
        #region Fields
        private readonly IInventarioAgendamentoGateway m_agendamentoGateway;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioAgendamentoPodeSerRemovidoSpec"/>.
        /// </summary>
        /// <param name="agendamentoGateway">O gateway para agendamento de inventário.</param>
        public InventarioAgendamentoPodeSerRemovidoSpec(IInventarioAgendamentoGateway agendamentoGateway)
        {
            m_agendamentoGateway = agendamentoGateway;
        }
        #endregion

        /// <summary>
        /// Verifica se as datas especificadas satisfazem a especificação.
        /// </summary>
        /// <param name="target">As datas.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelas datas.
        /// </returns>
        public override SpecResult IsSatisfiedBy(InventarioAgendamento target)
        {
            var inventario = target.Inventario;

            // Inventário cancelado não pode ser excluído.
            if (inventario.stInventario == InventarioStatus.Cancelado)
            {
                return NotSatisfied(Texts.CanceledInventaryCannotBeRemoved);
            }

            // Inventários com data atual ou passada não pode ser excluído.
            if (inventario.dhInventario.Date <= DateTime.Today)
            {
                return NotSatisfied(Texts.InventaryWithDateBeforeTodayCannotBeRemoved);
            }

            // Inventário com agendamento único para a data não pode ser excluído.
            var agendamentosParaData = m_agendamentoGateway.ObterQuantidadeAgendamentos(inventario.IDLoja, inventario.IDDepartamento.Value, inventario.dhInventario);

            if (agendamentosParaData == 1)
            {
                return NotSatisfied(Texts.UniqueInventarySchedulingToStoreCannotBeRemoved.With(
                        target.Bandeira.CdSistema,
                        target.Loja.cdLoja,
                        target.Departamento.cdDepartamento,
                        target.dtAgendamento));
            }

            return Satisfied();
        }
    }
}
