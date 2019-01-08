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
    /// Implementação de um table data gateway para departamento utilizando o Dapper.
    /// </summary>
    public class DapperDepartamentoGateway : EntityDapperDataGatewayBase<Departamento>, IDepartamentoGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperDepartamentoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperDepartamentoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Departamento", "IDDepartamento")
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
                return new string[] { "IDDivisao", "cdSistema", "cdDepartamento", "dsDepartamento", "blPerecivel", "blAtivo", "dhCriacao", "dhAtualizacao", "cdUsuarioCriacao", "cdUsuarioAtualizacao", "pcDivergenciaNF" };
            }
        }        
        #endregion

        #region Methods

        /// <summary>
        /// Obtém um departamento pelo seu código de departamento e estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="modoPereciveis">Informa o modo pereciveis.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>Retorna apenas se blPerecivel='S' (comportamento padrão das lookups de departamento)</remarks>
        public Departamento ObterPorDepartamentoESistema(int cdDepartamento, byte cdSistema, string modoPereciveis)
        {
            return this.Resource.Query<Departamento, Divisao, Departamento>(
                Sql.Departamento.ObterPorDepartamentoESistema, 
                new { cdDepartamento, cdDivisao = (int?)null, cdSistema, modoPereciveis },
                MapDepartamento,
                "SplitOn1")
                .SingleOrDefault();
        }

        /// <summary>
        /// Pesquisa departamentos filtrando pelo código de departamento, descrição do departamento, flag que indica se é de perecíveis, e/ou código da estrutura mercadológica.
        /// </summary>
        /// <param name="cdDepartamento">O código de departamento.</param>
        /// <param name="dsDepartamento">A descrição do departamento.</param>
        /// <param name="blPerecivel">A flag de perecível.</param>
        /// <param name="cdDivisao">O código da divisão.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>O departamento.</returns>
        /// <remarks>No mapeamento da divisão, traz apenas o código e a descrição da divisão.</remarks>
        public IEnumerable<Departamento> PesquisarPorDivisaoESistema(int? cdDepartamento, string dsDepartamento, bool? blPerecivel, int? cdDivisao, byte cdSistema, Paging paging)
        {
            return this.Resource.Query<Departamento, Divisao, Departamento>(
                Sql.Departamento.PesquisarPorDivisaoESistema,
                new { cdDepartamento = cdDepartamento, dsDepartamento = dsDepartamento, cdSistema = cdSistema, blPerecivel = blPerecivel.ToDb(), cdDivisao = cdDivisao },
                MapDepartamento,
                "SplitOn").AsPaging(paging);
        }

        /// <summary>
        /// Obtém o departamento junto com a divisão por id.
        /// </summary>
        /// <param name="idDepartamento">O id de departamento.</param>
        /// <returns>
        /// O departamento estruturado.
        /// </returns>
        public Departamento ObterEstruturadoPorId(int idDepartamento)
        {
            return Resource.Query<Departamento, Divisao, Departamento>(
                Sql.Departamento.ObterEstruturadoPorId,
                new { idDepartamento },
                MapDepartamento,
                "SplitOn").SingleOrDefault();
        }

        private Departamento MapDepartamento(Departamento departamento, Divisao divisao)
        {
            departamento.Divisao = divisao;
            if (null != divisao)
            {
                departamento.Divisao.IDDivisao = departamento.IDDivisao;
            }

            return departamento;
        }

        #endregion
    }
}
