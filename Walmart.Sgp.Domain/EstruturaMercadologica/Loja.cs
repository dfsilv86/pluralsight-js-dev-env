using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Representa uma Loja.
    /// </summary>
    [DebuggerDisplay("{cdLoja} - {nmLoja}")]
    public class Loja : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDLoja;
            }

            set
            {
                IDLoja = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public byte cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define IDBandeira.
        /// </summary>
        public int? IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define cdLoja.
        /// </summary>
        public int cdLoja { get; set; }

        /// <summary>
        /// Obtém ou define nmLoja.
        /// </summary>
        public string nmLoja { get; set; }

        /// <summary>
        /// Obtém ou define dsEndereco.
        /// </summary>
        public string dsEndereco { get; set; }

        /// <summary>
        /// Obtém ou define dsCidade.
        /// </summary>
        public string dsCidade { get; set; }

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
        /// Obtém ou define blAtivo.
        /// </summary>
        public bool? blAtivo { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime? dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAtualizacao.
        /// </summary>
        public DateTime? dhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCriacao.
        /// </summary>
        public int? cdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAtualizacao.
        /// </summary>
        public int? cdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define nmDatabase.
        /// </summary>
        public string nmDatabase { get; set; }

        /// <summary>
        /// Obtém ou define IDDistrito.
        /// </summary>
        public int? IDDistrito { get; set; }

        /// <summary>
        /// Obtém ou define blEnvioBI.
        /// </summary>
        public bool? blEnvioBI { get; set; }

        /// <summary>
        /// Obtém ou define blCarregaSGP.
        /// </summary>
        public int? blCarregaSGP { get; set; }

        /// <summary>
        /// Obtém ou define blContabilizar.
        /// </summary>
        public int? blContabilizar { get; set; }

        /// <summary>
        /// Obtém ou define blCorrecaoPLU.
        /// </summary>
        public bool? blCorrecaoPLU { get; set; }

        /// <summary>
        /// Obtém ou define dsEstado.
        /// </summary>
        public string dsEstado { get; set; }

        /// <summary>
        /// Obtém ou define DataConversao.
        /// </summary>
        public DateTime? DataConversao { get; set; }

        /// <summary>
        /// Obtém ou define TipoCusto.
        /// </summary>
        public int? TipoCusto { get; set; }

        /// <summary>
        /// Obtém ou define TipoArquivoInventario.
        /// </summary>
        public TipoArquivoInventario TipoArquivoInventario { get; set; }

        /// <summary>
        /// Obtém ou define DataEnvioBI.
        /// </summary>
        public DateTime? DataEnvioBI { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioResponsavelLoja.
        /// </summary>
        public int? cdUsuarioResponsavelLoja { get; set; }

        /// <summary>
        /// Obtém ou define blCalculaSugestao.
        /// </summary>
        public bool blCalculaSugestao { get; set; }

        /// <summary>
        /// Obtém ou define blEmitePedido.
        /// </summary>
        public bool blEmitePedido { get; set; }

        /// <summary>
        /// Obtém ou define blAutorizaPedido.
        /// </summary>
        public bool? blAutorizaPedido { get; set; }

        /// <summary>
        /// Obtém ou define a bandeira.
        /// </summary>
        public Bandeira Bandeira { get; set; }

        /// <summary>
        /// Obtém ou define o distrito.
        /// </summary>
        public Distrito Distrito { get; set; }

        /// <summary>
        /// Obtém ou define o IdRegiaoAdministrativa.
        /// </summary>
        public int? IdRegiaoAdministrativa { get; set; }
    }
}
