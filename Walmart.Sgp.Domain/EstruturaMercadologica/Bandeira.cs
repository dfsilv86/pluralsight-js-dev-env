using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.EstruturaMercadologica
{
    /// <summary>
    /// Cadastro de Bandeira.
    /// </summary>
    public class Bandeira : EntityBase, IAggregateRoot, IStampContainer
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Bandeira"/>
        /// </summary>
        public Bandeira()
        {
            TpCusto = "U";
            Detalhes = new List<BandeiraDetalhe>();
            Regioes = new List<Regiao>();
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
                return IDBandeira;
            }

            set
            {
                IDBandeira = value;
            }
        }

        /// <summary>
        /// Obtém ou define o id da bandeira.
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define a descrição.
        /// </summary>
        public string DsBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o código de estrutura mercadológica.
        /// </summary>
        public int? CdSistema { get; set; }

        /// <summary>
        /// Obtém ou define a sigla.
        /// </summary>
        public string SgBandeira { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de custo.
        /// </summary>
        public string TpCusto { get; set; }

        /// <summary>
        /// Obtém ou define se a bandeira está ativa.
        /// </summary>
        public BandeiraStatus BlAtivo { get; set; }

        /// <summary>
        /// Obtém ou define a data de criação.
        /// </summary>
        public DateTime DhCriacao { get; set; }

        /// <summary>
        /// Obtém ou define a data de atualização.
        /// </summary>
        public DateTime? DhAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define o código do usuário criador.
        /// </summary>
        public int? CdUsuarioCriacao { get; set; }

        /// <summary>
        /// Obtém ou define o código do usuário que atualizou o registro.
        /// </summary>
        public int? CdUsuarioAtualizacao { get; set; }

        /// <summary>
        /// Obtém ou define se deve importar todos.
        /// </summary>
        public bool BlImportarTodos { get; set; }

        /// <summary>
        /// Obtém ou define o formato.
        /// </summary>
        public int? IDFormato { get; set; }

        /// <summary>
        /// Obtém ou define o formato.
        /// </summary>
        public Formato Formato { get; set; }

        /// <summary>
        /// Obtém ou define os detalhes.
        /// </summary>
        public IEnumerable<BandeiraDetalhe> Detalhes { get; set; }

        /// <summary>
        /// Obtém ou define as regiões.
        /// </summary>
        public IEnumerable<Regiao> Regioes { get; set; }
        #endregion
    }
}
