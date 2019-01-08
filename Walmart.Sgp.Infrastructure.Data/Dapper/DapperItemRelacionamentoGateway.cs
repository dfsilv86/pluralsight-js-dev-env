using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para relacionamento de itens usando o Dapper.
    /// </summary>
    public class DapperItemRelacionamentoGateway : EntityDapperDataGatewayBase<RelacionamentoItemPrincipal>, IItemRelacionamentoGateway
    {
        #region Fields
        private readonly DapperRelacionamentoItemSecundarioGateway m_secundarioGateway;
        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperItemRelacionamentoGateway"/>.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperItemRelacionamentoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "RelacionamentoItemPrincipal", "IDRelacionamentoItemPrincipal")
        {
            m_secundarioGateway = new DapperRelacionamentoItemSecundarioGateway(databases);
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
                    "cdSistema",
                    "IDTipoRelacionamento",
                    "IDItemDetalhe",
                    "IDDepartamento",
                    "IDCategoria",
                    "qtProdutoBruto",
                    "pcRendimentoReceita",
                    "qtProdutoAcabado",
                    "pcQuebra",
                    "dhCadastro",
                    "dhAlteracao",
                    "psUnitario",
                    "blReprocessamentoManual",
                    "statusReprocessamentoCusto",
                    "dtInicioReprocessamentoCusto",
                    "dtFinalReprocessamentoCusto",
                    "idUsuarioReprocessamento",
                    "descErroReprocessamento",
                    "idUsuarioAlteracao"
                };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Pesquisa relacionamentos por informações dos itens, departamento e sistema.
        /// </summary>
        /// <param name="tipoRelacionamento">O tipo de relacionamento.</param>
        /// <param name="dsItem">Descrição do item.</param>
        /// <param name="cdItem">O código do item (cdItem).</param>
        /// <param name="cdFineLine">O código do fineline.</param>
        /// <param name="cdSubcategoria">O código da subcategoria.</param>
        /// <param name="cdCategoria">O código da categoria.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idRegiaoCompra">O identificador da região de compra</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>Os relacionamentos (item principal e itens secundários).</returns>
        public IEnumerable<RelacionamentoItemPrincipal> PesquisarPorTipoRelacionamento(TipoRelacionamento tipoRelacionamento, string dsItem, int? cdItem, int? cdFineLine, int? cdSubcategoria, int? cdCategoria, int? cdDepartamento, int cdSistema, int? idRegiaoCompra, Paging paging)
        {
            var args = new
            {
                cdItem,
                dsItem,
                cdSistema,
                cdDepartamento,
                cdCategoria,
                cdSubcategoria,
                cdFineLine,
                idTipoRelacionamento = tipoRelacionamento.Value,
                idRegiaoCompra
            };

            // TODO: remover IDCategoria e IDDepartamento da RelacionamentoItemPrincipal quando possivel (deve usar a estrutura mercadológica do item principal)
            return this.Resource.Query<RelacionamentoItemPrincipal, ItemDetalhe, Departamento, Categoria, Subcategoria, FineLine, RelacionamentoItemPrincipal>(
                Sql.RelacionamentoItemPrincipal.PesquisarPorTipoRelacionamento,
                args,
                (rip, id, dep, cat, sub, fin) =>
                {
                    rip.ItemDetalhe = id;

                    if (null != id)
                    {
                        rip.IDItemDetalhe = id.IDItemDetalhe;

                        MapEstruturaMercadologica(id, dep, cat, sub, fin);
                    }

                    return rip;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5").AsPaging(paging);
        }

        /// <summary>
        /// Determina o percentual de rendimento transformado.
        /// </summary>
        /// <param name="idItemDetalhe">Id do item detalhe.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O percentual.</returns>
        /// <remarks>Determinado a partir da tabela RelacionamentoItemPrincipal.</remarks>
        public decimal? ObterPercentualRendimentoTransformado(int idItemDetalhe, byte cdSistema)
        {
            return this.Resource.ExecuteScalar<decimal?>(Sql.RelacionamentoItemPrincipal.ObterPercentualRendimentoTransformado, new { idItemDetalhe, cdSistema });
        }

        /// <summary>
        /// Determina o percentual de rendimento derivado.
        /// </summary>
        /// <param name="idItemDetalhe">Id do item detalhe.</param>
        /// <param name="cdSistema">O código da estrutura mercadológica.</param>
        /// <returns>O percentual.</returns>
        /// <remarks>Determinado a partir da tabela RelacionamentoItemSecundario.</remarks>
        public decimal? ObterPercentualRendimentoDerivado(int idItemDetalhe, byte cdSistema)
        {
            return m_secundarioGateway.ObterPercentualRendimentoDerivado(idItemDetalhe, cdSistema);
        }

        /// <summary>
        /// Conta o número de vezes que um item detalhe foi utilizado como saída em outros relacionamentos.
        /// </summary>
        /// <param name="idRelacionamentoItemPrincipalCorrente">O id do relacionamento item principal corrente.</param>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>A contagem.</returns>
        public int ContarItemDetalheComoSaidaEmOutrosRelacionamentos(int idRelacionamentoItemPrincipalCorrente, int idItemDetalhe)
        {
            return Resource.ExecuteScalar<int>(
                Sql.RelacionamentoItemPrincipal.ContarItemDetalheComoSaidaEmOutrosRelacionamentos, 
                new { idRelacionamentoItemPrincipalCorrente, idItemDetalhe });
        }

        /// <summary>
        /// Conta o número de vezes que um item detalhe foi utilizado em outros relacionamentos.
        /// </summary>
        /// <param name="idRelacionamentoItemPrincipalCorrente">O id do relacionamento item principal corrente.</param>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <param name="tipoRelacionamento">O tipo de relacionamento a ser considerado.</param>
        /// <returns>A contagem.</returns>
        public int ContarItemDetalheEmOutrosRelacionamentos(int idRelacionamentoItemPrincipalCorrente, int idItemDetalhe, TipoRelacionamento tipoRelacionamento)
        {
            return Resource.ExecuteScalar<int>(
                Sql.RelacionamentoItemPrincipal.ContarItemDetalheEmOutrosRelacionamentos, 
                new { idRelacionamentoItemPrincipalCorrente, idItemDetalhe, tipoRelacionamento });
        }

        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        public override void Insert(RelacionamentoItemPrincipal entity)
        {
            base.Insert(entity);

            ChildrenHelper.Insert(
                entity.RelacionamentoSecundario,
                m_secundarioGateway,
                (s) => s.IDRelacionamentoItemPrincipal = entity.Id);
        }

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <exception cref="InvalidOperationException">Caso o registro a ser atualizado não exista.</exception>
        /// <remarks>
        /// Será atualizado um registro já existente no banco.
        /// </remarks>
        public override void Update(RelacionamentoItemPrincipal entity)
        {
            var oldEntity = FindById(entity.Id);
            base.Update(entity);

            ChildrenHelper.Sync(
                oldEntity.RelacionamentoSecundario,
                entity.RelacionamentoSecundario,
                m_secundarioGateway,
                (s) => s.IDRelacionamentoItemPrincipal = entity.Id);
        }

        /// <summary>
        /// Exclui uma entidade.
        /// </summary>
        /// <param name="id">O id da entidade existente e que se deseja excluir.</param>
        /// <exception cref="InvalidOperationException">Caso o registro a ser excluído não exista.</exception>
        /// <remarks>
        /// Um registro será excluído do banco de dados.
        /// </remarks>   
        public override void Delete(int id)
        {
            var oldEntity = FindById(id);
            ChildrenHelper.Delete(oldEntity.RelacionamentoSecundario, m_secundarioGateway);
            base.Delete(id);
        }

        /// <summary>
        /// Obtém pelo id.
        /// </summary>
        /// <param name="id">O id da entidade desejada.</param>
        /// <returns>A entidade.</returns>
        public override RelacionamentoItemPrincipal FindById(int id)
        {
            RelacionamentoItemPrincipal result = null;

            this.Resource.Query<RelacionamentoItemPrincipal, ItemDetalhe, Departamento, RelacionamentoItemSecundario, ItemDetalhe, RegiaoCompra, AreaCD, RelacionamentoItemPrincipal>(
                Sql.RelacionamentoItemPrincipal.ObterPorId,
                new { idRelacionamentoItemPrincipal = id },
                (principal, itemDetalhePrincipal, departamento, secundario, itemDetalheSecundario, regiaoCompra, areaCD) =>
                {
                    if (result == null)
                    {
                        result = principal;
                        itemDetalhePrincipal.Departamento = departamento;
                        itemDetalhePrincipal.RegiaoCompra = regiaoCompra;
                        itemDetalhePrincipal.AreaCD = areaCD;
                        result.ItemDetalhe = itemDetalhePrincipal;
                    }

                    if (!result.RelacionamentoSecundario.Contains(secundario))
                    {
                        secundario.ItemDetalhe = itemDetalheSecundario;
                        result.RelacionamentoSecundario.Add(secundario);
                    }

                    return result;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,SplitOn6").ToList();

            return result;
        }

        /// <summary>
        /// Obtém os relacionamentos onde o item participa como principal.
        /// </summary>
        /// <param name="idItemDetalhe">O id de item detalhe.</param>
        /// <returns>Os relacionamentos principais onde o item é o principal, e seus secundários.</returns>
        public IEnumerable<RelacionamentoItemPrincipal> ObterPrincipaisPorItem(int idItemDetalhe)
        {
            Dictionary<int, RelacionamentoItemPrincipal> rips = new Dictionary<int, RelacionamentoItemPrincipal>();
            Dictionary<int, ItemDetalhe> ids = new Dictionary<int, ItemDetalhe>();

            this.Resource.Query<RelacionamentoItemPrincipal, ItemDetalhe, RelacionamentoItemSecundario, ItemDetalhe, RelacionamentoItemPrincipal>(
                Sql.RelacionamentoItemPrincipal.ObterPrincipaisPorItem,
                new { idItemDetalhe },
                (rip, id1, ris, id2) =>
                {
                    if (!rips.ContainsKey(rip.IDRelacionamentoItemPrincipal))
                    {
                        rips[rip.IDRelacionamentoItemPrincipal] = rip;
                    }

                    RelacionamentoItemPrincipal principal = rips[rip.IDRelacionamentoItemPrincipal];

                    if (!ids.ContainsKey(principal.IDItemDetalhe))
                    {
                        ids[principal.IDItemDetalhe] = id1;
                    }

                    if (ris.IDItemDetalhe.HasValue && !ids.ContainsKey(ris.IDItemDetalhe.Value))
                    {
                        ids[ris.IDItemDetalhe.Value] = id2;
                    }

                    principal.ItemDetalhe = ids[principal.IDItemDetalhe];

                    if (ris.IDItemDetalhe.HasValue)
                    {
                        ris.ItemDetalhe = ids[ris.IDItemDetalhe.Value];
                    }

                    principal.RelacionamentoSecundario.Add(ris);
                    return principal;
                },
                "SplitOn1,SplitOn2,SplitOn3").Perform();

            return rips.Values;
        }

        /// <summary>
        /// Pesquisa informações sobre itens relacionados ao item informado.
        /// </summary>
        /// <param name="cdItem">O código do item.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>Informações sobre itens relacionados.</returns>
        public ItensRelacionadosResponse ObterItensRelacionados(int cdItem, int? idLoja)
        {
            var result = this.StoredProcedure.QueryMultiple<dynamic, dynamic, dynamic, dynamic, dynamic>("PR_SelecionarRelacionamentos", new { cdItem, idLoja });

            var temp = new IEnumerable<dynamic>[] { result.Item1, result.Item2, result.Item3, result.Item4, result.Item5 };

            return new ItensRelacionadosResponse(
                temp.Where(x => x.Count() > 0 && x.First().Tipo == "Entrada").SingleOrDefault(),
                temp.Where(x => x.Count() > 0 && x.First().Tipo == "Derivado").SingleOrDefault(),
                temp.Where(x => x.Count() > 0 && x.First().Tipo == "Insumo").SingleOrDefault(),
                temp.Where(x => x.Count() > 0 && x.First().Tipo == "Saida").SingleOrDefault(),
                temp.Where(x => x.Count() > 0 && x.First().Tipo == "Transformado").SingleOrDefault());
        }

        /// <summary>
        /// Mapeia a estrutura mercadologica.
        /// </summary>
        /// <param name="itemDetalhe">O ItemDetalhe.</param>
        /// <param name="departamento">O Departamento.</param>
        /// <param name="categoria">A Categoria.</param>
        /// <param name="subcategoria">A Subcategoria.</param>
        /// <param name="fineLine">O Fineline.</param>
        private static void MapEstruturaMercadologica(ItemDetalhe itemDetalhe, Departamento departamento, Categoria categoria, Subcategoria subcategoria, FineLine fineLine)
        {
            // TODO: reaproveitar isso na ItemDetalhe
            itemDetalhe.Subcategoria = subcategoria;
            itemDetalhe.FineLine = fineLine;
            itemDetalhe.Categoria = categoria;
            itemDetalhe.Departamento = departamento;

            if (null != fineLine)
            {
                fineLine.IDFineLine = (int)itemDetalhe.IDFineline; // TODO: resolver o problema das PKs com BIGINT
            }

            if (null != subcategoria)
            {
                subcategoria.IDSubcategoria = (int)itemDetalhe.IDSubcategoria; // TODO: resolver o problema das PKs com BIGINT
            }

            if (null != categoria)
            {
                categoria.IDCategoria = (int)itemDetalhe.IDCategoria; // TODO: resolver o problema das PKs com BIGINT        
            }

            if (null != departamento)
            {
                departamento.IDDepartamento = itemDetalhe.IDDepartamento;
            }
        }

        #endregion
    }
}
