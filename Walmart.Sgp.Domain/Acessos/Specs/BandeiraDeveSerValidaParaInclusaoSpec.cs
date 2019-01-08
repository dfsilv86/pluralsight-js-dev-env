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
    /// Especificação que valida se uma bandeira pode ser incluída na permissão.
    /// </summary>
    public class BandeiraDeveSerValidaParaInclusaoSpec : SpecBase<Bandeira>
    {
        private readonly IBandeiraGateway m_bandeiraGateway;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="BandeiraDeveSerValidaParaInclusaoSpec" />.
        /// </summary>
        /// <param name="bandeiraGateway">O data table gateway de bandeira.</param>
        public BandeiraDeveSerValidaParaInclusaoSpec(IBandeiraGateway bandeiraGateway)
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
        public override SpecResult IsSatisfiedBy(Bandeira target)
        {
            var bandeira = m_bandeiraGateway.Find("BlAtivo", "IDBandeira = @idBandeira", new { idBandeira = target.IDBandeira }).Single();

            if (bandeira.BlAtivo == BandeiraStatus.Ativo)
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.inactiveChain);
        }
    }
}
