using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para ReturnSheetItemPrincipal utilizando o Dapper.
    /// </summary>
    public class DapperReturnSheetItemPrincipalGateway : EntityDapperDataGatewayBase<ReturnSheetItemPrincipal>, IReturnSheetItemPrincipalGateway
    {
        private static String[] s_returnSheetItemPrincipalAuditProperties = new String[] { "blAtivo" };
        private IAuditService m_auditService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperReturnSheetItemPrincipalGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperReturnSheetItemPrincipalGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "ReturnSheetItemPrincipal", "IdReturnSheetItemPrincipal")
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperReturnSheetItemPrincipalGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        public DapperReturnSheetItemPrincipalGateway(ApplicationDatabases databases, IAuditService auditService)
            : base(databases.Wlmslp, "ReturnSheetItemPrincipal", "IdReturnSheetItemPrincipal")
        {
            this.m_auditService = auditService;
        }

        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "IdReturnSheet", "IdItemDetalhe", "blAtivo" };
            }
        }

        /// <summary>
        /// Obtém um ReturnSheetItemPrincipal pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade ReturnSheetItemPrincipal.</returns>
        public ReturnSheetItemPrincipal ObterPorId(long id)
        {
            return this.Find("IDReturnSheetItemPrincipal=@IDReturnSheetItemPrincipal", new { IDReturnSheetItemPrincipal = id }).SingleOrDefault();
        }

        /// <summary>
        /// Pesquisa ReturnSheetItemPrincipal por um IdReturnSheet.
        /// </summary>
        /// <param name="idReturnSheet">O ID do return sheet.</param>
        /// <param name="paging">O controle de paginação.</param>
        /// <returns>Uma lista de ReturnSheetItemPrincipal.</returns>
        public IEnumerable<ReturnSheetItemPrincipal> PesquisarPorIdReturnSheet(int idReturnSheet, Paging paging)
        {
            var args = new
           {
               IdReturnSheet = idReturnSheet
           };

            var r = this.Resource.Query<ReturnSheetItemPrincipal, ReturnSheet, ItemDetalhe, RegiaoCompra, Departamento, ReturnSheetItemPrincipal>(
                Sql.ReturnSheetItemPrincipal.PesquisarPorIdReturnSheet,
                args,
                MapReturnSheetItemPrincipal,
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4");

            if (paging != null)
            {
                return r.AsPaging(paging);
            }
            else
            {
                return r;
            }
        }
        
        /// <summary>
        /// Obtem uma lista de ReturnSheetItemPrincipal por IdReturnSheet
        /// </summary>
        /// <param name="idReturnSheet">O IdReturnSheet</param>
        /// <returns>Uma lista de ReturnSheetItemPrincipal.</returns>
        public IEnumerable<ReturnSheetItemPrincipal> ObterPorIdReturnSheet(int idReturnSheet)
        {
            return this.PesquisarPorIdReturnSheet(idReturnSheet, null);
        }

        /// <summary>
        /// Insere (caso necessário) e retornar o identificador.
        /// </summary>
        /// <param name="idReturnSheet">O identificador da return sheet.</param>
        /// <param name="idItemDetalhe">O identificador do item detalhe de saída.</param>
        /// <returns>Retorna o ReturnSheetItemPrincipal.</returns>
        public ReturnSheetItemPrincipal Insert(int idReturnSheet, long idItemDetalhe)
        {
            var returnSheetItemPrincipal = Find("IdReturnSheet=@idReturnSheet AND IdItemDetalhe=@idItemDetalhe", new { idReturnSheet, idItemDetalhe }).SingleOrDefault();

            if (returnSheetItemPrincipal == null)
            {
                returnSheetItemPrincipal = new ReturnSheetItemPrincipal
                {
                    IdReturnSheet = idReturnSheet,
                    IdItemDetalhe = idItemDetalhe,
                    blAtivo = true
                };

                Insert(returnSheetItemPrincipal);
                this.m_auditService.LogInsert(returnSheetItemPrincipal, s_returnSheetItemPrincipalAuditProperties);
            }

            return returnSheetItemPrincipal;
        }

        /// <summary>
        /// Obtém o identificador do ReturnSheetItemPrincipal.
        /// </summary>
        /// <param name="idReturnSheet">O identificador da ReturnSheet.</param>
        /// <param name="cdItem">O código do item detalhe de saída.</param>
        /// <returns>Retorna o identificador do ReturnSheetItemPrincipal.</returns>
        public int ObterPorReturnSheetEItemDetalheSaida(int idReturnSheet, int cdItem)
        {
            var args = new 
            {
                idReturnSheet,
                cdItem
            };

            return this.Resource.Query<int>(Sql.ReturnSheetItemPrincipal.ObterPorReturnSheetEItemDetalheSaida, args)
                .SingleOrDefault();
        }

        private ReturnSheetItemPrincipal MapReturnSheetItemPrincipal(ReturnSheetItemPrincipal returnSheetItemPrincipal, ReturnSheet returnSheet, ItemDetalhe itemDetalhe, RegiaoCompra regiaoCompra, Departamento departamento)
        {
            returnSheetItemPrincipal.ReturnSheet = returnSheet;
            returnSheetItemPrincipal.ReturnSheet.RegiaoCompra = regiaoCompra;
            returnSheetItemPrincipal.ReturnSheet.Departamento = departamento;
            returnSheetItemPrincipal.ItemDetalhe = itemDetalhe;
            return returnSheetItemPrincipal;
        }
    }
}
