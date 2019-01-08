using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Acessos
{
    /// <summary>
    /// Representa uma permissao.
    /// </summary>
    public class Permissao : EntityBase, IAggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Permissao"/>
        /// </summary>
        public Permissao()
        {
            dhCriacao = dhAlteracao = DateTime.Now;
            Bandeiras = new List<PermissaoBandeira>();
            Lojas = new List<PermissaoLoja>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get
            {
                return IDPermissao;
            }

            set
            {
                IDPermissao = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDPermissao.
        /// </summary>
        public int IDPermissao { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuario.
        /// </summary>
        public int IDUsuario { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuarioCriacao.
        /// </summary>
        public int IDUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define dhCriacao.
        /// </summary>
        public DateTime dhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define IDUsuarioAlteracao.
        /// </summary>
        public int IDUsuarioAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define dhAlteracao.
        /// </summary>
        public DateTime dhAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define blAdministrador.
        /// </summary>
        public bool blAdministrador { get; set; }

        /// <summary>
        /// Obtém ou define blRecebeNotificaoOperacoes.
        /// </summary>
        public bool blRecebeNotificaoOperacoes { get; set; }

        /// <summary>
        /// Obtém ou define blRecebeNotificaoFinanceiro.
        /// </summary>
        public bool blRecebeNotificaoFinanceiro { get; set; }

        /// <summary>
        /// Obtém ou define permissões de bandeiras.
        /// </summary>
        public IList<PermissaoBandeira> Bandeiras { get; set; }

        /// <summary>
        /// Obtém ou define as permissões de lojas.
        /// </summary>
        public IList<PermissaoLoja> Lojas { get; set; }
        
        /// <summary>
        /// Obtém ou define o usuário.
        /// </summary>
        public Usuario Usuario { get; set; }
        #endregion       
    }
}
