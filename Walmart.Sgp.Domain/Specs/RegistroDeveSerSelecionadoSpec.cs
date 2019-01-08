using System;
using System.Collections.Generic;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Specs
{
    /// <summary>
    /// Especificação referente a seleção de um registro.
    /// </summary>
    public class RegistroDeveSerSelecionadoSpec : SpecBase<IEnumerable<IRegistroSelecionavel>>
    {
        private Func<IEnumerable<IRegistroSelecionavel>> m_obterRegistrosPersistidos;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="RegistroDeveSerSelecionadoSpec" />
        /// </summary>
        /// <param name="obterRegistrosPersistidos">O delegate que retorna os registros persistidos.</param>
        public RegistroDeveSerSelecionadoSpec(Func<IEnumerable<IRegistroSelecionavel>> obterRegistrosPersistidos)
        {
            m_obterRegistrosPersistidos = obterRegistrosPersistidos;
        }

        /// <summary>
        /// Verifica se a instância informada satisfaz a especificação.
        /// </summary>
        /// <param name="target">A instância a ser validada.</param>
        /// <returns>Se a especificação foi satisfeita pela instância.</returns>
        public override SpecResult IsSatisfiedBy(IEnumerable<IRegistroSelecionavel> target)
        {
            var persistidasExcetoAlteradas = m_obterRegistrosPersistidos().Except(target);

            if (target.Any(x => x.Selecionado) || persistidasExcetoAlteradas.Any(x => x.Selecionado))
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.AtLeastOneRecordMustBeSelected);
        }
    }
}
