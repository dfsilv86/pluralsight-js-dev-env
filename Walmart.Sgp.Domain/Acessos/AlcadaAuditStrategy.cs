using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Implementa estratégia de auditoria para alçada.
    /// </summary>
    public class AlcadaAuditStrategy : IAuditStrategy
    {
        #region Fields
        private readonly Alcada m_alcada;
        private readonly IAuditService m_auditService;
        private readonly string[] m_alcadaProperties;
        private readonly string[] m_alcadaDetalheProperties;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AlcadaAuditStrategy"/>.
        /// </summary>
        /// <param name="original">A alçada original persistida em banco.</param>
        /// <param name="auditService">O serviço de auditoria.</param>
        /// <param name="alcadaProperties">As propriedades da Alcada que devem ser auditadas.</param>
        /// <param name="alcadaDetalheProperties">As propriedades da AlcadaDetalhe que devem ser auditadas.</param>
        public AlcadaAuditStrategy(Alcada original, IAuditService auditService, string[] alcadaProperties, string[] alcadaDetalheProperties)
        {
            m_alcada = original;
            m_auditService = auditService;
            m_alcadaProperties = alcadaProperties;
            m_alcadaDetalheProperties = alcadaDetalheProperties;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Usou o gateway para inserir as instâncias especificadas.
        /// </summary>
        /// <param name="instances">As instâncias.</param>
        public void DidInsert(params object[] instances)
        {
            foreach (object instance in instances)
            {
                if (instance is Alcada)
                {
                    LogInsert((Alcada)instance);
                }
                else if (instance is AlcadaDetalhe)
                {
                    LogInsert((AlcadaDetalhe)instance);
                }
            }
        }

        /// <summary>
        /// Usou o gateway para alterar as instâncias especificadas.
        /// </summary>
        /// <param name="instances">As instâncias.</param>
        /// <remarks>
        /// Isso não indica que a instância teve algum valor realmente modificado, apenas que o gateway foi usado para executar um UPDATE no banco.
        /// </remarks>
        public void DidUpdate(params object[] instances)
        {
            foreach (object instance in instances)
            {
                if (instance is Alcada)
                {
                    LogUpdate((Alcada)instance);
                }
                else if (instance is AlcadaDetalhe)
                {
                    LogUpdate((AlcadaDetalhe)instance);
                }
            }
        }

        /// <summary>
        /// Irá usar o gateway para excluir as instâncias especificadas.
        /// </summary>
        /// <param name="instances">As instâncias.</param>
        public void WillDelete(params object[] instances)
        {
            foreach (object instance in instances)
            {
                if (instance is Alcada)
                {
                    LogDelete((Alcada)instance);
                }
                else if (instance is AlcadaDetalhe)
                {
                    LogDelete((AlcadaDetalhe)instance);
                }
            }
        }

        private void LogInsert(Alcada alcada)
        {
            m_auditService.LogInsert(alcada, m_alcadaProperties);
        }

        private void LogInsert(AlcadaDetalhe alcadaDetalhe)
        {
            m_auditService.LogInsert(alcadaDetalhe, m_alcadaDetalheProperties);
        }

        private void LogUpdate(Alcada alcada)
        {
            m_auditService.LogUpdate(alcada, m_alcadaProperties);
        }

        private void LogUpdate(AlcadaDetalhe alcadaDetalhe)
        {
            var detalheOriginal = m_alcada.Detalhe.Single(d => d.Id == alcadaDetalhe.Id);

            if (detalheOriginal.vlPercentualAlterado != alcadaDetalhe.vlPercentualAlterado)
            {
                m_auditService.LogUpdate(alcadaDetalhe, m_alcadaDetalheProperties);
            }
        }

        private void LogDelete(Alcada alcada)
        {
            m_auditService.LogDelete(alcada, m_alcadaProperties);
        }

        private void LogDelete(AlcadaDetalhe alcadaDetalhe)
        {
            m_auditService.LogDelete(alcadaDetalhe, m_alcadaDetalheProperties);
        }

        #endregion
    }
}
