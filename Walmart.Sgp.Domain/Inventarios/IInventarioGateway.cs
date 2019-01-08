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
    /// Define a interface de um table data gateway para inventário.
    /// </summary>
    public interface IInventarioGateway : IDataGateway<Inventario>
    {
        /// <summary>
        /// Localiza a data do inventário da loja informada.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>A data do inventário, se existir.</returns>
        DateTime? ObterDataInventarioDaLoja(int idLoja);     

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
        /// Realiza o cancelamento do inventário informado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        void CancelarInventario(Inventario inventario);

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
        /// Conta os inventários para a loja, departamento que estão no intervalo de data e que possuem um dos status informados.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="intervaloDhInventario">O intervalo para a data do inventário.</param>
        /// <param name="stInventarios">Os status do inventario.</param>
        /// <returns>O número de inventários.</returns>
        int ContarInventarios(int idLoja, int idDepartamento, RangeValue<DateTime> intervaloDhInventario, params InventarioStatus[] stInventarios);

        /// <summary>
        /// Obtém o inventário estruturado pelo id especificado.
        /// </summary>
        /// <param name="id">O id do inventário.</param>
        /// <returns>
        /// O inventário estruturado.
        /// </returns>
        InventarioSumario ObterEstruturadoPorId(int id);

        /// <summary>
        /// Obtém o ultimo inventário da loja.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="dhUltimoFechamentoFiscalLoja">A data do ultimo fechamento fiscal da loja.</param>
        /// <returns>O ultimo inventario da loja ou <c>null</c> caso a loja não possua inventários.</returns>
        Inventario ObterUltimo(int idLoja, int? idDepartamento, DateTime dhUltimoFechamentoFiscalLoja);

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado possui
        /// itens inativos ou deletados.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns><c>true</c> se o inventário possui itens inativos ou
        /// deletados; caso contrario <c>false</c>.</returns>
        bool PossuiItemInativoDeletado(int idInventario);

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado
        /// possui sortimento inválido.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns><c>true</c> se o possui sortimento invalido;
        /// caso contrário <c>false</c>.</returns>
        bool PossuiSortimentoInvalido(int idInventario);

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado
        /// possui itens com custo de cadastro.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns><c>true</c> se algum item possui custo de cadastro;
        /// caso contrário <c>false</c>.</returns>
        bool PossuiItemComCustoDeCadastro(int idInventario);

        /// <summary>
        /// Finaliza o inventário especificado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        void Finalizar(Inventario inventario);

        /// <summary>
        /// Obtém o inventário aprovado e finalizado na data especificada.
        /// </summary>
        /// <param name="request">O filtro.</param>
        /// <returns>O inventário.</returns>
        Inventario ObterInventarioAprovadoFinalizadoMesmaData(ImportarInventarioAutomaticoRequest request);

        /// <summary>
        /// Reverte o inventário para o status informado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        /// <param name="novoStatus">O status para o qual o inventário será revertido.</param>
        void ReverterParaStatus(Inventario inventario, InventarioStatus novoStatus);

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado possui
        /// itens do tipo vinculado de entrada.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns><c>true</c> se o inventário possui itens do tipo
        /// vinculado de entrada;caso contrario <c>false</c>.
        /// </returns>
        bool PossuiItemVinculadoEntrada(int idInventario);

        /// <summary>
        /// Ajusta estoque inventariado.
        /// </summary>
        /// <param name="idInventario">O id do inventário.</param>
        /// <param name="aprovacao">Indica se é uma operação de aprovação.</param>
        void AjustarEstoqueInventariado(int idInventario, bool aprovacao);
    }
}
