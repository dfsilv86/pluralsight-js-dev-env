using System.Collections.Generic;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para item de nota fiscal utilizando o Dapper.
    /// </summary>
    public class DapperNotaFiscalItemGateway : EntityDapperDataGatewayBase<NotaFiscalItem>, INotaFiscalItemGateway
    {
        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperNotaFiscalItemGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperNotaFiscalItemGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "NotaFiscalItem", "IDNotaFiscalItem")
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
                return new string[] { "IDNotaFiscal", "IDItemDetalhe", "qtItem", "vlMercadoria", "vlCusto", "dhCriacao", "dhAtualizacao", "blDivergente", "tpStatus", "dtLiberacao", "blLiberado", "qtItemAnterior", "nrLinha", "VariacaoUltimoCusto", "IdNotaFiscalItemStatus" };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Salva o log de correção de custo.
        /// </summary>
        /// <param name="notaFiscalItem">O item de nota fiscal.</param>
        public void SalvarLogCorrecaoCusto(NotaFiscalItem notaFiscalItem)
        {
            StoredProcedure.Execute(
                "PR_IncluirLogCorrecaoCusto",
                new
                {
                    IDNotaFiscalItem = notaFiscalItem.IDNotaFiscalItem,
                    qtItemAnterior = notaFiscalItem.qtItemAnterior,
                    qtItemCorrigida = notaFiscalItem.qtItem,
                    dhLiberacao = notaFiscalItem.dtLiberacao,
                    IDUsuarioLiberacao = RuntimeContext.Current.User.Id
                });
        }

        /// <summary>
        /// Libera item de nota fiscal divergente.
        /// </summary>
        /// <param name="notaFiscalItem">O item de nota fiscal.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        public void LiberarItemNotaFiscalDivergente(NotaFiscalItem notaFiscalItem, int idBandeira)
        {
            StoredProcedure.Execute(
                "PR_ImportarNotasFiscais",
                new
                {
                    IdBandeira = idBandeira,
                    IdNotaFiscalItem = notaFiscalItem.IDNotaFiscalItem,
                    Paralelo = 0,
                    IdUsuario = RuntimeContext.Current.User.Id
                });
        }
        #endregion
    }
}
