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
    /// Implementação de um table data gateway para subcategoria utilizando o Dapper.
    /// </summary>
    public class DapperSubcategoriaGateway : DapperDataGatewayBase<Subcategoria>, ISubcategoriaGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperSubcategoriaGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperSubcategoriaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém a subcategoria pelo código de subcategoria e código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <returns>A subcategoria.</returns>
        public Subcategoria ObterPorSubcategoriaESistema(int cdSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            var result = this.Resource.Query<Subcategoria, Categoria, Departamento, Divisao, Subcategoria>(
                Sql.Subcategoria.ObterPorSubcategoriaESistema,
                new { cdSubcategoria, cdCategoria, cdDepartamento, cdDivisao = (int?)null, cdSistema },
                MapSubcategoria,
                "SplitOn1,SplitOn2,SplitOn3")
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
        /// Pesquisa subcategorias filtrando pelo código da subcategoria, descrição da subcategoria, código da categoria, código do departamento, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdSubcategoria">O código de subcategoria.</param>
        /// <param name="dsSubcategoria">Descrição da subcategoria.</param>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As subcategorias.</returns>
        public IEnumerable<Subcategoria> PesquisarPorSubcategoriaCategoriaDepartamentoESistema(int? cdSubcategoria, string dsSubcategoria, int? cdCategoria, int? cdDepartamento, byte cdSistema, Paging paging)
        {
            return this.Resource.Query<Subcategoria, Categoria, Departamento, Divisao, Subcategoria>(
                Sql.Subcategoria.PesquisarPorSubcategoriaCategoriaDepartamentoESistema,
                new { cdSubcategoria, dsSubcategoria, cdCategoria, cdDepartamento, cdSistema },
                MapSubcategoria,
                "SplitOn1,SplitOn2,SplitOn3").AsPaging(paging);
        }

        private Subcategoria MapSubcategoria(Subcategoria subcategoria, Categoria categoria, Departamento departamento, Divisao divisao)
        {
            if (null != categoria)
            {
                subcategoria.Categoria = categoria;
                categoria.IDCategoria = subcategoria.IDCategoria;

                if (null != departamento)
                {
                    categoria.Departamento = departamento;
                    departamento.IDDepartamento = subcategoria.IDDepartamento;

                    if (null != divisao)
                    {
                        departamento.Divisao = divisao;
                        divisao.IDDivisao = departamento.IDDivisao;
                    }
                }
            }

            return subcategoria;
        }
        #endregion
    }
}
