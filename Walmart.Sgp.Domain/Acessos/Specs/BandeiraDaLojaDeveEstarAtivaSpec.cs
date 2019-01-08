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
    /// Especificação que valida se uma bandeira vinculada a loja está ativa.
    /// </summary>
    public class BandeiraDaLojaDeveEstarAtivaSpec : SpecBase<Loja>
    {
        private readonly IBandeiraGateway m_bandeiraGateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="BandeiraDaLojaDeveEstarAtivaSpec" />.
        /// </summary>
        /// <param name="bandeiraGateway">O data table gateway de bandeira.</param>
        public BandeiraDaLojaDeveEstarAtivaSpec(IBandeiraGateway bandeiraGateway)
        {
            m_bandeiraGateway = bandeiraGateway;
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
            var bandeira = m_bandeiraGateway.ObterPorIdLoja(target.Id);

            if (bandeira.BlAtivo == BandeiraStatus.Ativo)
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.storeChainIsInactive);
        }
    }
}
