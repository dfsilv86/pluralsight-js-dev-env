using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para crítica de inventário utilizando o Dapper.
    /// </summary>
    public class DapperInventarioCriticaGateway : EntityDapperDataGatewayBase<InventarioCritica>, IInventarioCriticaGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperInventarioCriticaGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperInventarioCriticaGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "InventarioCritica", "IDInventarioCritica")
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
                    "IDInventario",
                    "IDLoja",
                    "IDInventarioCriticaTipo",
                    "dsCritica",
                    "dhInclusao",
                    "IDDepartamento",
                    "IDCategoria",
                    "dhInventario",
                    "blAtivo"
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza a inativação das críticas do inventário informado.
        /// </summary>
        /// <param name="inventario">O inventário.</param>
        public void InativarCriticas(Inventario inventario)
        {
            var filter = @"  blAtivo = 1
                    AND 
                    (
	                    IDInventario = @IDInventario 
                        OR (    
    	                        IDInventario IS NULL 
    	                    AND IDLoja = @IDLoja 
    	                    AND IDDepartamento = ISNULL(@IDDepartamento, IDDepartamento)
    	                    AND IDCategoria = ISNULL(@IDCategoria, IDCategoria)    
	                    AND dhInventario = @dhInventario
	                    )
                    )";

            Update("blAtivo = 0", filter, inventario);
        }

        /// <summary>
        /// Pesquisa as críticas de inventário pelo filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro para críticas de inventário.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As críticas de inventário.</returns>
        public IEnumerable<InventarioCritica> Pesquisar(InventarioCriticaFiltro filtro, Paging paging)
        {
            return Resource.Query<InventarioCritica, InventarioCriticaTipo, Loja, Departamento, Categoria, InventarioCritica>(
                Sql.InventarioCritica.Pesquisar,
                new
                {
                    idUsuario = RuntimeContext.Current.User.Id,
                    dhInclusaoInicio = filtro.DhInclusao.StartValue,
                    dhInclusaoFim = filtro.DhInclusao.EndValue,
                    idBandeira = filtro.IDBandeira,
                    idCategoria = filtro.IDCategoria,
                    idDepartamento = filtro.IDDepartamento,
                    idLoja = filtro.IDLoja
                },
                (ic, t, l, d, c) =>
                {
                    ic.Tipo = t;
                    ic.Loja = l;
                    ic.Departamento = d;

                    if (c != null)
                    {
                        ic.Categoria = c;
                    }

                    return ic;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4")
                .AsPaging(paging);
        }
        #endregion
    }
}
