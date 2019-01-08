using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para ReturnSheet utilizando o Dapper.
    /// </summary>
    public class DapperReturnSheetGateway : EntityDapperDataGatewayBase<ReturnSheet>, IReturnSheetGateway
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperReturnSheetGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperReturnSheetGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ReturnSheet", "IdReturnSheet")
        {
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "DhInicioReturn", "DhFinalReturn", "DhInicioEvento", "DhFinalEvento", "IdRegiaoCompra", "idDepartamento", "Descricao", "IdUsuarioCriacao", "DhAtualizacao", "DhCriacao", "BlAtivo" };
            }
        }

        /// <summary>
        /// Obtém um ReturnSheet pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade ReturnSheet.</returns>
        public ReturnSheet Obter(long id)
        {
            return this.Find("IDReturnSheet=@IDReturnSheet", new { IDReturnSheet = id }).SingleOrDefault();
        }

        /// <summary>
        /// Verifica se uma ReturnSheet possui itens exportados
        /// </summary>
        /// <param name="idReturnSheet">Id da returnSheet.</param>
        /// <returns>Se a ReturnSheet possui itens exportados.</returns>
        public bool PossuiExportacao(int idReturnSheet)
        {
            var args = new
            {
                IdReturnSheet = idReturnSheet
            };

            var result = this.Resource.Query<int>(Sql.ReturnSheet.PossuiExportacao, args).Single();

            return result > 0;
        }

        /// <summary>
        /// Verifica se uma ReturnSheet possui itens autorizados
        /// </summary>
        /// <param name="idReturnSheet">Id da returnSheet.</param>
        /// <returns>Se a ReturnSheet possui itens autorizados.</returns>
        public bool PossuiAutorizacao(int idReturnSheet)
        {
            var args = new
            {
                IdReturnSheet = idReturnSheet
            };

            var result = this.Resource.Query<int>(Sql.ReturnSheet.PossuiAutorizacao, args).Single();

            return result > 0;
        }

        /// <summary>
        /// Pesquisar ReturnSheet
        /// </summary>
        /// <param name="dtInicioReturn">Data inicio do ReturnSheet</param>
        /// <param name="dtFinalReturn">Data final do ReturnSheet</param>
        /// <param name="evento">Descricao do evento (ReturnSheet)</param>
        /// <param name="idDepartamento">Id do departamento</param>
        /// <param name="filtroAtivos">Filtrar somente ativos (0 = Somente Inativos, 1 = Somente Ativos, 2 = Todos)</param>
        /// <param name="idRegiaoCompra">Id da regiao de compra.</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>Uma lista de ReturnSheet</returns>
        public IEnumerable<ReturnSheet> Pesquisar(DateTime? dtInicioReturn, DateTime? dtFinalReturn, string evento, int? idDepartamento, int filtroAtivos, int? idRegiaoCompra, Paging paging)
        {
            var args = new
            {
                inicioReturn = dtInicioReturn,
                finalReturn = dtFinalReturn,
                evento,
                idDepartamento,
                ativos = filtroAtivos,
                idRegiaoCompra
            };

            return this.Resource.Query<ReturnSheet, RegiaoCompra, Usuario, ReturnSheet>(
                Sql.ReturnSheet.Pesquisar,
                args,
                MapReturnSheet,
                "SplitOn1,SplitOn2")
            .AsPaging(paging);
        }

        private ReturnSheet MapReturnSheet(ReturnSheet returnSheet, RegiaoCompra regiaoCompra, Usuario usuario)
        {
            returnSheet.Usuario = usuario;
            returnSheet.RegiaoCompra = regiaoCompra;
            return returnSheet;
        }
    }
}
