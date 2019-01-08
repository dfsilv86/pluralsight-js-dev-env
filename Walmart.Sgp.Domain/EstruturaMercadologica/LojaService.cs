using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Serviço de domínio relacionado a loja.
    /// </summary>
    public class LojaService : EntityDomainServiceBase<Loja, ILojaGateway>, ILojaService
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LojaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para loja.</param>
        public LojaService(ILojaGateway mainGateway)
            : base(mainGateway)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obter por usuário e sistema.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <returns>As lojas.</returns>
        /// <remarks>Não valida o tipo de permissão do usuário.</remarks>
        public Loja ObterPorLojaUsuarioEBandeira(int idUsuario, short? idBandeira, int cdLoja)
        {
            return this.MainGateway.ObterPorLojaUsuarioEBandeira(idUsuario, idBandeira, cdLoja);
        }

        /// <summary>
        /// Conta o número de lojas por usuário.
        /// </summary>
        /// <param name="idUsuario">O id de usuário.</param>
        /// <returns>
        /// O número de lojas.
        /// </returns>
        public long ContarPorUsuario(int idUsuario)
        {
            return MainGateway.Count("IdUsuario = @idUsuario", new { idUsuario });
        }

        /// <summary>
        /// Pesquisa lojas com os parâmetros especificados.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="cdSistema">O código de estrutura mercadológica.</param>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <param name="cdLoja">O código da loja.</param>
        /// <param name="nmLoja">O nome da loja.</param>
        /// <returns>Lista de lojas que atendem aos parâmetros especificados.</returns>
        /// <param name="paging">A paginação que será utilizada para retornar o resultado.</param>
        /// <remarks>Valida o tipo de permissão do usuário.</remarks>
        public IEnumerable<Loja> Pesquisar(int idUsuario, int cdSistema, int? idBandeira, int? cdLoja, string nmLoja, Paging paging)
        {
            Assert(new { MarketingStructure = cdSistema, User = idUsuario }, new AllMustBeInformedSpec());

            return MainGateway.Pesquisar(idUsuario, RuntimeContext.Current.User.TipoPermissao, cdSistema, idBandeira, cdLoja, nmLoja, paging);
        }

        /// <summary>
        /// Obtém as lojas pelo id da bandeira.
        /// </summary>
        /// <param name="idBandeira">O id da bandeira.</param>
        /// <returns>As lojas.</returns>
        public IEnumerable<Loja> ObterLojasPorBandeira(int idBandeira)
        {
            return MainGateway.Find("IDBandeira = @idBandeira AND BlCarregaSGP = 1", new { idBandeira });
        }

        /// <summary>
        /// Obtém a entidade pelo cdLoja.
        /// </summary>
        /// <param name="cdSistema">O código do sistema.</param>
        /// <param name="cdLoja">O cdLoja da entidade desejada.</param>
        /// <returns>A instância da entidade.</returns>
        public Loja ObterPorCdLoja(int cdSistema, int cdLoja)
        {
            return MainGateway.Find("cdSistema = @cdSistema AND cdLoja = @cdLoja", new { cdSistema, cdLoja }).FirstOrDefault();
        }

        /// <summary>
        /// Pesquisa lojas por item destino e origem
        /// </summary>
        /// <param name="filtro">Os filtros</param>
        /// <param name="paging">A paginacao</param>
        /// <returns>As lojas</returns>
        public IEnumerable<Loja> PesquisarLojasPorItemDestinoItemOrigem(LojaFiltro filtro, Paging paging)
        {
            return this.MainGateway.PesquisarLojasPorItemDestinoItemOrigem(filtro, paging);
        }

        /// <summary>
        /// Obtem as informações da loja e entidades associadas.
        /// </summary>
        /// <param name="idLoja">O id da loja.</param>
        /// <returns>A loja.</returns>
        public Loja ObterEstruturadoPorId(int idLoja)
        {
            return this.MainGateway.ObterEstruturadoPorId(idLoja);
        }

        /// <summary>
        /// Altera informações de uma loja.
        /// </summary>
        /// <param name="loja">As novas informações da loja.</param>
        /// <returns>A loja modificada.</returns>
        public Loja AlterarLoja(Loja loja)
        {
            Assert(new { loja, loja.cdSistema, loja.IDBandeira, loja.IDDistrito, loja.TipoArquivoInventario }, new AllMustBeInformedSpec());
            Assert(loja, new SupercenterPermiteApenasArquivosFinaisSpec());

            this.MainGateway.Update("IDBandeira=@IDBandeira,IDDistrito=@IDDistrito,dsServidorSmartEndereco=@dsServidorSmartEndereco,dsServidorSmartDiretorio=@dsServidorSmartDiretorio,dsServidorSmartNomeUsuario=@dsServidorSmartNomeUsuario,dsServidorSmartSenha=@dsServidorSmartSenha,TipoArquivoInventario=@TipoArquivoInventario,blCalculaSugestao=@blCalculaSugestao,blEmitePedido=@blEmitePedido,cdUsuarioResponsavelLoja=@cdUsuarioResponsavelLoja", loja);

            return ObterEstruturadoPorId(loja.IDLoja);
        }

        #endregion

    }
}
