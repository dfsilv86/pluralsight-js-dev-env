using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Databases;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Implementação de um table data gateway para permissão utilizando o Dapper.
    /// </summary>
    public class DapperPermissaoGateway : EntityDapperDataGatewayBase<Permissao>, IPermissaoGateway
    {
        #region Fields
        private DapperPermissaoBandeiraGateway m_permissaoBandeiraGateway;
        private DapperPermissaoLojaGateway m_permissaoLojaGateway;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DapperPermissaoGateway"/>.
        /// </summary>
        /// <param name="databases">Os bancos de dados da aplicação.</param>
        public DapperPermissaoGateway(ApplicationDatabases databases)
            : base(databases.Wlmslp, "Permissao", "IDPermissao")
        {
            m_permissaoBandeiraGateway = new DapperPermissaoBandeiraGateway(databases);
            m_permissaoLojaGateway = new DapperPermissaoLojaGateway(databases);
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
                return new string[] { "IDUsuario", "IDUsuarioCriacao", "dhCriacao", "IDUsuarioAlteracao", "dhAlteracao", "blAdministrador", "blRecebeNotificaoOperacoes", "blRecebeNotificaoFinanceiro" };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Insere uma nova entidade e preenche a propriedade Id do novo registro criado.
        /// </summary>
        /// <param name="entity">A nova entidade a ser inserida.</param>
        /// <remarks>
        /// Um novo registro será criado no banco de dados.
        /// </remarks>
        public override void Insert(Permissao entity)
        {
            base.Insert(entity);

            ChildrenHelper.Insert(
               entity.Bandeiras,
               m_permissaoBandeiraGateway,
               (b) => b.IDPermissao = entity.IDPermissao);

            ChildrenHelper.Insert(
                entity.Lojas,
                m_permissaoLojaGateway,
                (b) => b.IDPermissao = entity.IDPermissao);
        }

        /// <summary>
        /// Insere as novas entidade em lote, mas, por razões de performance, não preenche as propriedades Id dos novos registros criados.
        /// </summary>
        /// <param name="entities">As entidades a serem inseridas.</param>
        /// <remarks>
        /// Novos registros serão criados no banco.
        /// </remarks>
        public override void Insert(IEnumerable<Permissao> entities)
        {
            throw new NotImplementedException(Texts.MultipleInsertNotSupporteOnDapperGatewayWithChildrenEntities);
        }

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <param name="entity">A entidade a ser atualizada. Deve possuir a propriedade Id preenchida.</param>
        /// <remarks>
        /// Será atualizado um registro já existente no banco.
        /// </remarks>
        public override void Update(Permissao entity)
        {
            var oldEntity = Find("IdPermissao = @IdPermissao", entity).Single();
            base.Update(entity);

            ChildrenHelper.Sync(
                oldEntity.Bandeiras,
                entity.Bandeiras,
                m_permissaoBandeiraGateway,
                (b) => b.IDPermissao = entity.IDPermissao);

            ChildrenHelper.Sync(
                oldEntity.Lojas,
                entity.Lojas,
                m_permissaoLojaGateway,
                (b) => b.IDPermissao = entity.IDPermissao);
        }

        /// <summary>
        /// Exclui uma entidade.
        /// </summary>
        /// <param name="id">O id da entidade existente e que se deseja excluir.</param>
        /// <remarks>
        /// Um registro será excluído do banco de dados.
        /// </remarks>
        public override void Delete(int id)
        {
            var oldEntity = Find("IdPermissao = @Id", new { Id = id }).Single();

            foreach (var b in oldEntity.Bandeiras)
            {
                m_permissaoBandeiraGateway.Delete(b.Id);
            }

            foreach (var l in oldEntity.Lojas)
            {
                m_permissaoLojaGateway.Delete(l.Id);
            }

            base.Delete(id);
        }

        /// <summary>
        /// Exclui as entidades que corresponderem ao filtro informado.
        /// </summary>
        /// <param name="filter">A cláusula WHERE definindo quais entidades serão excluídas. Exemplo: Name = @Name.</param>
        /// <param name="filterArgs">O objeto anônimo com os argumentos para o filtro. Exemplo: new { Name = "Test" }.</param>
        /// <remarks>
        /// Excluirá todos os registros que corresponderem ao filtro.
        /// </remarks>
        public override void Delete(string filter, object filterArgs)
        {
            throw new NotImplementedException(Texts.MultipleDeleteNotSupporteOnDapperGatewayWithChildrenEntities1);
        }

        /// <summary>
        /// Retorna todas as entidades.
        /// </summary>
        /// <returns>
        /// Todas entidades.
        /// </returns>
        public override IEnumerable<Permissao> FindAll()
        {
            return FillChildren(base.FindAll());
        }

        /// <summary>
        /// Pesquisa as entidades que correspondem ao filtro informado.
        /// </summary>
        /// <param name="filter">O filtro no formato: Username = @Username AND Active = @Active. Expressões válidas em uma cláusula where SQL são aceitas, como LIKE.</param>
        /// <param name="filterArgs">Os argumentos para o filtro. Pode ser um objeto concreto ou um objeto anônimo.</param>
        /// <returns>
        /// As entidades localizadas.
        /// </returns>
        public override IEnumerable<Permissao> Find(string filter, object filterArgs)
        {
            return FillChildren(base.Find(filter, filterArgs));
        }

        /// <summary>
        /// Pesquisa permissões utilizando os filtros informados.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <returns>As permissões.</returns>
        public IEnumerable<Permissao> Pesquisar(int? idUsuario, int? idBandeira, int? idLoja)
        {
            Dictionary<int, Permissao> result = new Dictionary<int, Permissao>();

            Resource.Query<Permissao, PermissaoBandeira, PermissaoLoja, Permissao>(
              Sql.Permissao.Pesquisar,
              new { IdUsuario = idUsuario, IdBandeira = idBandeira, idLoja = idLoja },
              (permissao, bandeira, loja) =>
              {
                  if (!result.ContainsKey(permissao.Id))
                  {
                      result[permissao.Id] = permissao;
                  }

                  Permissao item = result[permissao.Id];

                  if (!item.Bandeiras.Any(b => b.IDBandeira == bandeira.IDBandeira))
                  {
                      item.Bandeiras.Add(bandeira);
                  }

                  if (!item.Lojas.Any(l => l.IDLoja == loja.IDLoja))
                  {
                      item.Lojas.Add(loja);
                  }

                  return item;
              },
              "IDPermissao,IDPermissaoBandeira,IDPermissaoLoja").Perform();

            return result.Values;
        }

        /// <summary>
        /// Pesquisa permissões utilizando os filtros informados retornando objetos Usuario, Bandeira e Loja preenchidos.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id de bandeira.</param>
        /// <param name="idLoja">O id de loja.</param>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <returns>As permissões.</returns>
        public IEnumerable<Permissao> PesquisarComFilhos(int? idUsuario, int? idBandeira, int? idLoja, Paging paging)
        {
            Dictionary<int, Permissao> result = new Dictionary<int, Permissao>();

            // Se vem loja, então passa nulo em bandeira, pois não é necessário buscar por ela.
            // Bug 4757:Não exibe dados
            if (idLoja.HasValue && idBandeira.HasValue)
            {
                idBandeira = (int?)null;
            }

            return Resource.Query<Permissao, Usuario, Bandeira, Loja, dynamic, Permissao>(
              Sql.Permissao.PesquisarComFilhos_Paging,
              new { idUsuario, idBandeira, idLoja },
              (permissao, usuario, bandeira, loja, bandeira2) =>
              {
                  if (!result.ContainsKey(permissao.Id))
                  {
                      permissao.Usuario = usuario;
                      result[permissao.Id] = permissao;
                  }

                  Permissao item = result[permissao.Id];

                  if (bandeira.IDBandeira != 0 && !item.Bandeiras.Any(b => b.Bandeira.IDBandeira == bandeira.IDBandeira))
                  {
                      item.Bandeiras.Add(new PermissaoBandeira
                      {
                          Bandeira = bandeira
                      });
                  }

                  if (loja.IDLoja != 0 && !item.Lojas.Any(l => l.Loja.IDLoja == loja.IDLoja))
                  {
                      if (bandeira2 != null)
                      {
                          loja.Bandeira = new Bandeira
                          {
                              DsBandeira = bandeira2.dsBandeiraLoja
                          };
                      }

                      item.Lojas.Add(new PermissaoLoja
                      {
                          Loja = loja
                      });
                  }

                  return item;
              },
              "SplitOn1,SplitOn2,SplitOn3,dsBandeiraLoja")
                  .AsPaging(paging, Sql.Permissao.PesquisarComFilhos_Paging, Sql.Permissao.PesquisarComFilhos_Count)
                  .AsMemoryResult(result.Values);
        }

        /// <summary>
        /// Obtém as permissões do usuário informado.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <returns>As permissões do usuário.</returns>
        public UsuarioPermissoes ObterPermissoesDoUsuario(int idUsuario)
        {
            var bandeiras = new List<Bandeira>();
            var lojas = new List<Loja>();

            Resource.Query<Bandeira, Loja, object>(
            Sql.Permissao.ObterPermissoesDoUsuario,
            new { idUsuario },
            (bandeira, loja) =>
            {
                if (bandeira.Id != 0 && !bandeiras.Contains(bandeira))
                {
                    bandeiras.Add(bandeira);
                }

                if (loja.Id != 0 && !lojas.Contains(loja))
                {
                    lojas.Add(loja);
                }

                return null;
            },
            "SplitOn1").Perform();

            return new UsuarioPermissoes(idUsuario, bandeiras, lojas);
        }

        /// <summary>
        /// Obtém a permissão pelo seu id.
        /// </summary>
        /// <param name="idPermissao">O id da permissão.</param>
        /// <returns>A permissão.</returns>
        public Permissao ObterPorId(int idPermissao)
        {
            Dictionary<int, Permissao> result = new Dictionary<int, Permissao>();

            Resource.Query<Permissao, Usuario, PermissaoBandeira, Bandeira, PermissaoLoja, Loja, dynamic, Permissao>(
              Sql.Permissao.ObterPorId,
              new { idPermissao },
              (permissao, usuario, pb, bandeira, pl, loja, bandeira2) =>
              {
                  if (!result.ContainsKey(permissao.Id))
                  {
                      permissao.Usuario = usuario;
                      result[permissao.Id] = permissao;
                  }

                  Permissao item = result[permissao.Id];

                  if (pb.IDPermissaoBandeira != 0 && !item.Bandeiras.Any(b => b.IDPermissaoBandeira == pb.IDPermissaoBandeira))
                  {
                      pb.Bandeira = bandeira;
                      pb.IDBandeira = bandeira.IDBandeira;
                      item.Bandeiras.Add(pb);
                  }

                  if (pl.IDPermissaoLoja != 0 && !item.Lojas.Any(l => l.IDPermissaoLoja == pl.IDPermissaoLoja))
                  {
                      if (bandeira2 != null)
                      {
                          loja.Bandeira = new Bandeira
                          {
                              DsBandeira = bandeira2.dsBandeiraLoja
                          };
                      }

                      pl.Loja = loja;
                      pl.IDLoja = loja.IDLoja;
                      item.Lojas.Add(pl);
                  }

                  return item;
              },
              "SplitOn1,SplitOn2,SplitOn3,SplitOn4,SplitOn5,dsBandeiraLoja").Perform();

            return result.Values.SingleOrDefault();
        }

        /// <summary>
        /// Verifica se o usuário possui permissão para uma determinada loja.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>True se o usuário possui permissão para a loja, através de permissão específica na loja (PermissaoLoja) ou através da bandeira da loja (PermissaoBandeira).</returns>
        public bool PossuiPermissaoLoja(int idUsuario, int idLoja)
        {
            return Resource.ExecuteScalar<bool?>(Sql.Permissao.PossuiPermissaoLoja, new { idUsuario, idLoja }) ?? false;
        }
        #endregion

        #region Helpers
        private IEnumerable<Permissao> FillChildren(IEnumerable<Permissao> permissao)
        {
            foreach (var r in permissao)
            {
                r.Bandeiras = m_permissaoBandeiraGateway.Find("IDPermissao = @IdPermissao", r).ToList();
                r.Lojas = m_permissaoLojaGateway.Find("IDPermissao = @IdPermissao", r).ToList();
            }

            return permissao;
        }
        #endregion

    }
}
