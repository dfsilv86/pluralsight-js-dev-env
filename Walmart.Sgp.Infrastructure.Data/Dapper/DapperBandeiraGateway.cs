using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para bandeira utilizando o Dapper.
    /// </summary>
    public class DapperBandeiraGateway : EntityDapperDataGatewayBase<Bandeira>, IBandeiraGateway
    {
        #region Fields
        private DapperBandeiraDetalheGateway m_detalheGateway;
        private DapperRegiaoGateway m_regiaoGateway;
        private DapperDistritoGateway m_distritoGateway;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperBandeiraGateway"/>.
        /// </summary>
        /// <param name="databases">A data de .</param>
        public DapperBandeiraGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Bandeira", "IDBandeira")
        {
            m_detalheGateway = new DapperBandeiraDetalheGateway(databases);
            m_regiaoGateway = new DapperRegiaoGateway(databases);
            m_distritoGateway = new DapperDistritoGateway(databases);
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
                return new string[] { "dsBandeira", "sgBandeira", "tpCusto", "cdSistema", "blAtivo", "dhCriacao", "dhAtualizacao", "cdUsuarioCriacao", "cdUsuarioAtualizacao", "blImportarTodos", "IDFormato" };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém as bandeiras associadas ao sistema informado, que o usuário informado tem acesso.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="idSistema">O id do sistema.</param>
        /// <param name="idFormato">O id do formato.</param>
        /// <param name="idRegiaoAdministrativa">O id da região administrativa.</param>
        /// <returns>A lista de bandeiras ativas do sistema informado, que o usuário tem acesso.</returns>
        public IEnumerable<BandeiraResumo> ObterPorUsuarioESistema(int idUsuario, int? idSistema, int? idFormato, int? idRegiaoAdministrativa)
        {
            return this.Resource.Query<BandeiraResumo>(Sql.Bandeira.ObterPorUsuarioESistema, new { IDUsuario = idUsuario, cdSistema = idSistema, idFormato, IDRegiaoAdministrativa = idRegiaoAdministrativa });
        }

        /// <summary>
        /// Pesquisa bandeiras pelo filtro informado.
        /// </summary>
        /// <param name="filtro">O filtro de bandeira.</param>
        /// <param name="paging">A paginação.</param>
        /// <returns>As bandeiras.</returns>
        public IEnumerable<Bandeira> PesquisarPorFiltros(BandeiraFiltro filtro, Paging paging)
        {
            var user = RuntimeContext.Current.User;

            return this.Resource.Query<Bandeira>(
                Sql.Bandeira.PesquisarPorFiltros_Paging,
                new
                {
                    IDUsuario = user.Id,
                    tipoPermissao = user.TipoPermissao,
                    cdSistema = filtro.CdSistema,
                    dsBandeira = filtro.DsBandeira,
                    idFormato = filtro.IDFormato
                }).AsPaging(paging, Sql.Bandeira.PesquisarPorFiltros_Paging, Sql.Bandeira.PesquisarPorFiltros_Count);
        }

        /// <summary>
        /// Obtém a bandeira estruturada pelo id.
        /// </summary>
        /// <param name="id">O id da bandeira desejada.</param>
        /// <returns>A bandeira.</returns>
        public Bandeira ObterEstruturadoPorId(int id)
        {
            Bandeira bandeira = null;
            var detalhes = new List<BandeiraDetalhe>();
            var regioes = new List<Regiao>();
            var regioesDistritos = new Dictionary<int, List<Distrito>>();

            Resource.Query<Bandeira, BandeiraDetalhe, Departamento, Categoria, Regiao, Distrito, Bandeira>(
                Sql.Bandeira.ObterEstruturadoPorId,
                new { id },
                (b, bd, d, c, r, distrito) =>
                {
                    if (bandeira == null)
                    {
                        bandeira = b;
                        b.Detalhes = detalhes;
                        b.Regioes = regioes;
                    }

                    if (!bd.IsNew && !detalhes.Contains(bd))
                    {
                        detalhes.Add(bd);
                        bd.Departamento = d.IsNew ? null : d;
                        bd.Categoria = c.IsNew ? null : c;
                    }

                    if (!r.IsNew && !regioes.Contains(r))
                    {
                        var temp = new List<Distrito>();
                        r.Distritos = temp;
                        regioesDistritos.Add(r.Id, temp);
                        regioes.Add(r);
                    }

                    if (!distrito.IsNew)
                    {
                        var distritos = regioesDistritos[r.Id];

                        if (!distritos.Contains(distrito))
                        {
                            distritos.Add(distrito);
                        }
                    }

                    return bandeira;
                },
                "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5")
                .Perform();

            return bandeira;
        }

        /// <summary>
        /// Obtém a bandeira pelo id da loja.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>Retorna a bandeira.</returns>
        public Bandeira ObterPorIdLoja(int idLoja)
        {
            return this.Resource.Query<Bandeira>(Sql.Bandeira.ObterPorIdLoja, new { idLoja }).SingleOrDefault();
        }

        /// <summary>
        /// Insere uma nova bandeira, seus detalhes, regiões e distritos.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>                
        public override void Insert(Bandeira entity)
        {
            base.Insert(entity);

            ChildrenHelper.Insert(
               entity.Detalhes,
               m_detalheGateway,
               (s) => s.IDBandeira = entity.IDBandeira);

            ChildrenHelper.Insert(
               entity.Regioes,
               m_regiaoGateway,
               (r) => r.IDBandeira = entity.IDBandeira);

            foreach (var r in entity.Regioes)
            {
                ChildrenHelper.Insert(
                r.Distritos,
                m_distritoGateway,
                (d) => d.IDRegiao = r.IDRegiao);
            }
        }

        /// <summary>
        /// Atualiza uma bandeira, seus detalhes, regiões e distritos.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        public override void Update(Bandeira entity)
        {
            var oldEntity = ObterEstruturadoPorId(entity.Id);

            entity.CdUsuarioCriacao = oldEntity.CdUsuarioCriacao;
            entity.DhCriacao = oldEntity.DhCriacao;
            base.Update(entity);

            ChildrenHelper.SyncNoUpdate(
               oldEntity.Detalhes,
               entity.Detalhes,
               m_detalheGateway,
               (s) => s.IDBandeira = entity.IDBandeira);

            RemoverDistritosRegioes(entity, oldEntity);
            InserirRegioesDistritos(entity, oldEntity);
            AtualizarRegioesDistritos(entity, oldEntity);
        }

        /// <summary>
        /// Exclui uma bandeira, seus detalhes, regiões e distritos.
        /// </summary>
        /// <param name="id">O id da entidade existente e que se deseja excluir.</param>
        public override void Delete(int id)
        {
            var bandeira = ObterEstruturadoPorId(id);

            ChildrenHelper.Delete(bandeira.Detalhes, m_detalheGateway);

            var distritosToRemove = bandeira.Regioes.SelectMany(d => d.Distritos);
            ChildrenHelper.Delete(distritosToRemove, m_distritoGateway);
            ChildrenHelper.Delete(bandeira.Regioes, m_regiaoGateway);

            base.Delete(id);
        }

        private void SyncDistritos(Bandeira oldEntity, Regiao regiao)
        {
            var oldRegiao = oldEntity.Regioes.FirstOrDefault(r => r.IDRegiao == regiao.IDRegiao) ?? new Regiao();
            var distritosToDelete = ChildrenHelper.GetItemsToDelete(oldRegiao.Distritos, regiao.Distritos);
            var distritosToInsert = ChildrenHelper.GetItemsToInsert(oldRegiao.Distritos, regiao.Distritos);

            foreach (var distrito in distritosToDelete)
            {
                m_distritoGateway.Delete(distrito.Id);
            }

            foreach (var distrito in distritosToInsert)
            {
                distrito.IDRegiao = regiao.IDRegiao;
                m_distritoGateway.Insert("IDRegiao, nmDistrito", distrito);
            }
        }

        private void AtualizarRegioesDistritos(Bandeira entity, Bandeira oldEntity)
        {
            // Atualiza regiões e insere/atualiza distritos.
            var regioesToUpdate = ChildrenHelper.GetItemsToUpdate(oldEntity.Regioes, entity.Regioes);

            foreach (var regiao in regioesToUpdate)
            {
                SyncDistritos(oldEntity, regiao);
            }
        }

        private void InserirRegioesDistritos(Bandeira entity, Bandeira oldEntity)
        {
            // Insere regiões e distritos.
            var regioesToInsert = ChildrenHelper.GetItemsToInsert(oldEntity.Regioes, entity.Regioes);

            foreach (var regiao in regioesToInsert)
            {
                regiao.IDBandeira = entity.IDBandeira;
                m_regiaoGateway.Insert(regiao);
                SyncDistritos(oldEntity, regiao);
            }
        }

        private void RemoverDistritosRegioes(Bandeira entity, Bandeira oldEntity)
        {
            // Remover distritos e regiões.
            var regioesToDelete = ChildrenHelper.GetItemsToDelete(oldEntity.Regioes, entity.Regioes);

            foreach (var regiao in regioesToDelete)
            {
                foreach (var distrito in regiao.Distritos)
                {
                    m_distritoGateway.Delete(distrito.Id);
                }

                m_regiaoGateway.Delete(regiao.Id);
            }
        }
        #endregion
    }
}
