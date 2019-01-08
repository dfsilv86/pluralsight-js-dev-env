using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Gerenciamento
{
    /// <summary>
    /// Representa uma parametro.
    /// </summary>
    public class Parametro : EntityBase, IAggregateRoot
    {
        #region Properties 
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDParametro;
            }

            set
            {
                IDParametro = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDParametro.
        /// </summary>
        public int IDParametro { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAdministrador.
        /// </summary>
        public int cdUsuarioAdministrador { get; set; }

        /// <summary>
        /// Obtém ou define dsServidorSmartEndereco.
        /// </summary>
        public string dsServidorSmartEndereco { get; set; }

        /// <summary>
        /// Obtém ou define dsServidorSmartDiretorio.
        /// </summary>
        public string dsServidorSmartDiretorio { get; set; }

        /// <summary>
        /// Obtém ou define dsServidorSmartNomeUsuario.
        /// </summary>
        public string dsServidorSmartNomeUsuario { get; set; }

        /// <summary>
        /// Obtém ou define dsServidorSmartSenha.
        /// </summary>
        public string dsServidorSmartSenha { get; set; }

        /// <summary>
        /// Obtém ou define dhAlteracao.
        /// </summary>
        public DateTime dhAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAlteracao.
        /// </summary>
        public int cdUsuarioAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define pcDivergenciaCustoCompra.
        /// </summary>
        public decimal pcDivergenciaCustoCompra { get; set; }

        /// <summary>
        /// Obtém ou define qtdDiasSugestaoInventario.
        /// </summary>
        public int? qtdDiasSugestaoInventario { get; set; }

        /// <summary>
        /// Obtém ou define PercentualAuditoria.
        /// </summary>
        public int? PercentualAuditoria { get; set; }

        /// <summary>
        /// Obtém ou define qtdDiasArquivoInventarioVarejo.
        /// </summary>
        public int? qtdDiasArquivoInventarioVarejo { get; set; }

        /// <summary>
        /// Obtém ou define qtdDiasArquivoInventarioAtacado.
        /// </summary>
        public int? qtdDiasArquivoInventarioAtacado { get; set; }

        /// <summary>
        /// Obtém ou define TpArquivoInventario.
        /// </summary>
        public TipoFormatoArquivoInventario TpArquivoInventario { get; set; }

        /// <summary>
        /// Obtém ou define UsuarioAdministrador.
        /// </summary>
        public Usuario UsuarioAdministrador { get; set; }

        /// <summary>
        /// Obtém ou define UsuarioAlteracao.
        /// </summary>
        public Usuario UsuarioAlteracao { get; set; }
        #endregion
    }
}
