using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para fineline utilizando o Dapper.
    /// </summary>
    public class DapperFineLineGateway : DapperDataGatewayBase<FineLine>, IFineLineGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperFineLineGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperFineLineGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o fineline pelo código de fineline e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdFineLine">O código de fineline.</param>
        /// <param name="cdSubcategoria">O código da subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>O fineline.</returns>
        public FineLine ObterPorFineLineESistema(int cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            var result = this.Resource.Query<FineLine, Subcategoria, Categoria, Departamento, Divisao, FineLine>(
                Sql.FineLine.ObterPorFineLineESistema,
                new { cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdDivisao = (int?)null, cdSistema },
                MapFineLine,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4")
                .ToArray();

            if (result.Length != 1)
            {
                // Faz com que a lookup abra a modal de pesquisa para escolher o item certo caso exista mais de um com mesmo código
                return null;
            }
            else
            {
                return result[0];
            }
        }

        /// <summary>
        /// Pesquisa finelines filtrando pelo código do fineline, descrição do fineline, código da subcategoria, código da categoria, código do departamento, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdFineLine">O código do fineline.</param>
        /// <param name="dsFineLine">A descrição do fineline.</param>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os finelines.</returns>
        public IEnumerable<FineLine> PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(int? cdFineLine, string dsFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema, Paging paging)
        {
            return this.Resource.Query<FineLine, Subcategoria, Categoria, Departamento, Divisao, FineLine>(
                Sql.FineLine.PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema,
                new { cdFineLine, dsFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema },
                MapFineLine,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4")
                .AsPaging(paging);
        }

        private FineLine MapFineLine(FineLine fineLine, Subcategoria subcategoria, Categoria categoria, Departamento departamento, Divisao divisao)
        {
            subcategoria.IDSubcategoria = fineLine.IDSubcategoria;
            categoria.IDCategoria = fineLine.IDCategoria;
            departamento.IDDepartamento = fineLine.IDDepartamento;
            divisao.IDDivisao = departamento.IDDivisao;

            fineLine.Subcategoria = subcategoria;
            fineLine.Subcategoria.Categoria = categoria;
            fineLine.Subcategoria.Categoria.Departamento = departamento;
            fineLine.Subcategoria.Categoria.Departamento.Divisao = divisao;

            return fineLine;
        }
        #endregion
    }
}
