using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Serviço de domínio relacionado a distrito.
    /// </summary>
    public class DistritoService : EntityDomainServiceBase<Distrito, IDistritoGateway>, IDistritoService
    {
        #region Constructor        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DistritoService"/>
        /// </summary>
        /// <param name="mainGateway">O table data gateway principal.</param>
        public DistritoService(IDistritoGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods        

        /// <summary>
        /// Salvar um Distrito.
        /// </summary>
        /// <param name="entidade">O distrito a ser salvo.</param>
        public override void Salvar(Distrito entidade)
        {
            Assert(new { DistrictUserResponsible = entidade.cdUsuarioResponsavelDistrito }, new AllMustBeInformedSpec());

            var usuario = RuntimeContext.Current.User.Id;
            var dt = DateTime.Now;

            if (entidade.IsNew)
            {
                entidade.dhCriacao = dt;
                entidade.cdUsuarioCriacao = usuario;

                MainGateway.Insert(entidade);
            }
            else
            {
                entidade.dhAtualizacao = dt;
                entidade.cdUsuarioAtualizacao = usuario;

                MainGateway.Update(
                    @"dhAtualizacao = @dhAtualizacao, 
                     cdUsuarioAtualizacao = @cdUsuarioAtualizacao, 
                     cdUsuarioResponsavelDistrito = @cdUsuarioResponsavelDistrito",
                      entidade);
            }
        }

        /// <summary>
        /// Obtem um Distrito por Id.
        /// </summary>
        /// <param name="id">O ID do Distrito.</param>
        /// <returns>Retorna um Distrito.</returns>
        public Distrito ObterEstruturado(int id)
        {
            return this.MainGateway.ObterEstruturado(id);
        }

        /// <summary>
        /// Obtém os distritos associados a uma região.
        /// </summary>
        /// <param name="idRegiao">O id de região.</param>
        /// <returns>
        /// A lista de distritos.
        /// </returns>
        public IEnumerable<Distrito> ObterPorRegiao(int idRegiao)
        {
            return this.MainGateway.Find("IDRegiao=@IDRegiao", new { idRegiao });
        }

        /// <summary>
        /// Pesquisar Distritos
        /// </summary>
        /// <param name="cdSistema">Código do sistema.</param>
        /// <param name="idBandeira">ID da Bandeira.</param>
        /// <param name="idRegiao">ID da Região.</param>
        /// <param name="idDistrito">ID do Distrito.</param>
        /// <param name="paging">Parametro de paginação.</param>
        /// <returns>Retorna uma lista de Distritos como resultado da busca.</returns>
        public IEnumerable<Distrito> Pesquisar(int? cdSistema, int? idBandeira, int? idRegiao, int? idDistrito, Paging paging)
        {
            return this.MainGateway.Pesquisar(cdSistema, idBandeira, idRegiao, idDistrito, paging);
        }
        #endregion
    }
}
