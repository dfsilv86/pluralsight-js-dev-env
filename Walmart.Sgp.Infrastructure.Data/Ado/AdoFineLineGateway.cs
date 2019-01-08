#if ADO_BENCHMARK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Common;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Ado
{
    /// <summary>
    /// Implementação de um table data gateway para fineline utilizando o ADO .NET.
    /// </summary>
    public class AdoFineLineGateway : AdoDataGatewayBase<FineLine>, IFineLineGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AdoFineLineGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public AdoFineLineGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp.Transaction)
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
            var sql = SqlResourceReader.Read("FineLine", Sql.FineLine.ObterPorFineLineESistema);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, new { cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema });

            return Map<FineLine>(cmd).SingleOrDefault();
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
            var sql = SqlResourceReader.Read("FineLine", Sql.FineLine.PesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            CreateParameters(cmd, new { cdFineLine, dsFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema });

            var result = Map<FineLine>(
                cmd, 
                "*", 
                (fineLine, dr) =>
                {
                    fineLine.Subcategoria = Map<Subcategoria>(dr);
                    fineLine.Subcategoria.Categoria = Map<Categoria>(dr);
                    fineLine.Subcategoria.Categoria.Departamento = Map<Departamento>(dr);
                    fineLine.Subcategoria.Categoria.Departamento.Divisao = Map<Divisao>(dr);
                });

            return result;
        }
        #endregion
    }
}
#endif