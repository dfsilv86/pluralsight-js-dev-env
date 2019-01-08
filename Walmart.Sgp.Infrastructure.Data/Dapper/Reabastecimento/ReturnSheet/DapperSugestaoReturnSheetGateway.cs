using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Implementação de um table data gateway para SugestaoReturnSheet utilizando o Dapper.
    /// </summary>
    public class DapperSugestaoReturnSheetGateway : EntityDapperDataGatewayBase<SugestaoReturnSheet>, ISugestaoReturnSheetGateway
    {
        private static String[] s_sugestaoReturnSheetAuditProperties = new String[] { "QtdLoja", "QtdRA", "BlAutorizado", "BlAtivo" };
        private IAuditService m_auditService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperSugestaoReturnSheetGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperSugestaoReturnSheetGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "SugestaoReturnSheet", "IdSugestaoReturnSheet")
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperSugestaoReturnSheetGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        public DapperSugestaoReturnSheetGateway(ApplicationDatabases databases, IAuditService auditService)
            : base(databases.Wlmslp, "SugestaoReturnSheet", "IdSugestaoReturnSheet")
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
                return new string[] { "IdReturnSheetItemLoja", "EstoqueItemVenda", "qtVendorPackageItemCompra", "PackSugeridoCompra", "vlPesoLiquidoItemCompra", "vlTipoAbastetimentoItemCompra", "QtdLoja", "QtdRA", "IdUsuarioCriacao", "DhCriacao", "IdUsuarioAtualizacao", "DhAtualizacao", "IdUsuarioRA", "DhAtualizacaoRA", "BlExportado", "DhExportacao", "BlAtivo", "BlAutorizado", "DhAutorizacao", "IdUsuarioAutorizacao" };
            }
        }

        /// <summary>
        /// Buscar por IdReturnSheetItemLoja.
        /// </summary>
        /// <param name="idReturnSheetItemLoja">O idReturnSheetItemLoja.</param>
        /// <returns>Lista de returnSheetItemLoja.</returns>
        public IEnumerable<SugestaoReturnSheet> ObterPorIdReturnSheetItemLoja(int idReturnSheetItemLoja)
        {
            return this.Find("IdReturnSheetItemLoja = @IdReturnSheetItemLoja", new { IdReturnSheetItemLoja = idReturnSheetItemLoja });
        }

        /// <summary>
        /// Obtém um SugestaoReturnSheet pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade SugestaoReturnSheet.</returns>
        public SugestaoReturnSheet Obter(long id)
        {
            return this.Find("IDSugestaoReturnSheet=@IDSugestaoReturnSheet", new { IDSugestaoReturnSheet = id }).SingleOrDefault();
        }

        /// <summary>
        /// Autoriza a exportação dos registros pesquisados.
        /// </summary>
        /// <param name="dtInicioReturn">A data de início da return sheet.</param>
        /// <param name="dtFinalReturn">A data final da return sheet.</param>
        /// <param name="cdV9D">O código de 9 dígitos do vendor.</param>
        /// <param name="evento">O nome do evento.</param>
        /// <param name="cdItemDetalhe">O código do item detalhe de entrada.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra.</param>
        /// <param name="blExportado">O flag indicando se a sugestão foi exportada.</param>
        /// <param name="blAutorizado">O flag indicando se a sugestão foi autorizada.</param>
        public void AutorizarExportarPlanilhas(DateTime? dtInicioReturn, DateTime? dtFinalReturn, long? cdV9D, string evento, int? cdItemDetalhe, int? cdDepartamento, int? cdLoja, int? idRegiaoCompra, bool? blExportado, bool? blAutorizado)
        {
            var registros = ConsultaReturnSheetLojaRA(dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado, null).Where(r => !r.BlAutorizado.Value);
            var dh = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", RuntimeContext.Current.Culture);
            var id = RuntimeContext.Current.User.Id;

            foreach (var r in registros)
            {
                this.Update("blAutorizado = 1, dhAutorizacao = '{0}', idUsuarioAutorizacao = {1}".With(dh, id), r);
                r.BlAutorizado = true;
                this.m_auditService.LogUpdate(r, s_sugestaoReturnSheetAuditProperties);
            }
        }

        /// <summary>
        /// Obtem Sugestoes para visualização na tela de Consulta Loja.
        /// </summary>
        /// <param name="idDepartamento">O Id do departamento</param>
        /// <param name="idLoja">O ID da loja.</param>
        /// <param name="dataSolicitacao">A data da solicitacao</param>
        /// <param name="evento">Nome do evento</param>
        /// <param name="vendor9D">Codigo vendor 9 digitos</param>
        /// <param name="idItemDetalhe">Id de um Item</param>
        /// <param name="paging">Dados de paginacao</param>
        /// <returns>Uma lista de SugestaoReturnSheet para consulta.</returns>
        public IEnumerable<SugestaoReturnSheet> ConsultaReturnSheetLoja(int idDepartamento, long idLoja, DateTime dataSolicitacao, string evento, long vendor9D, int idItemDetalhe, Paging paging)
        {
            var args = new
            {
                IdDepartamento = idDepartamento,
                DataSolicitacao = dataSolicitacao.ToString("yyyy-MM-dd HH:mm:ss", RuntimeContext.Current.Culture),
                Evento = evento,
                Vendor9D = vendor9D,
                IDItemDetalhe = idItemDetalhe,
                IdLoja = idLoja
            };

            return this.Resource.Query<ReturnSheet, FornecedorParametro, ItemDetalhe, SugestaoReturnSheet, SugestaoReturnSheet>(
                Sql.SugestaoReturnSheet.ConsultaLoja,
                args,
                MapReturnSheetConsultaLoja,
                "SplitOn1,SplitOn2,SplitOn3")
            .AsPaging(paging);
        }

        /// <summary>
        /// Obtem Sugestões para visualização na tela de Consulta Return Sheet RA.
        /// </summary>
        /// <param name="dtInicioReturn">A data de início da return sheet.</param>
        /// <param name="dtFinalReturn">A data final da return sheet.</param>
        /// <param name="cdV9D">O código de 9 dígitos do vendor.</param>
        /// <param name="evento">O nome do evento.</param>
        /// <param name="cdItemDetalhe">O código do item detalhe de entrada.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra.</param>
        /// <param name="blExportado">O flag indicando se a sugestão foi exportada.</param>
        /// <param name="blAutorizado">O flag indicando se a sugestão foi autorizada.</param>
        /// <param name="paging">Dados de paginação.</param>
        /// <returns>Retorna Sugestões para visualização na tela de Consulta Return Sheet RA.</returns>
        public IEnumerable<SugestaoReturnSheet> ConsultaReturnSheetLojaRA(DateTime? dtInicioReturn, DateTime? dtFinalReturn, long? cdV9D, string evento, int? cdItemDetalhe, int? cdDepartamento, int? cdLoja, int? idRegiaoCompra, bool? blExportado, bool? blAutorizado, Paging paging)
        {
            var args = new
            {
                dtInicioReturn = dtInicioReturn,
                dtFinalReturn = dtFinalReturn,
                cdV9D = cdV9D,
                evento = evento,
                cdItemDetalhe = cdItemDetalhe,
                cdDepartamento = cdDepartamento,
                cdLoja = cdLoja,
                idRegiaoCompra = idRegiaoCompra,
                blExportado = blExportado,
                blAutorizado = blAutorizado
            };

            var consolidados = this.Resource.Query<SugestaoReturnSheetConsolidado>(
                Sql.SugestaoReturnSheet.ConsultaReturnSheetRA,
                args);

            if (paging != null)
            {
                return new DapperResult<SugestaoReturnSheetConsolidado, SugestaoReturnSheet>(consolidados.AsPaging(paging), MapConsultaReturnSheetRA);
            }

            return new DapperResult<SugestaoReturnSheetConsolidado, SugestaoReturnSheet>(consolidados, MapConsultaReturnSheetRA);
        }

        /// <summary>
        /// Verifica se existem return sheets vigentes que ainda não foram solicitadas quantidades considerando o papel do usuário e lojas que está logado.
        /// </summary>
        /// <param name="idUsuario">O identificador do usuário.</param>
        /// <param name="idPapel">O identificador do papel.</param>
        /// <param name="idLoja">O identificador da loja.</param>
        /// <returns>Retorna true caso existam return sheets vigentes, do contrário retorna false.</returns>
        public bool PossuiReturnsVigentesQuantidadesVazias(int idUsuario, int idPapel, int idLoja)
        {
            var args = new 
            { 
                idUsuario = idUsuario,
                idPapel = idPapel,
                idLoja = idLoja
            };

            return this.Resource.ExecuteScalar<int>(Sql.SugestaoReturnSheet.PossuiReturnsVigentesQuantidadesVazias, args) > 0;
        }

        private SugestaoReturnSheet MapReturnSheetConsultaLoja(ReturnSheet rs, FornecedorParametro fp, ItemDetalhe id, SugestaoReturnSheet srs)
        {
            id.FornecedorParametro = fp;

            srs.ItemLoja = new ReturnSheetItemLoja()
            {
                ItemPrincipal = new ReturnSheetItemPrincipal()
                {
                    ItemDetalhe = id,
                    ReturnSheet = rs
                }
            };

            return srs;
        }

        private SugestaoReturnSheet MapConsultaReturnSheetRA(SugestaoReturnSheetConsolidado consolidado)
        {
            var ide = new ItemDetalhe()
            {
                CdItem = consolidado.cdItemDetalheEntrada,
                DsItem = consolidado.dsItemDetalheEntrada,
                TpCaixaFornecedor = consolidado.tpCaixaFornecedor,
                VlTipoReabastecimento = consolidado.TipoRA,
            };

            var rs = new ReturnSheet()
            {
                Descricao = consolidado.Descricao,
                DhInicioEvento = consolidado.DhInicioEvento,
                DhFinalEvento = consolidado.DhFinalEvento,
                DhInicioReturn = consolidado.DhInicioReturn,
                DhFinalReturn = consolidado.DhFinalReturn,
                BlAtivo = consolidado.BlAtivo
            };

            return new SugestaoReturnSheet
            {
                IdSugestaoReturnSheet = consolidado.IdSugestaoReturnSheet,
                BlExportado = consolidado.BlExportado,
                qtVendorPackageItemCompra = consolidado.qtVendorPackageItemCompra,
                vlPesoLiquidoItemCompra = consolidado.vlPesoLiquidoItemCompra,
                DhExportacao = consolidado.DhExportacao,
                QtdLoja = consolidado.QtdLoja,
                QtdRA = consolidado.qtdRA,
                DhAtualizacao = consolidado.DhAtualizacao,
                BlAutorizado = consolidado.BlAutorizado,
                DhAutorizacao = consolidado.DhAutorizacao,
                Subtotal = consolidado.Subtotal,
                vlCustoContabilItemVenda = consolidado.vlCustoContabilItemVenda,
                EstoqueItemVenda = consolidado.EstoqueItemVenda,
                BlAtivo = consolidado.SRSBlAtivo,
                PrecoVenda = consolidado.PrecoVenda,
                ItemLoja = new ReturnSheetItemLoja()
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal()
                    {
                        ReturnSheet = rs,
                        ItemDetalhe = ide
                    }
                },
                ReturnSheet = rs,

                Fornecedor = new Fornecedor
                {
                    nmFornecedor = consolidado.nmFornecedor
                },

                FornecedorParametro = new FornecedorParametro
                {
                    cdV9D = consolidado.cdV9D,
                    cdTipo = consolidado.cdTipo
                },

                ItemDetalheEntrada = ide,

                ItemDetalheSaida = new ItemDetalhe
                {
                    CdItem = consolidado.cdItemDetalheSaida,
                    DsItem = consolidado.dsItemDetalheSaida,
                },

                UsuarioAutorizacao = new Domain.Acessos.Usuario
                {
                    FullName = consolidado.UsuarioAutorizacao
                },

                UsuarioAtualizacao = new Domain.Acessos.Usuario
                {
                    FullName = consolidado.UsuarioAtualizacao
                },

                Loja = new Domain.EstruturaMercadologica.Loja
                {
                    cdLoja = consolidado.cdLoja
                }
            };
        }
    }
}