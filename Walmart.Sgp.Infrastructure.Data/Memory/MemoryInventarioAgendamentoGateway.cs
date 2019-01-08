using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Memory
{
    /// <summary>
    /// Implementação de um table data gateway para agendamento de inventário em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryInventarioAgendamentoGateway : MemoryDataGateway<InventarioAgendamento>, IInventarioAgendamentoGateway
    {
        /// <summary>
        /// Obtém o número de lojas sem agendamento.
        /// A loja só é considerada como agendada quando todos seus respectivos departamentos por sistema estão agendados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário para verificação do sistema.</param>
        /// <returns>Quantidade de lojas sem agendamento.</returns>
        public int ObterQuantidadeLojasSemAgendamento(int idUsuario)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém a quantidade de agendamentos para a loja/departamento na data de agendamento informada.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="dtAgendamento">A data de agendamento.</param>
        /// <returns>A quantidade de agendamentos.</returns>
        public int ObterQuantidadeAgendamentos(int idLoja, int idDepartamento, DateTime dtAgendamento)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém os agendamentos de acordo com os parâmetros informados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os agendamentos.</returns>
        public IEnumerable<InventarioAgendamento> ObterAgendamentos(InventarioAgendamentoFiltro filtro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém os inventários não agendados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os inventários não agendados.</returns>
        public IEnumerable<InventarioNaoAgendado> ObterNaoAgendados(InventarioAgendamentoFiltro filtro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém os agendamentos de inventário com a estrutura preenchida.
        /// </summary>
        /// <param name="ids">Os ids dos agendamentos desejados.</param>
        /// <returns>Os agendamentos.</returns>
        public IEnumerable<InventarioAgendamento> ObterEstruturadosPorIds(int[] ids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Conta os agendamentos para a loja, departamento que estão no intervalo de agendamento e que possuem um dos status informados.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="intervaloDtAgendamento">O intervalo para a data de agendamento.</param>
        /// <param name="stInventarios">Os status do inventario.</param>
        /// <returns>O número de agendamentos.</returns>
        public int ContarAgendamentos(int idLoja, int idDepartamento, Framework.Commons.RangeValue<DateTime> intervaloDtAgendamento, params InventarioStatus[] stInventarios)
        {
            throw new NotImplementedException();
        }
    }
}