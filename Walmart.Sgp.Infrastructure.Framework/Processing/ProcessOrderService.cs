using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Informações sobre um serviço.
    /// </summary>
    public class ProcessOrderService : EntityBase
    {
        #region Propriedades
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public int ProcessOrderServiceId { get; set; }

        /// <summary>
        /// Obtém ou define o id da ordem de execução a qual estas informações estão associadas.
        /// </summary>
        public int ProcessOrderId { get; set; }

        /// <summary>
        /// Obtém ou define o nome completo do tipo que implementa o serviço, que corresponde ao FullName e o nome do assembly do serviço.
        /// </summary>
        public string ServiceTypeName { get; set; }

        /// <summary>
        /// Obtém ou define o nome do método do serviço;
        /// </summary>
        public string ServiceMethodName { get; set; }

        /// <summary>
        /// Obtém ou define o tipo do resultado.
        /// </summary>
        public string ResultTypeFullName { get; set; }

        /// <summary>
        /// Obtém ou define o id do ticket do arquivo com resultado.
        /// </summary>
        public string ResultFilePath { get; set; }

        /// <summary>
        /// Obtém ou define o número máximo de execuções paralelas.
        /// </summary>
        public int? MaxGlobal { get; set; }

        /// <summary>
        /// Obtém ou define o número máximo de execuções paralelas por usuário.
        /// </summary>
        public int? MaxPerUser { get; set; }

        /// <summary>
        /// Obtém ou define o id do perfil utilizado pelo usuário no momento da criação da ordem de execução.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Obtém ou define o id da loja utilizada pelo usuário no momento da criação da ordem de execução.
        /// </summary>
        public int? StoreId { get; set; }

        /// <summary>
        /// Obtém ou define o id da bandeira utilizada pelo usuário no momento da criação da ordem de execução.
        /// </summary>
        public int? BandeiraId { get; set; }

        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return this.ProcessOrderServiceId;
            }

            set
            {
                this.ProcessOrderServiceId = value;
            }
        }
        #endregion
    }
}
