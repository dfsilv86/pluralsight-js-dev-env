using System;
using System.Collections.Generic;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Define a interface de um serviço de domínio para inventários.
    /// </summary>
    public interface IInventarioService : IDomainService<Inventario>
    {
        /// <summary>
        /// Localiza a data do inventário da loja informada.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>A data do inventário, se existir.</returns>
        DateTime? ObterDataInventarioDaLoja(int idLoja);

        /// <summary>
        /// Obtém o número de lojas sem agendamento.
        /// A loja só é considerada como agendada quando todos seus respectivos departamentos por sistema estão agendados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário para verificação do sistema.</param>
        /// <returns>Quantidade de lojas sem agendamento.</returns>
        int ObterQuantidadeLojasSemAgendamento(int idUsuario);

        /// <summary>
        /// Obtém a lista de inventários agendados e abertos.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="dataInventario">A data do inventário.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="idCategoria">O id da categoria.</param>
        /// <returns>Os Inventario que estão abertos.</returns>
        IEnumerable<Inventario> ObterInventariosAbertosParaImportacao(int idLoja, DateTime? dataInventario, int? idDepartamento, int? idCategoria);

        /// <summary>
        /// Obtém os agendamentos de acordo com os parâmetros informados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os agendamentos.</returns>
        IEnumerable<InventarioAgendamento> ObterAgendamentos(InventarioAgendamentoFiltro filtro);

        /// <summary>
        /// Obtém os inventários não agendados
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os inventários não agendados.</returns>
        IEnumerable<InventarioNaoAgendado> ObterNaoAgendados(InventarioAgendamentoFiltro filtro);

        /// <summary>
        /// Remove os agendamentos de inventários informados.
        /// </summary>
        /// <param name="ids">Os ids dos agendamentos de inventário a serem removidos.</param>
        void RemoverAgendamentos(int[] ids);

        /// <summary>
        /// Cancela o inventário informado.
        /// </summary>
        /// <param name="idInventario">O id do inventário.</param>
        void Cancelar(int idInventario);

        /// <summary>
        /// Obtém o agendamento estruturado por id.
        /// </summary>
        /// <param name="id">O id do agendamento.</param>
        /// <returns>O agendamento.</returns>
        InventarioAgendamento ObterAgendamentoEstruturadoPorId(int id);

        /// <summary>
        /// Obtém os inventários que correspondem ao filtro especificado.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os inventários.</returns>
        IEnumerable<InventarioSumario> ObterSumarizadoPorFiltro(InventarioFiltro filtro, Paging paging);

        /// <summary>
        /// Obtém o custo total por filtro.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>O valor do custo total.</returns>
        decimal ObterCustoTotalPorFiltro(InventarioFiltro filtro);    

        /// <summary>
        /// Insere agendamentos para lojas x departamentos.
        /// </summary>
        /// <param name="dtAgendamento">A data de agendamento.</param>
        /// <param name="lojas">As lojas que devem ser gerados os agendamentos.</param>
        /// <param name="departamentos">Os departamentos que devem ser gerados os agendamentos.</param>
        /// <returns>A quantidade de agendamentos inseridos.</returns>
        AgendamentoResponse InserirAgendamentos(DateTime dtAgendamento, IEnumerable<Loja> lojas, IEnumerable<Departamento> departamentos);

        /// <summary>
        /// Atualiza a data de agendamento dos agendamentos de inventário informados.
        /// </summary>
        /// <param name="dtAgendamento">A data de agendamento.</param>
        /// <param name="agendamentoInventarioIDs">Os ids dos agendamentos.</param>
        /// <returns>A quantidade de agendamentos atualizados.</returns>
        AgendamentoResponse AtualizarAgendamentos(DateTime dtAgendamento, params int[] agendamentoInventarioIDs);

        /// <summary>
        /// Obtém o inventário estruturado pelo id especificado.
        /// </summary>
        /// <param name="id">O id do inventário.</param>
        /// <returns>O inventário estruturado.</returns>
        InventarioSumario ObterEstruturadoPorId(int id);

        /// <summary>
        /// Obtém os itens do inventário por filtro.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os itens de inventário que satisfazem o filtro.</returns>
        IEnumerable<InventarioItemSumario> ObterItensEstruturadoPorFiltro(InventarioItemFiltro filtro, Paging paging);

        /// <summary>
        /// Obtém um item de inventário estruturado pelo id.
        /// </summary>
        /// <param name="idInventarioItem">O id do item de inventário.</param>
        /// <returns>O item do inventário.</returns>
        InventarioItem ObterItemEstruturadoPorId(int idInventarioItem);

        /// <summary>
        /// Salva um item de inventário.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="inventario">O inventario que o item pertence.</param>
        void SalvarItem(InventarioItem item, Inventario inventario);

        /// <summary>
        /// Exclui o item de inventário.
        /// </summary>
        /// <param name="idInventarioItem">O id do inventario item.</param>
        void RemoverItem(int idInventarioItem);

        /// <summary>
        /// Finaliza o inventário.
        /// </summary>
        /// <param name="id">O id do inventário.</param>
        void Finalizar(int id);

        /// <summary>
        /// Obtém as irregularidades do inventário.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>Uma coleção de irregularidades que o inventário possui.</returns>
        IEnumerable<string> ObterIrregularidadesFinalizacao(int idInventario);

        /// <summary>
        /// Obtém as operações permitidas sobre o inventário.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        /// <returns>As operações permitidas.</returns>
        InventarioOperacoesPermitidas ObterOperacoesPermitidas(Inventario inventario);

        /// <summary>
        /// Volta o status do inventário.
        /// </summary>
        /// <param name="idInventario">O id do inventário.</param>
        void VoltarStatus(int idInventario);

        /// <summary>
        /// Obtém as irregularidades para aprovação do inventário.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>Uma coleção de irregularidades que o inventário possui.</returns>
        IEnumerable<string> ObterIrregularidadesAprovacao(int idInventario);

        /// <summary>
        /// Aprova o inventário informado.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        void Aprovar(int idInventario);

        /// <summary>
        /// Pesquisa as críticas de inventário pelo filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro para críticas de inventário.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As críticas de inventário.</returns>
        IEnumerable<InventarioCritica> PesquisarCriticas(InventarioCriticaFiltro filtro, Paging paging);
    }
}
