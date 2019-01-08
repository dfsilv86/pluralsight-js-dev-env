using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.Domain.Item
{
    /// <summary>
    /// Representa uma RelacionamentoItemPrincipal.
    /// </summary>
    public class RelacionamentoItemPrincipal : EntityBase, IAggregateRoot
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RelacionamentoItemPrincipal"/>
        /// </summary>
        public RelacionamentoItemPrincipal()
        {
            RelacionamentoSecundario = new List<RelacionamentoItemSecundario>();
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
                return IDRelacionamentoItemPrincipal;
            }

            set
            {
                IDRelacionamentoItemPrincipal = value;
            }
        }

        /// <summary>
        /// Obtém ou define IDRelacionamentoItemPrincipal.
        /// </summary>
        public int IDRelacionamentoItemPrincipal { get; set; }

        /// <summary>
        /// Obtém ou define cdSistema.
        /// </summary>
        public int? cdSistema { get; set; }

        /// <summary>
        /// Obtém ou define IDTipoRelacionamento.
        /// </summary>
        public int? IDTipoRelacionamento { get; set; }

        /// <summary>
        /// Obtém ou define o tipo relacionamento.
        /// </summary>
        public TipoRelacionamento TipoRelacionamento
        {
            get
            {
                return IDTipoRelacionamento.HasValue ? (TipoRelacionamento)IDTipoRelacionamento.Value : null;
            }

            set
            {
                IDTipoRelacionamento = value.Value;
            }
        }

        /// <summary>
        /// Obtém ou define IDItemDetalhe.
        /// </summary>
        public int IDItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define o item detalhe.
        /// </summary>
        public ItemDetalhe ItemDetalhe { get; set; }

        /// <summary>
        /// Obtém ou define IDDepartamento.
        /// </summary>
        public int? IDDepartamento { get; set; }

        /// <summary>
        /// Obtém ou define IDCategoria.
        /// </summary>
        public long? IDCategoria { get; set; }

        /// <summary>
        /// Obtém ou define qtProdutoBruto.
        /// </summary>
        public decimal? QtProdutoBruto { get; set; }

        /// <summary>
        /// Obtém ou define pcRendimentoReceita.
        /// </summary>
        public float? PcRendimentoReceita { get; set; }

        /// <summary>
        /// Obtém ou define qtProdutoAcabado.
        /// </summary>
        public decimal? QtProdutoAcabado { get; set; }

        /// <summary>
        /// Obtém ou define pcQuebra.
        /// </summary>
        public float? PcQuebra { get; set; }

        /// <summary>
        /// Obtém ou define dhCadastro.
        /// </summary>
        public DateTime? DhCadastro { get; set; }

        /// <summary>
        /// Obtém ou define dhAlteracao.
        /// </summary>
        public DateTime? DhAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define psUnitario.
        /// </summary>
        public decimal? PsUnitario { get; set; }

        /// <summary>
        /// Obtém ou define blReprocessamentoManual.
        /// </summary>
        public bool? BlReprocessamentoManual { get; set; }

        /// <summary>
        /// Obtém ou define o statuds do repreocessamento de custo.
        /// </summary>
        public StatusReprocessamentoCusto StatusReprocessamentoCusto { get; set; }

        /// <summary>
        /// Obtém ou define dtInicioReprocessamentoCusto.
        /// </summary>
        public DateTime? DtInicioReprocessamentoCusto { get; set; }

        /// <summary>
        /// Obtém ou define dtFinalReprocessamentoCusto.
        /// </summary>
        public DateTime? DtFinalReprocessamentoCusto { get; set; }

        /// <summary>
        /// Obtém ou define idUsuarioReprocessamento.
        /// </summary>
        public int? IdUsuarioReprocessamento { get; set; }

        /// <summary>
        /// Obtém ou define descErroReprocessamento.
        /// </summary>
        public string DescErroReprocessamento { get; set; }

        /// <summary>
        /// Obtém ou define idUsuarioAlteracao.
        /// </summary>
        public int? IdUsuarioAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define o departamento.
        /// </summary>
        public Departamento Departamento { get; set; }

        /// <summary>
        /// Obtém ou define os relacionamentos de itens secundários.
        /// </summary>
        public IList<RelacionamentoItemSecundario> RelacionamentoSecundario { get; set; }

        /// <summary>
        /// Obtém ou define a categoria.
        /// </summary>
        public Categoria Categoria { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Agenda o item para que possa ser reprocessado.
        /// </summary>
        public void MarcarParaReprocessar()
        {
            StatusReprocessamentoCusto = StatusReprocessamentoCusto.Agendado; // Agendado para Reprocessamento de custo.
            BlReprocessamentoManual = false; // Coloca como reprocessamento não manual para o serviço poder reprocessar.
            DescErroReprocessamento = string.Empty;
            DtInicioReprocessamentoCusto = null;
            DtFinalReprocessamentoCusto = null;
            IdUsuarioAlteracao = RuntimeContext.Current.User.Id;
        }

        /// <summary>
        /// Prepara os relacionamentos secundários.
        /// </summary>
        public void MarcarTipoItemEntrada()
        {
            if (TipoRelacionamento == TipoRelacionamento.Manipulado)
            {
                foreach (var s in RelacionamentoSecundario)
                {
                    s.TpItem = TipoItemEntrada.Insumo;
                }
            }
        }

        /// <summary>
        /// Obtém o secundário pelo id do item detalhe.
        /// </summary>
        /// <param name="idItemDetalhe">O id do item detalhe.</param>
        /// <returns>O secundário.</returns>
        public RelacionamentoItemSecundario ObterSecundarioPorIdItemDetalhe(int idItemDetalhe)
        {
            return RelacionamentoSecundario.SingleOrDefault(s => s.IDItemDetalhe.HasValue && s.IDItemDetalhe.Value == idItemDetalhe);
        }

        /// <summary>
        /// Verifica se o item detalhe é novo no relacionamento.
        /// </summary>
        /// <param name="itemDetalhe">O item detalhe.</param>
        /// <returns>Se é novo no relacionamento.</returns>
        public bool EhNovoNoRelacionamento(ItemDetalhe itemDetalhe)
        {
            if (ItemDetalhe == null || itemDetalhe.Id == ItemDetalhe.Id)
            {
                return IsNew;
            }

            var secundario = ObterSecundarioPorIdItemDetalhe(itemDetalhe.IDItemDetalhe);

            return secundario == null || secundario.IsNew;
        }  

        /// <summary>
        /// Verifica se o item principal no relacionamento é uma entrada.
        /// </summary>
        /// <remarks>
        /// Quando o item principal é uma entrada no relacionamento, então os secundários são as saídas.
        /// Manipulado = principal entrada x secundários saída
        /// Receituário = principal saida x secundários Entrada
        /// Vinculado = principal saida x secundários Entrada
        /// </remarks>        
        /// <returns>True se o item principal é a entrada no relacionamento.</returns>
        public bool EhEntrada()
        {
            return !EhSaida();
        }

        /// <summary>
        /// Verifica se o item principal no relacionamento é uma saída.
        /// </summary>
        /// <remarks>
        /// Quando o item principal é uma saída no relacionamento, então os secundáriso são as entradas.
        /// Manipulado = principal entrada x secundários saída
        /// Receituário = principal saida x secundários Entrada
        /// Vinculado = principal saida x secundários Entrada        
        /// </remarks>
        /// <returns>True se o item principal é a saída no relacionamento.</returns>
        public bool EhSaida()
        {
            return TipoRelacionamento != TipoRelacionamento.Manipulado;
        }
        #endregion
    }
}
