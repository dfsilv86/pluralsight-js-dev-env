using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para Roteiro utilizando o Dapper.
    /// </summary>
    public class DapperRoteiroGateway : EntityDapperDataGatewayBase<Roteiro>, IRoteiroGateway
    {
        private static String[] s_roteiroAuditProperties = new String[] { "Descricao", "vlCargaMinima", "blKgCx", "blAtivo", "cdV9D" };
        private static String[] s_roteiroLojaAuditProperties = new String[] { "idRoteiro", "idloja", "blativo" };
        private readonly DapperRoteiroLojaGateway m_roteiroLojaGateway;
        private readonly IAuditService m_auditService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRoteiroGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        public DapperRoteiroGateway(ApplicationDatabases databases, IAuditService auditService)
            : base(databases.Wlmslp, "Roteiro", "IDRoteiro")
        {
            m_roteiroLojaGateway = new DapperRoteiroLojaGateway(databases);
            m_auditService = auditService;
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperRoteiroGateway" />.
        /// </summary>
        /// <param name="databases">O registro de bases de dados da aplicação.</param>
        public DapperRoteiroGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Roteiro", "IDRoteiro")
        {
        }
        
        /// <summary>
        /// Obtém o nome das colunas que devem ser consideradas nas operações de SELECT, INSERT e UPDATE.
        /// </summary>
        protected override IEnumerable<string> ColumnsName
        {
            get
            {
                return new string[] { "Descricao", "vlCargaMinima", "blKgCx", "idUsuarioCriacao", "dhCriacao", "idUsuarioAtualizacao", "dhAtualizacao", "blAtivo", "cdV9D" };
            }
        }

        /// <summary>
        /// Obtém um Roteiro estruturado pelo seu id.
        /// </summary>
        /// <param name="id">O id.</param>
        /// <returns>A entidade Roteiro.</returns>
        public Roteiro ObterEstruturadoPorId(int id)
        {
            return Resource.Query<Roteiro, Fornecedor, Roteiro>(
                Sql.Roteiro.ObterPorId,
                new { idRoteiro = id },
                (roteiro, fornecedor) =>
                {
                    roteiro.Fornecedor = fornecedor;
                    return roteiro;
                },
                "SplitOn1").SingleOrDefault();
        }

        /// <summary>
        /// Obtém os roteiros dos fornecedores.
        /// </summary>
        /// <param name="cdV9D">O código 9 dígitos do fornecedor.</param>
        /// <param name="cdDepartamento">O código do departamento.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="roteiro">O nome do roteiro.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>A lista contendo os roteiros dos fornecedores.</returns>
        public IEnumerable<Roteiro> ObterRoteirosPorFornecedor(long? cdV9D, int? cdDepartamento, int? cdLoja, string roteiro, Paging paging)
        {
            return Resource.Query<Fornecedor, Roteiro, Roteiro>(
                Sql.Roteiro.ObterRoteirosPorFornecedor,
                new { cdV9D, cdDepartamento, cdLoja, roteiro },
                MapObterRoteirosPorFornecedor,
                "SplitOn1").AsPaging(paging);
        }

        /// <summary>
        /// Insere um novo roteiro e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">O novo roteiro a ser inserido.</param>
        /// <remarks>
        /// Um novo roteiro será criado no banco de dados.
        /// </remarks>
        public override void Insert(Roteiro entity)
        {
            base.Insert(entity);

            ChildrenHelper.Insert(
                entity.Lojas,
                m_roteiroLojaGateway,
                (s) => s.idRoteiro = entity.Id);

            m_auditService.LogInsert(entity, s_roteiroAuditProperties);

            foreach (var loja in entity.Lojas)
            {
                m_auditService.LogInsert(loja, s_roteiroLojaAuditProperties);
            }
        }

        /// <summary>
        /// Atualiza um roteiro existente.
        /// </summary>
        /// <param name="entity">O roteiro a ser atualizado. Deve possuir a propriedade Id preenchida.</param>
        /// <remarks>
        /// Será atualizado um roteiro já existente no banco.
        /// </remarks>
        public override void Update(Roteiro entity)
        {
            var oldEntity = FindById(entity.Id);
            base.Update(entity);

            ChildrenHelper.Sync(
                oldEntity.Lojas.Intersect(entity.Lojas),
                entity.Lojas,
                m_roteiroLojaGateway,
                (s) => s.idRoteiro = entity.Id);

            RegistrarLogRoteiro(entity, oldEntity);
            RegistrarLogRoteiroLojaInsercao(entity, oldEntity);
            RegistrarLogRoteiroLojaAtualizacao(entity, oldEntity);
        }

        /// <summary>
        /// Exclui um roteiro.
        /// </summary>
        /// <param name="id">O id do roteiro existente e que se deseja excluir.</param>
        /// <remarks>
        /// Um roteiro será excluído do banco de dados.
        /// </remarks>   
        public override void Delete(int id)
        {
            var oldEntity = FindById(id);
            oldEntity.blAtivo = false;
            oldEntity.Lojas.ToList().ForEach(l => l.blativo = false);

            this.m_auditService.LogDelete(oldEntity, s_roteiroAuditProperties);
            base.Update(oldEntity);

            foreach (var loja in oldEntity.Lojas) 
            {
                loja.blativo = false;
                this.m_auditService.LogDelete(loja, s_roteiroLojaAuditProperties);
                m_roteiroLojaGateway.Update(loja);
            }
        }

        /// <summary>
        /// Pesquisa um roteiro pelo id.
        /// </summary>
        /// <param name="id">O id do roteiro desejado.</param>
        /// <returns>O roteiro caso exista um com id informado, caso contrário null.</returns>
        public override Roteiro FindById(int id)
        {
            Roteiro result = null;

            this.Resource.Query<Roteiro, RoteiroLoja, Roteiro>(
                Sql.Roteiro.ObterPorIdComLojas,
                new { idRoteiro = id },
                (roteiro, roteiroLoja) =>
                {
                    if (result == null)
                    {
                        result = roteiro;
                    }

                    if (!result.Lojas.Contains(roteiroLoja))
                    {
                        result.Lojas.Add(roteiroLoja);
                    }

                    return result;
                },
                "SplitOn1").ToList();

            return result;
        }

        /// <summary>
        /// Obtém uma lista de SugestaoPedido com Loja populada
        /// </summary>
        /// <param name="idRoteiro">O id do roteiro.</param>
        /// <param name="dtPedido">A data do pedido.</param>
        /// <param name="idItemDetalhe">O id do item.</param>
        /// <param name="paging">A paginação. (OPCIONAL)</param>
        /// <returns>Uma lista de sugestaoPedido.</returns>
        public IEnumerable<SugestaoPedido> ObterSugestaoPedidoLoja(int idRoteiro, DateTime dtPedido, int idItemDetalhe, Paging paging)
        {
            var args = new
            {
                idRoteiro = idRoteiro,
                idItemDetalhe = idItemDetalhe,
                dtPedido = dtPedido.Date.ToString("yyyy-MM-dd HH:mm:ss", RuntimeContext.Current.Culture)
            };

            if (paging != null)
            {
                return Resource.Query<Loja, SugestaoPedido, ItemDetalhe, SugestaoPedido>(
                Sql.Roteiro.ObterItemLojaRoteiroDetalhe,
                args,
                MapSugestaoPedidoDetalhe,
                "SplitOn1,SplitOn2").AsPaging(paging);
            }

            return Resource.Query<Loja, SugestaoPedido, ItemDetalhe, SugestaoPedido>(
                Sql.Roteiro.ObterItemLojaRoteiroDetalhe,
                args,
                MapSugestaoPedidoDetalhe,
                "SplitOn1,SplitOn2");
        }

        private SugestaoPedido MapSugestaoPedidoDetalhe(Loja l, SugestaoPedido sp, ItemDetalhe id)
        {
            sp.ItemDetalhePedido = id;
            sp.Loja = l;
            return sp;
        }

        private Roteiro MapObterRoteirosPorFornecedor(Fornecedor fornecedor, Roteiro roteiro)
        {
            roteiro.Fornecedor = fornecedor;
            return roteiro;
        }

        private void RegistrarLogRoteiro(Roteiro entity, Roteiro oldEntity)
        {
            if (entity.Descricao != oldEntity.Descricao
                || entity.vlCargaMinima != oldEntity.vlCargaMinima
                || entity.blKgCx != oldEntity.blKgCx
                || entity.blAtivo != oldEntity.blAtivo)
            {
                m_auditService.LogUpdate(entity, s_roteiroAuditProperties);
            }
        }

        private void RegistrarLogRoteiroLojaInsercao(Roteiro entity, Roteiro oldEntity)
        {
            var novasLojas = entity.Lojas.Except(oldEntity.Lojas);

            foreach (var loja in novasLojas)
            {
                m_auditService.LogInsert(loja, s_roteiroLojaAuditProperties);
            }
        }

        private void RegistrarLogRoteiroLojaAtualizacao(Roteiro entity, Roteiro oldEntity)
        {
            var lojasAtualizadas = entity.Lojas.Intersect(oldEntity.Lojas);

            foreach (var loja in lojasAtualizadas)
            {
                var oldLoja = oldEntity.Lojas.Single(ol => ol == loja);

                if (oldLoja.blativo != loja.blativo)
                {
                    m_auditService.LogUpdate(loja, s_roteiroLojaAuditProperties);
                }
            }
        }
    }
}
