using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Item.Specs
{
    /// <summary>
    /// Especificação referente a se um item detalhe pode ser utilizado como item de entrada ou saída no relacionamento.
    /// </summary>
    public class ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec : SpecBase<ItemDetalhe>
    {
        #region Fields
        private readonly IItemRelacionamentoService m_service;
        private readonly RelacionamentoItemPrincipal m_relacionamentoItemPrincipal;
        private bool m_utilizadoComoSaida;
        private TipoAcao m_tipoAcao;
        private Func<ItemDetalhe, SpecResult> m_isSatisfiedByTipoRelacionamentoFunc;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec"/>.
        /// </summary>
        /// <param name="service">O serviço de item relacionamento.</param>
        /// <param name="relacionamentoItemPrincipal">Entidade de relacionamento principal.</param>
        /// <param name="utilizadoComoSaida">Se será utilizado como saída no relacionamento.</param>
        public ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(IItemRelacionamentoService service, RelacionamentoItemPrincipal relacionamentoItemPrincipal, bool utilizadoComoSaida)
        {
            m_service = service;
            m_relacionamentoItemPrincipal = relacionamentoItemPrincipal;
            m_utilizadoComoSaida = utilizadoComoSaida;
            m_tipoAcao = relacionamentoItemPrincipal.IsNew ? TipoAcao.Insercao : TipoAcao.Alteracao;

            if (relacionamentoItemPrincipal.TipoRelacionamento == TipoRelacionamento.Manipulado)
            {
                m_isSatisfiedByTipoRelacionamentoFunc = IsSatisfiedByManipulado;
            }
            else if (relacionamentoItemPrincipal.TipoRelacionamento == TipoRelacionamento.Receituario)
            {
                m_isSatisfiedByTipoRelacionamentoFunc = IsSatisfiedByReceituario;
            }
            else
            {
                m_isSatisfiedByTipoRelacionamentoFunc = IsSatisfiedByVinculado;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(ItemDetalhe target)
        {
            if (target.TpStatus != TipoStatusItem.Ativo)
            {
                return NotSatisfied(Texts.ItemIsNotEnabled.With(ObterTextoEntradaSaida()));
            }

            if (m_utilizadoComoSaida)
            {
                var secundario = m_relacionamentoItemPrincipal.ObterSecundarioPorIdItemDetalhe(target.IDItemDetalhe);

                if (secundario != null && secundario.IsNew && !ReferenceEquals(target, secundario.ItemDetalhe))
                {
                    return NotSatisfied(Texts.InputItemAlreadyAddedToOutputItem);
                }
            }
            else if (IsSatisfiedByNaoEhSaida(target))
            {
                return Satisfied();
            }

            var podeEstarEmOutrosRelacionamentosSpecResult = new ItemDetalhePodeEstarEmOutrosRelacionamentosSpec(m_service, m_relacionamentoItemPrincipal, m_utilizadoComoSaida).IsSatisfiedBy(target);

            if (!podeEstarEmOutrosRelacionamentosSpecResult.Satisfied)
            {
                return podeEstarEmOutrosRelacionamentosSpecResult;
            }

            return m_isSatisfiedByTipoRelacionamentoFunc(target);
        }

        private bool IsSatisfiedByNaoEhSaida(ItemDetalhe target)
        {
            // Bug 3317: Relacionamento Receituário: Não está permitindo adicionar um item com o relacionamento tpVinculado = S, tpManipulado = P e tpReceituario = Null
            var ehNovoNoRelacionamento = m_relacionamentoItemPrincipal.EhNovoNoRelacionamento(target);
            var tipoRelacionamento = m_relacionamentoItemPrincipal.TipoRelacionamento;

            // Bug 4700:Vinculado: Está permitindo adicionar um Item Secundário com tpVinculado = S
            if (tipoRelacionamento == TipoRelacionamento.Vinculado && ehNovoNoRelacionamento && target.TpVinculado == TipoVinculado.Saida)
            {
                return false;
            }
            else if (target.TpVinculado == TipoVinculado.Saida
                && ((target.TpManipulado == TipoManipulado.Pai && tipoRelacionamento != TipoRelacionamento.Manipulado) // Bug 4698:BUG - Relac. Manipulado - Erro ao salvar item já relacionado.
                || (target.TpManipulado == TipoManipulado.NaoDefinido)) // Bug 4660:Relac. Manipulado: Não está permitindo relacionar um Item Saída como um item Pai, 
                && ((ehNovoNoRelacionamento && target.TpReceituario == TipoReceituario.NaoDefinido)
                 || (!ehNovoNoRelacionamento && target.TpReceituario == TipoReceituario.Insumo)))
            {
                return true;
            }
            else if (target.TpVinculado == TipoVinculado.Saida
                && target.TpReceituario == TipoReceituario.Insumo
                && ((ehNovoNoRelacionamento && target.TpManipulado == TipoManipulado.NaoDefinido)
                 || (!ehNovoNoRelacionamento && target.TpManipulado == TipoManipulado.Pai)))
            {
                return true;
            }

            return false;
        }

        private SpecResult IsSatisfiedByManipulado(ItemDetalhe target)
        {
            var ehNovoNoRelacionamento = m_relacionamentoItemPrincipal.EhNovoNoRelacionamento(target);

            if (m_utilizadoComoSaida)
            {
                if (target.TpVinculado != TipoVinculado.Entrada &&
                    target.TpReceituario != TipoReceituario.Transformado &&
                    ((m_tipoAcao == TipoAcao.Insercao && target.TpManipulado == TipoManipulado.NaoDefinido) ||
                    (ehNovoNoRelacionamento && target.TpManipulado == TipoManipulado.NaoDefinido) ||
                    (!ehNovoNoRelacionamento && target.TpManipulado == TipoManipulado.Derivado) ||
                    (m_tipoAcao == TipoAcao.Alteracao && target.TpManipulado == TipoManipulado.NaoDefinido)))
                {
                    return Satisfied();
                }
            }
            else if (target.TpVinculado != TipoVinculado.Entrada &&
                    target.TpReceituario != TipoReceituario.Transformado &&
                    ((m_tipoAcao == TipoAcao.Insercao && target.TpManipulado == TipoManipulado.NaoDefinido) ||
                    (m_tipoAcao == TipoAcao.Alteracao && target.TpManipulado == TipoManipulado.Pai)))
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.ItemCannotBeUsedAsManipulated.With(ObterTextoEntradaSaida()));
        }

        private SpecResult IsSatisfiedByReceituario(ItemDetalhe target)
        {
            var ehNovoNoRelacionamento = m_relacionamentoItemPrincipal.EhNovoNoRelacionamento(target);

            if (m_utilizadoComoSaida)
            {
                if (target.TpVinculado == TipoVinculado.NaoDefinido &&
                   ((m_tipoAcao == TipoAcao.Insercao && target.TpReceituario == TipoReceituario.NaoDefinido) ||
                    (m_tipoAcao == TipoAcao.Alteracao && target.TpReceituario == TipoReceituario.Transformado)) &&
                     target.TpManipulado == TipoManipulado.NaoDefinido)
                {
                    return Satisfied();
                }
            }
            else if (target.TpVinculado != TipoVinculado.Entrada &&
                    ((m_tipoAcao == TipoAcao.Insercao && target.TpReceituario == TipoReceituario.NaoDefinido) ||
                    (ehNovoNoRelacionamento && target.TpReceituario == TipoReceituario.NaoDefinido) ||
                    (target.TpReceituario == TipoReceituario.Insumo || target.TpReceituario == TipoReceituario.Transformado) ||
                    (m_tipoAcao == TipoAcao.Alteracao && target.TpReceituario == TipoReceituario.NaoDefinido)))
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.ItemIsInAnotherRelationship.With(ObterTextoEntradaSaida()));
        }

        private SpecResult IsSatisfiedByVinculado(ItemDetalhe target)
        {
            var ehNovoNoRelacionamento = m_relacionamentoItemPrincipal.EhNovoNoRelacionamento(target);

            if (m_utilizadoComoSaida)
            {
                if (((ehNovoNoRelacionamento && target.TpVinculado == TipoVinculado.NaoDefinido) ||
                    (!ehNovoNoRelacionamento && target.TpVinculado == TipoVinculado.Saida) ||
                    (!ehNovoNoRelacionamento && target.TpVinculado == TipoVinculado.NaoDefinido)) &&
                    target.TpReceituario != TipoReceituario.Transformado &&
                    target.TpManipulado != TipoManipulado.Derivado)
                {
                    return Satisfied();
                }
            }
            else if (((ehNovoNoRelacionamento && target.TpVinculado == TipoVinculado.NaoDefinido) ||
                     (!ehNovoNoRelacionamento && target.TpVinculado == TipoVinculado.Entrada)) &&
                     target.TpReceituario == TipoReceituario.NaoDefinido &&
                     target.TpManipulado == TipoManipulado.NaoDefinido)
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.ItemIsInAnotherRelationship.With(ObterTextoEntradaSaida()));
        }

        private string ObterTextoEntradaSaida()
        {
            return m_utilizadoComoSaida ? Texts.OutputItem : Texts.InputItem;
        }

        #endregion
    }
}
