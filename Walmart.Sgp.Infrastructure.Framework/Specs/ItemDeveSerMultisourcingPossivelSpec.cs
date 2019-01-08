using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Specs
{
    /// <summary>
    /// Especificação para verificar se um item de saída pode ser multisourcing.
    /// </summary>
    public class ItemDeveSerMultisourcingPossivelSpec : SpecBase<Tuple<long, long, long>>
    {
        private Func<long, long, long, int> m_contagemItensEntradaPorSaidaECD = null;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ItemDeveSerMultisourcingPossivelSpec" /> para verificar se um item pode ser multisourcing.
        /// </summary>
        /// <param name="contagemItensEntradaPorSaidaECD">Func que retorna a quantidade de itens de entrada para um item de venda e seu CD.</param>
        public ItemDeveSerMultisourcingPossivelSpec(Func<long, long, long, int> contagemItensEntradaPorSaidaECD)
        {
            this.m_contagemItensEntradaPorSaidaECD = contagemItensEntradaPorSaidaECD;
        }

        #region Methods
        /// <summary>
        /// Verifica se a tupla de cdItem + cdCD satisfazem os requisitos.
        /// </summary>
        /// <param name="target">Um <see cref="Tuple" /> (long,long,long) aonde Item1 = cdItem, Item2 = cdCD, Item3 = cdSistema.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo object.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Tuple<long, long, long> target)
        {
            if (!Possivel(target.Item1, target.Item2, target.Item3))
            {
                return NotSatisfied(Texts.NoMultisourcingItem);
            }

            return Satisfied();
        }

        private bool Possivel(long cdItem, long cdCD, long cdSistema)
        {
            return m_contagemItensEntradaPorSaidaECD(cdItem, cdCD, cdSistema) > 1;
        }

        #endregion
    }
}
