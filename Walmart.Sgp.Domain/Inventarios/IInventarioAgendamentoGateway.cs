using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Define a interface de um table data gateway para agendamento de inventário.
    /// </summary>
    public interface IInventarioAgendamentoGateway : IDataGateway<InventarioAgendamento>
    {       
        /// <summary>
        /// Obtém o número de lojas sem agendamento.
        /// A loja só é considerada como agendada quando todos seus respectivos departamentos por sistema estão agendados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário para verificação do sistema.</param>
        /// <returns>Quantidade de lojas sem agendamento.</returns>
        int ObterQuantidadeLojasSemAgendamento(int idUsuario);

        /// <summary>
        /// Obtém a quantidade de agendamentos para a loja/departamento na data de agendamento informada.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="dtAgendamento">A data de agendamento.</param>
        /// <returns>A quantidade de agendamentos.</returns>
        int ObterQuantidadeAgendamentos(int idLoja, int idDepartamento, DateTime dtAgendamento);

        /// <summary>
        /// Conta os agendamentos para a loja, departamento que estão no intervalo de agendamento e que possuem um dos status informados.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="intervaloDtAgendamento">O intervalo para a data de agendamento.</param>
        /// <param name="stInventarios">Os status do inventario.</param>
        /// <returns>O número de agendamentos.</returns>
        int ContarAgendamentos(int idLoja, int idDepartamento, RangeValue<DateTime> intervaloDtAgendamento, params InventarioStatus[] stInventarios);

        /// <summary>
        /// Obtém os agendamentos de acordo com os parâmetros informados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os agendamentos.</returns>
        IEnumerable<InventarioAgendamento> ObterAgendamentos(InventarioAgendamentoFiltro filtro);

        /// <summary>
        /// Obtém os inventários não agendados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os inventários não agendados.</returns>
        IEnumerable<InventarioNaoAgendado> ObterNaoAgendados(InventarioAgendamentoFiltro filtro);

        /// <summary>
        /// Obtém os agendamentos de inventário com a estrutura preenchida.
        /// </summary>
        /// <param name="ids">Os ids dos agendamentos desejados.</param>
        /// <returns>Os agendamentos.</returns>
        IEnumerable<InventarioAgendamento> ObterEstruturadosPorIds(int[] ids);
    }
}
