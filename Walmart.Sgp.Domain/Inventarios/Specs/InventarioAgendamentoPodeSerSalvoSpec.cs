using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se um agendamento de inventário pode ser salvo. 
    /// </summary>
    public class InventarioAgendamentoPodeSerSalvoSpec : SpecBase<InventarioAgendamento>
    {
        #region Constants
        /// <summary>
        /// A quantidade de dias mínimo que será verificada para agendamento de mesma loja e departamento.
        /// </summary>
        private const int DiasMinimosEntreAgendamentos = 5;
        #endregion

        #region Fields
        private readonly IInventarioGateway m_inventarioGateway;
        private readonly IInventarioAgendamentoGateway m_agendamentoGateway;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="InventarioAgendamentoPodeSerSalvoSpec"/>.
        /// </summary>
        /// <param name="inventarioGateway">>O gateway para nventário.</param>
        /// <param name="agendamentoGateway">O gateway para agendamento de inventário.</param>
        public InventarioAgendamentoPodeSerSalvoSpec(IInventarioGateway inventarioGateway, IInventarioAgendamentoGateway agendamentoGateway)
        {
            m_inventarioGateway = inventarioGateway;
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
            var idLoja = inventario.IDLoja;
            var idDepartamento = inventario.IDDepartamento.Value;
            var dtAgendamento = target.dtAgendamento;

            if (dtAgendamento < DateTime.Today)
            {
                return NotSatisfied(Texts.DateCannotBeLowerThanToday);
            }

            if (target.IsNew)
            {
                return IsSatisfiedByInserir(dtAgendamento, idLoja, idDepartamento, inventario);
            }
            else
            {
                return IsSatisfiedByAtualizar(dtAgendamento, idLoja, idDepartamento, inventario);
            }
        }

        private SpecResult IsSatisfiedByInserir(DateTime dtAgendamento, int idLoja, int idDepartamento, Inventario inventario)
        {
            // Existe agendamento aberto ou importado no intervalo de -5 e +5 dias.    
            var data = dtAgendamento.Date;
            var intervaloDtAgendamento = new RangeValue<DateTime>
            {
                StartValue = data.AddDays(-DiasMinimosEntreAgendamentos),
                EndValue = data.AddDays(DiasMinimosEntreAgendamentos).ToLastDayTime()
            };

            if (m_agendamentoGateway.ContarAgendamentos(
                idLoja,
                idDepartamento,
                intervaloDtAgendamento,
                InventarioStatus.Aberto,
                InventarioStatus.Importado) > 0)
            {
                return NotSatisfied(Texts.ThereAreOthersOpenedApprovedInventoryScheduling.With(DiasMinimosEntreAgendamentos, inventario.Loja.cdLoja, inventario.Departamento.cdDepartamento));
            }

            if (TemInventarioNoMes(dtAgendamento, idLoja, idDepartamento))
            {
                return NotSatisfied(Texts.InventoryDateIsNearToAnotherScheduling.With(inventario.Loja.cdLoja, inventario.Departamento.cdDepartamento));
            }

            return Satisfied();
        }

        private SpecResult IsSatisfiedByAtualizar(DateTime dtAgendamento, int idLoja, int idDepartamento, Inventario inventario)
        {
            if (inventario.stInventario == InventarioStatus.Cancelado || (dtAgendamento == DateTime.Today && inventario.stInventario != InventarioStatus.Aberto) || dtAgendamento < DateTime.Today)
            {
                return NotSatisfied(Texts.OnlyUpdateInventorySchedulingWithScheduledStatus);
            }

            if (TemInventarioNoMes(dtAgendamento, idLoja, idDepartamento))
            {
                return NotSatisfied(Texts.CannotUpdateInventorySchedulingThereIsNotFinishedOne);
            }

            return Satisfied();
        }

        private bool TemInventarioNoMes(DateTime dtAgendamento, int idLoja, int idDepartamento)
        {
            // Existe inventário aprovado no mês.            
            var inicioDoMes = new DateTime(dtAgendamento.Year, dtAgendamento.Month, 1);
            var intervaloDhInventario = new RangeValue<DateTime>
            {
                StartValue = inicioDoMes,
                EndValue = inicioDoMes.ToLastMonthTime()
            };

            return m_inventarioGateway.ContarInventarios(
                idLoja,
                idDepartamento,
                intervaloDhInventario,
                InventarioStatus.Aprovado) > 0;
        }
    }
}
