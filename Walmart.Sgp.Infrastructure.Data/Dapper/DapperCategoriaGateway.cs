using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para categoria utilizando o Dapper.
    /// </summary>
    public class DapperCategoriaGateway : EntityDapperDataGatewayBase<Categoria>, ICategoriaGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperCategoriaGateway" />.
        /// </summary>
        /// <param name="databases">A data de .</param>
        public DapperCategoriaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Categoria", "IDCategoria")
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get 
            {
                return new string[] 
        {
                    "IDDepartamento",
                    "cdSistema",
                    "cdCategoria",
                    "dsCategoria",
                    "blPerecivel",
                    "blAtivo",
                    "dhCriacao",
                    "dhAtualizacao",
                    "cdUsuarioCriacao",
                    "cdUsuarioAtualizacao"
                };
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Obtém uma categoria a partir do código da categoria e do código de estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>A categoria.</returns>
        public Categoria ObterPorCategoriaESistema(int cdCategoria, int? cdDepartamento, byte cdSistema)
        {
            var result = this.Resource.Query<Categoria, Departamento, Divisao, Categoria>(
                Sql.Categoria.ObterPorCategoriaESistema,
                new { cdCategoria, cdDepartamento, cdDivisao = (int?)null, cdSistema },
                MapCategoria,
                "SplitOn1,SplitOn2").ToArray();

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
        /// Pesquisa categorias filtrando pelo código da categoria, descrição da categoria, flag que indica se é de perecíveis, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdCategoria">O código de categoria.</param>
        /// <param name="dsCategoria">The ds categoria.</param>
        /// <param name="cdSistema">O código de sistema.</param>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As categorias.</returns>
        /// <remarks>Nos mapeamentos de departamento e divisão, traz apenas o código e a descrição do departamento e da divisão.</remarks>
        public IEnumerable<Categoria> PesquisarPorCategoriaDepartamentoESistema(int? cdCategoria, string dsCategoria, byte cdSistema, int? cdDepartamento, Paging paging)
        {
            return this.Resource.Query<Categoria, Departamento, Divisao, Categoria>(
                Sql.Categoria.PesquisarPorCategoriaDepartamentoESistema,
                new { cdCategoria, dsCategoria, cdSistema, cdDepartamento },
                MapCategoria,
                "SplitOn1,SplitOn2")
                .AsPaging(paging);
        }

        private Categoria MapCategoria(Categoria categoria, Departamento departamento, Divisao divisao)
        {
            categoria.Departamento = departamento;
            categoria.Departamento.IDDepartamento = categoria.IDDepartamento;
            departamento.Divisao = divisao;
            divisao.IDDivisao = departamento.IDDivisao;
            return categoria;
        }
        #endregion
    }
}
