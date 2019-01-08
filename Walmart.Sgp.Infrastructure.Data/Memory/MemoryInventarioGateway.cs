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
    /// Implementação de um table data gateway para inventário em memória.
    /// </summary>
    /// <remarks>
    /// Essa implementação deve ser utilizada apenas para fins de testes unitários.
    /// </remarks>
    public class MemoryInventarioGateway : MemoryDataGateway<Inventario>, IInventarioGateway
    {
        /// <summary>
        /// Localiza a data do inventário da loja informada.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>A data do inventário, se existir.</returns>
        public DateTime? ObterDataInventarioDaLoja(int idLoja)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém a lista de inventários agendados e abertos.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="dataInventario">A data do inventário.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="idCategoria">O id da categoria.</param>
        /// <returns>Os Inventario que estão abertos.</returns>
        public IEnumerable<Inventario> ObterInventariosAbertosParaImportacao(int idLoja, DateTime? dataInventario, int? idDepartamento, int? idCategoria)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Realiza o cancelamento do inventário informado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        public void CancelarInventario(Inventario inventario)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém os inventários que correspondem ao filtro especificado.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Os inventários.</returns>
        public IEnumerable<InventarioSumario> ObterSumarizadoPorFiltro(InventarioFiltro filtro, Paging paging)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o custo total por filtro.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>O valor do custo total.</returns>
        public decimal ObterCustoTotalPorFiltro(InventarioFiltro filtro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Conta os inventários para a loja, departamento que estão no intervalo de data e que possuem um dos status informados.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="intervaloDhInventario">O intervalo para a data do inventário.</param>
        /// <param name="stInventarios">Os status do inventario.</param>
        /// <returns>O número de inventários.</returns>
        public int ContarInventarios(int idLoja, int idDepartamento, Framework.Commons.RangeValue<DateTime> intervaloDhInventario, params InventarioStatus[] stInventarios)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o inventário estruturado pelo id especificado.
        /// </summary>
        /// <param name="id">O id do inventário.</param>
        /// <returns>
        /// O inventário estruturado.
        /// </returns>        
        public InventarioSumario ObterEstruturadoPorId(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o ultimo inventário da loja.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="dhUltimoFechamentoFiscalLoja">A data do ultimo fechamento fiscal da loja.</param>
        /// <returns>
        /// O ultimo inventario da loja ou <c>null</c> caso a loja não possua inventários.
        /// </returns>        
        public Inventario ObterUltimo(int idLoja, int? idDepartamento, DateTime dhUltimoFechamentoFiscalLoja)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado possui
        /// itens inativos ou deletados.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// <c>true</c> se o inventário possui itens inativos ou
        /// deletados; caso contrario <c>false</c>.
        /// </returns>        
        public bool PossuiItemInativoDeletado(int idInventario)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado
        /// possui sortimento inválido.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// <c>true</c> se o possui sortimento invalido;
        /// caso contrário <c>false</c>.
        /// </returns>
        public bool PossuiSortimentoInvalido(int idInventario)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado
        /// possui itens com custo de cadastro.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// <c>true</c> se algum item possui custo de cadastro;
        /// caso contrário <c>false</c>.
        /// </returns>
        public bool PossuiItemComCustoDeCadastro(int idInventario)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finaliza o inventário especificado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        public void Finalizar(Inventario inventario)
        {
            throw new NotImplementedException();            
        }

        /// <summary>
        /// Obtém o inventário aprovado e finalizado na data especificada.
        /// </summary>
        /// <param name="request">O filtro.</param>
        /// <returns>
        /// O inventário.
        /// </returns>
        public Inventario ObterInventarioAprovadoFinalizadoMesmaData(ImportarInventarioAutomaticoRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reverters the para status.
        /// </summary>
        /// <param name="inventario">The inventario.</param>
        /// <param name="novoStatus">The novo status.</param>
        public void ReverterParaStatus(Inventario inventario, InventarioStatus novoStatus)
        {
            throw new NotImplementedException();    
        }

        /// <summary>
        /// Retorna um valor indicando se o inventario especificado possui
        /// itens do tipo vinculado de entrada.
        /// </summary>
        /// <param name="idInventario">O id de inventario.</param>
        /// <returns>
        /// <c>true</c> se o inventário possui itens do tipo
        /// vinculado de entrada;caso contrario <c>false</c>.
        /// </returns>
        public bool PossuiItemVinculadoEntrada(int idInventario)
        {
            throw new NotImplementedException();            
        }

        /// <summary>
        /// Ajusta estoque inventariado.
        /// </summary>
        /// <param name="idInventario">O id do inventário.</param>
        /// <param name="aprovacao">Indica se é uma operação de aprovação.</param>
        public void AjustarEstoqueInventariado(int idInventario, bool aprovacao)
        {
            throw new NotImplementedException();            
        }
    }
}