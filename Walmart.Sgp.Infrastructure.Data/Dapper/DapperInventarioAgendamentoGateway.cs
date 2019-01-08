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
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para agendamento de inventário utilizando o Dapper.
    /// </summary>
    public class DapperInventarioAgendamentoGateway : EntityDapperDataGatewayBase<InventarioAgendamento>, IInventarioAgendamentoGateway
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperInventarioAgendamentoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperInventarioAgendamentoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "InventarioAgendamento", "IDInventarioAgendamento")
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
                    "IDFormato",
                    "dtAgendamento",
                    "stAgendamento",
                    "dhCriacao",
                    "IDUsuarioCriacao",
                    "dhAlteracao"
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o número de lojas sem agendamento.
        /// A loja só é considerada como agendada quando todos seus respectivos departamentos por sistema estão agendados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário para verificação do sistema.</param>
        /// <returns>Quantidade de lojas sem agendamento.</returns>
        public int ObterQuantidadeLojasSemAgendamento(int idUsuario)
        {
            return StoredProcedure.ExecuteScalar<int>("PR_LojasNaoAgendadas", new { idUsuario });
        }

        /// <summary>
        /// Obtém a quantidade de agendamentos para a loja/departamento na data de agendamento informada.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="dtAgendamento">A data de agendamento.</param>
        /// <returns>A quantidade de agendamentos.</returns>
        public int ObterQuantidadeAgendamentos(int idLoja, int idDepartamento, DateTime dtAgendamento)
        {
            return Resource.ExecuteScalar<int>(
                Sql.InventarioAgendamento.ObterQuantidadeAgendamentos,
                new
                {
                    agendamentoStatusCancelado = InventarioAgendamentoStatus.Cancelado,
                    inventarioStatusCancelado = InventarioStatus.Cancelado,
                    idLoja,
                    idDepartamento,
                    dtAgendamento
                });
        }

        /// <summary>
        /// Obtém os agendamentos de acordo com os parâmetros informados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os agendamentos.</returns>
        public IEnumerable<InventarioAgendamento> ObterAgendamentos(InventarioAgendamentoFiltro filtro)
        {
            return Resource.Query<InventarioAgendamento, Bandeira, Loja, Departamento, Inventario, InventarioAgendamento>(
                Sql.InventarioAgendamento.ObterAgendamentos,
                new
                {
                    agendamentoStatusCancelado = InventarioAgendamentoStatus.Cancelado,
                    inventarioStatusCancelado = InventarioStatus.Cancelado,
                    filtro.IDBandeira,
                    filtro.CdLoja,
                    filtro.CdDepartamento,
                    filtro.DtAgendamento
                },
                (ia, b, l, d, i) =>
                {
                    ia.Bandeira = b;
                    ia.Loja = l;
                    ia.Departamento = d;
                    ia.Inventario = i;

                    return ia;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4");
        }

        /// <summary>
        /// Obtém os inventários não agendados.
        /// </summary>
        /// <param name="filtro">O filtro.</param>
        /// <returns>Os inventários não agendados.</returns>
        public IEnumerable<InventarioNaoAgendado> ObterNaoAgendados(InventarioAgendamentoFiltro filtro)
        {
            return Resource.Query<Bandeira, Loja, Departamento, InventarioNaoAgendado>(
                Sql.InventarioAgendamento.ObterNaoAgendados,
                new
                {
                    agendamentoStatusCancelado = InventarioAgendamentoStatus.Cancelado,
                    inventarioStatusCancelado = InventarioStatus.Cancelado,
                    filtro.IDBandeira,
                    filtro.CdLoja,
                    filtro.CdDepartamento
                },
                (b, l, d) =>
                {
                    return new InventarioNaoAgendado
                    {
                        Bandeira = b,
                        Loja = l,
                        Departamento = d
                    };
                },
                "SplitOn1,SplitOn2");
        }

        /// <summary>
        /// Obtém os agendamentos de inventário com a estrutura preenchida.
        /// </summary>
        /// <param name="ids">Os ids dos agendamentos desejados.</param>
        /// <returns>Os agendamentos.</returns>
        public IEnumerable<InventarioAgendamento> ObterEstruturadosPorIds(int[] ids)
        {
            return Resource.Query<InventarioAgendamento, Inventario, Bandeira, Loja, Departamento, InventarioAgendamento>(
                Sql.InventarioAgendamento.ObterEstruturadosPorIds,
                new { ids },
                  (ia, i, b, l, d) =>
                  {
                      ia.Bandeira = b;
                      ia.Loja = l;
                      ia.Departamento = d;
                      ia.Inventario = i;

                      return ia;
                  },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4");
        }

        /// <summary>
        /// Conta os agendamentos para a loja, departamento que estão no intervalo de agendamento e que possuem um dos status informados.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <param name="idDepartamento">O id do departamento.</param>
        /// <param name="intervaloDtAgendamento">O intervalo para a data de agendamento.</param>
        /// <param name="stInventarios">Os status do inventario.</param>
        /// <returns>O número de agendamentos.</returns>
        public int ContarAgendamentos(int idLoja, int idDepartamento, RangeValue<DateTime> intervaloDtAgendamento, params InventarioStatus[] stInventarios)
        {
            return Resource.ExecuteScalar<int>(
                Sql.InventarioAgendamento.ContarAgendamentos,
                new
                {
                    idLoja,
                    idDepartamento,
                    inicioDtAgendamento = intervaloDtAgendamento.StartValue,
                    fimDtAgendamento = intervaloDtAgendamento.EndValue,
                    statusAgendamentoCancelado = InventarioAgendamentoStatus.Cancelado.Value,
                    stInventarios = stInventarios.Select(s => s.Value)
                });
        }
        #endregion
    }
}
