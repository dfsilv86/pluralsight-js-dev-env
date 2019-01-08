using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Representa uma GradeSugestao.
    /// </summary>
    public class GradeSugestao : EntityBase, IAggregateRoot
    {
        /// <summary>
        /// Obtém ou define o id.
        /// </summary>
        public override int Id
        {
            get { return (int)IDGradeSugestao; }
            set { IDGradeSugestao = value; }
        }

        /// <summary>
        /// Obtém ou define IDGradeSugestao.
        /// </summary>
        public long IDGradeSugestao { get; set; }

        /// <summary>
        /// Obtém ou define IDBandeira.
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int? IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public int cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define vlHoraInicial.
        /// </summary>
        public int vlHoraInicial { get; set; }

        /// <summary>
        /// Obtém ou define vlHoraFinal.
        /// </summary>
        public int vlHoraFinal { get; set; }

        /// <summary>
        /// Obtém ou define o sistema.
        /// </summary>
        public Sistema Sistema { get; set; }

        /// <summary>
        /// Obtém ou define a bandeira.
        /// </summary>
        public Bandeira Bandeira { get; set; }

        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define a data/hora de criação.
        /// </summary>
        public DateTime? DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define a data/hora de atualização.
        /// </summary>
        public DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define o id do usuário de cricação.
        /// </summary>
        public int? CdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define o usuário de criação.
        /// </summary>
        public Usuario UsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define o id do usuário de atualização.
        /// </summary>
        public int? CdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define o usuário de atualização.
        /// </summary>
        public Usuario UsuarioAtualizacao { get; set; }
    }
}
