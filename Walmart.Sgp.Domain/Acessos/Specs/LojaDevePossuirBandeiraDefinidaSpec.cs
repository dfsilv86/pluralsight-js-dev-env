using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos.Specs
{
    /// <summary>
    /// Especificação que valida se a loja possui uma bandeira.
    /// </summary>
    public class LojaDevePossuirBandeiraDefinidaSpec : SpecBase<Loja>
    {
        private readonly ILojaGateway m_lojaGateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="LojaDevePossuirBandeiraDefinidaSpec" />.
        /// </summary>
        /// <param name="lojaGateway">O data table gateway de loja.</param>
        public LojaDevePossuirBandeiraDefinidaSpec(ILojaGateway lojaGateway)
        {
            m_lojaGateway = lojaGateway;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Loja target)
        {
            var loja = m_lojaGateway.Find("IDBandeira", "IDLoja = @idLoja", new { idLoja = target.IDLoja }).Single();

            if (loja.IDBandeira.HasValue)
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.storeHasNotChain);
        }
    }
}
