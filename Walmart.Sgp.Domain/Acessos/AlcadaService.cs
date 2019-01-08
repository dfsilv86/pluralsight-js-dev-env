using System;
using System.Linq;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Serviço de domínio relacionado a alçada.
    /// </summary>
    public class AlcadaService : EntityDomainServiceBase<Alcada, IAlcadaGateway>, IAlcadaService
    {
        #region Fields

        private static readonly string[] AlcadaAuditProperties = new string[] 
        { 
            "IDAlcada", 
            "IDPerfil",
            "blAlterarSugestao", 
            "blAlterarInformacaoEstoque", 
            "blAlterarPercentual", 
            "vlPercentualAlterado", 
            "blZerarItem"
        };

        private static readonly string[] AlcadaDetalheAuditProperties = new string[] 
        { 
            "vlPercentualAlterado", 
        };

        private readonly IAuditService m_auditService;

        #endregion

        #region Constructor
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AlcadaService"/>.
        /// </summary>
        /// <param name="mainGateway">O table data gateway para main data.</param>
        /// <param name="auditService">O serviço de log.</param>
        public AlcadaService(IAlcadaGateway mainGateway, IAuditService auditService)
            : base(mainGateway)
        {
            m_auditService = auditService;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Localiza uma alçada pelo seu id de perfil.
        /// </summary>
        /// <param name="idPerfil">O id de perfil.</param>
        /// <returns>
        /// A alcada existente ou um registro de alcada novo com valores default e ainda não persistido em banco.
        /// </returns>
        public Alcada ObterPorPerfil(int idPerfil)
        {
            Assert(new { idPerfil }, new AllMustBeInformedSpec());

            // Geradas\AlcadaData.cs linha 312
            // TODO: Talvez este aqui também devesse trazer a alçada com detalhes pronto (estruturado)
            return MainGateway.Find("IDPerfil=@idPerfil", new { idPerfil }).SingleOrDefault() ?? new Alcada() { IDPerfil = idPerfil };
        }

        /// <summary>
        /// Obtém a alçada juntamente com o papel baseado no id do papel.
        /// </summary>
        /// <param name="idPerfil">O identificador do perfil.</param>
        /// <returns>A alçada que percence ao perfil.</returns>
        public Alcada ObterEstruturadoPorPerfil(int idPerfil)
        {
            return MainGateway.ObterEstruturadoPorPerfil(idPerfil);
        }

        /// <summary>
        /// Remover uma entidade pelo Id.
        /// </summary>
        /// <param name="id">O Id da entidade.</param>
        public override void Remover(int id)
        {
            var alcada = this.MainGateway.ObterEstruturado(id, null);

            this.MainGateway.Delete(id, new AlcadaAuditStrategy(alcada, m_auditService, AlcadaAuditProperties, AlcadaDetalheAuditProperties));
        }

        /// <summary>
        /// Salva a alçada.
        /// </summary>
        /// <param name="entidade">A alçada.</param>
        public override void Salvar(Alcada entidade)
        {
            entidade.GarantirIntegridade();
            Assert(entidade, new AlcadaPodeSerSalvaSpec());
            Assert(entidade, new AlcadaNaoPodeTerDetalheRepetidoSpec(MainGateway.ObterEstruturado));

            if (entidade.IsNew)
            {
                MainGateway.Insert(entidade, new AlcadaAuditStrategy(null, m_auditService, AlcadaAuditProperties, AlcadaDetalheAuditProperties));
            }
            else
            {
                var alcada = this.MainGateway.ObterEstruturado(entidade.Id, null);

                MainGateway.Update(entidade, new AlcadaAuditStrategy(alcada, m_auditService, AlcadaAuditProperties, AlcadaDetalheAuditProperties));
            }
        }

        /// <summary>
        /// Verifica se não há tentativa de inserir registros filhos duplicados para Alçada.
        /// </summary>
        /// <param name="entidade">A entidade pai.</param>
        /// <returns>O SpecResult com o resultado a validação.</returns>
        public SpecResult ValidarDuplicidadeDetalhe(Alcada entidade)
        {
            return new AlcadaNaoPodeTerDetalheRepetidoSpec(MainGateway.ObterEstruturado).IsSatisfiedBy(entidade);
        }

        #endregion
    }
}
