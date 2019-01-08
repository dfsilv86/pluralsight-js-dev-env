using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Walmart.Sgp.Infrastructure.Data.Databases;

namespace Walmart.Sgp.WebApi.Controllers
{
    /// <summary>
    /// Classe base para as controlles da webapi.
    /// </summary>
    /// <typeparam name="TService">O tipo do serviço principal utilizado pela controller.</typeparam>
    public class ApiControllerBase<TService> : ApiController
    {
        #region Fields
        private bool disposedValue = false; // To detect redundant calls
        #endregion

        #region Constructors
        public ApiControllerBase(TService service)
        {
            MainService = service;
        }

        /// <summary>
        /// Finaliza uma instância da classe <see cref="ApiControllerBase{TService}"/>.
        /// </summary>
        ~ApiControllerBase()
        {
            Dispose(false);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define as conexões com os banco de dados para o web request corrente.
        /// </summary>
        protected ApplicationDatabases ApplicationDatabases { get; set; }

        /// <summary>
        /// Obtém o serviço principal.
        /// </summary>
        protected TService MainService { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Realiza o commit no banco principal (Wlmslp).
        /// </summary>
        protected virtual void Commit()
        {
            ApplicationDatabases.Wlmslp.Transaction.Commit();
        }
        #endregion

        #region IDisposable Support
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposedValue)
            {
                if (disposing)
                {
                    this.ApplicationDatabases.Dispose();
                }

                disposedValue = true;
            }
        }
        #endregion
    }
}