using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Define a interface de um table data gateway para revisao custo.
    /// </summary>
    public interface IRevisaoCustoGateway : IDataGateway<RevisaoCusto>
    {        
        /// <summary>
        /// Obtém uma revisao de custo pelo seu id e retorna a rebisao de custo com informações das entidades associadas.
        /// </summary>
        /// <param name="idRevisaoCusto">O id da revisao de custo.</param>
        /// <returns>A RevisaoCusto com informações de Loja, ItemDetalhe, StatusRevisaoCusto, MotivoRevisaoCusto, Departamento.</returns>
        RevisaoCusto ObterEstruturadoPorId(int idRevisaoCusto);

        /// <summary>
        /// Pesquisa detalhe de revisoes de custos pelos filtros informados.
        /// </summary>
        /// <param name="filtro">O filtro</param>
        /// <param name="paging">A paginação</param>
        /// <returns>As revisões de custos.</returns>
        IEnumerable<RevisaoCusto> PesquisarPorFiltros(RevisaoCustoFiltro filtro, Paging paging);
    }
}
