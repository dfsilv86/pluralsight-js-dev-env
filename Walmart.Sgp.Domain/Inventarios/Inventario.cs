using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.Inventarios
{
    /// <summary>
    /// Representa uma inventario.
    /// </summary>
    public class Inventario : EntityBase, IAggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Inventario" />.
        /// </summary>        
        public Inventario()
        {
            tpImportacao = TipoImportacao.Automatico;
            tpInventario = String.Empty;
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
                return IDInventario;
            }

            set
            {
                IDInventario = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDInventario.
        /// </summary>
        public int IDInventario { get; set; }

        /// <summary>
        /// Obtém ou define IDLoja.
        /// </summary>
        public int IDLoja { get; set; }

        /// <summary>
        /// Obtém ou define a loja.
        /// </summary>
        public Loja Loja { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém ou define IDCategoria.
        /// </summary>
        public int? IDCategoria { get; set; }

        /// <summary>
        /// Obtém ou define a categoria.
        /// </summary>
        public Categoria Categoria { get; set; }

        /// <summary>
        /// Obtém ou define IDBandeira.
        /// </summary>
        public int IDBandeira { get; set; }

        /// <summary>
        /// Obtém ou define a bandeira.
        /// </summary>
        public Bandeira Bandeira { get; set; }

        /// <summary>
        /// Obtém ou define dhInventario.
        /// </summary>
        public DateTime dhInventario { get; set; }

        /// <summary>
        /// Obtém ou define tpImportacao.
        /// </summary>
        public TipoImportacao tpImportacao { get; set; }

        /// <summary>
        /// Obtém ou define dhImportacao.
        /// </summary>
        public DateTime? dhImportacao { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioImportacao.
        /// </summary>
        public int? cdUsuarioImportacao { get; set; }

        /// <summary>
        /// Obtém ou define dhAprovacaoLoj.
        /// </summary>
        public DateTime? dhAprovacaoLoj { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAprovacaoLoj.
        /// </summary>
        public int? cdUsuarioAprovacaoLoj { get; set; }

        /// <summary>
        /// Obtém ou define dhAprovacaoOpe.
        /// </summary>
        public DateTime? dhAprovacaoOpe { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAprovacaoOpe.
        /// </summary>
        public int? cdUsuarioAprovacaoOpe { get; set; }

        /// <summary>
        /// Obtém ou define dhCancelamentoLoj.
        /// </summary>
        public DateTime? dhCancelamentoLoj { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCancelamentoLoj.
        /// </summary>
        public int? cdUsuarioCancelamentoLoj { get; set; }

        /// <summary>
        /// Obtém ou define dhCancelamentoOpe.
        /// </summary>
        public DateTime? dhCancelamentoOpe { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioCancelamentoOpe.
        /// </summary>
        public int? cdUsuarioCancelamentoOpe { get; set; }

        /// <summary>
        /// Obtém ou define dhContabilizacao.
        /// </summary>
        public DateTime? dhContabilizacao { get; set; }

        /// <summary>
        /// Obtém ou define stInventario.
        /// </summary>
        public InventarioStatus stInventario { get; set; }

        /// <summary>
        /// Obtém ou define tpInventario.
        /// </summary>
        public string tpInventario { get; set; }

        /// <summary>
        /// Obtém ou define dhFinalizacaoLoj.
        /// </summary>
        public DateTime? dhFinalizacaoLoj { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioFinalizacaoLoj.
        /// </summary>
        public int? cdUsuarioFinalizacaoLoj { get; set; }

        /// <summary>
        /// Obtém ou define dhAberturaLoj.
        /// </summary>
        public DateTime? dhAberturaLoj { get; set; }

        /// <summary>
        /// Obtém ou define cdUsuarioAberturaLoj.
        /// </summary>
        public int? cdUsuarioAberturaLoj { get; set; }

        /// <summary>
        /// Obtém ou define dhInventarioArquivo.
        /// </summary>
        public DateTime? dhInventarioArquivo { get; set; }

        /// <summary>
        /// Obtém ou define o usuário de abertura da loja.
        /// </summary>
        public Usuario UsuarioAberturaLoja { get; set; }

        /// <summary>
        /// Obtém ou define o usuário da importação.
        /// </summary>
        public Usuario UsuarioImportacao { get; set; }

        /// <summary>
        /// Obtém ou define o usuário de finalização da loja.
        /// </summary>
        public Usuario UsuarioFinalizacaoLoja { get; set; }

        /// <summary>
        /// Obtém ou define o usuário de aprovação da loja.
        /// </summary>
        public Usuario UsuarioAprovacaoLoja { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Marca o inventário como aberto.
        /// </summary>
        /// <param name="idUsuario">O id do usuário.</param>
        /// <param name="dhAberturaLoja">A data de abertura.</param>
        /// <param name="categoriaService">O serviço de categoria.</param>
        public void MarcarComoAberto(int idUsuario, DateTime dhAberturaLoja, ICategoriaService categoriaService)
        {
            IDCategoria = categoriaService.ObterIDCategoria(Departamento.cdSistema, Departamento.cdCategoria);
            IDBandeira = Loja.IDBandeira.Value;
            cdUsuarioImportacao = idUsuario;
            stInventario = InventarioStatus.Aberto;
            dhAberturaLoj = dhAberturaLoja;
            cdUsuarioAberturaLoj = idUsuario;
        }
        #endregion
    }
}