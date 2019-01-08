using System;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Serviço de domínio relacionado a parâmetro.
    /// </summary>
    public class ParametroService : IParametroService
    {
        #region Fields
        private readonly IParametroGateway m_parametroGateway;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ParametroService"/>.
        /// </summary>
        /// <param name="parametroGateway">O table data gateway para parâmetro.</param>
        public ParametroService(IParametroGateway parametroGateway)
        {
            m_parametroGateway = parametroGateway;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtém o parãmetro.
        /// </summary>
        /// <returns>
        /// O parâmetro.
        /// </returns>
        public Parametro Obter()
        {
            // TODO: cachear aqui OU injetar a instância de parâmetro por request (consequentemente por transação) no LightInject, e modificar a instância injetada no Salvar()
            return m_parametroGateway.FindAll().SingleOrDefault();
        }

        /// <summary>
        /// Salva o parãmetro
        /// </summary>
        /// <param name="parametro">O parâmetro.</param>
        public void Salvar(Parametro parametro)
        {
            SpecService.Assert(
            new
            {
                parametro.cdUsuarioAdministrador,
                parametro.pcDivergenciaCustoCompra,
                parametro.PercentualAuditoria,
                parametro.dsServidorSmartEndereco,
                parametro.dsServidorSmartDiretorio,
                parametro.TpArquivoInventario,
                parametro.qtdDiasArquivoInventarioVarejo,
                parametro.qtdDiasArquivoInventarioAtacado
            },
            new AllMustBeInformedSpec(true));

            var existing = m_parametroGateway.FindAll().SingleOrDefault();

            // Se ainda não existe, então cria o primeiro e único registro.
            if (existing == null)
            {
                m_parametroGateway.Insert(parametro);
            }
            else
            {
                // Atualiza o registro existente.
                parametro.Id = existing.Id;
                parametro.dhAlteracao = DateTime.Now;
                parametro.cdUsuarioAlteracao = RuntimeContext.Current.User.Id;
                m_parametroGateway.Update(parametro);
            }
        }

        /// <summary>
        /// Obtém o parâmetro com seus relacionamentos.
        /// </summary>
        /// <returns>
        /// O parâmetro.
        /// </returns>
        public Parametro ObterEstruturado()
        {
            return m_parametroGateway.ObterEstruturado();
        }
        #endregion
    }
}
