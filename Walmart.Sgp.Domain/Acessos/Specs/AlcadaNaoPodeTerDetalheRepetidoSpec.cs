using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Acessos.Specs
{
    /// <summary>
    /// Especificação de persistencia da alçada.
    /// </summary>
    public class AlcadaNaoPodeTerDetalheRepetidoSpec : SpecBase<Alcada>
    {
        private readonly Func<int?, int?, Alcada> m_obterEstruturado;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AlcadaNaoPodeTerDetalheRepetidoSpec"/>.
        /// </summary>
        /// <param name="obterEstruturado">O delegate que busca uma Alçada estruturada.</param>
        public AlcadaNaoPodeTerDetalheRepetidoSpec(Func<int?, int?, Alcada> obterEstruturado)
        {
            this.m_obterEstruturado = obterEstruturado;
        }

        /// <summary>
        /// Verifica se o alvo informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">O alvo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo alvo.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Alcada target)
        {
            var alcada = m_obterEstruturado(target.Id, null);

            var detalhesFiltrados = target.Detalhe.GroupBy(g => new { g.Bandeira.IDBandeira, g.Departamento.IDDepartamento, g.RegiaoAdministrativa.IdRegiaoAdministrativa }).ToList();

            foreach (var detalhe in detalhesFiltrados)
            {
                if (ValidarDetalhe(detalhe, alcada))
                {
                    return NotSatisfied(Texts.AlcadaAlreadyExistsForRegionFlagAndDepartment);
                }
            }

            return Satisfied();
        }

        private bool ValidarDetalhe(IGrouping<object, AlcadaDetalhe> detalhe, Alcada alcada)
        {
            var result = detalhe.Count() > 1 || (alcada != null && alcada.Detalhe.Count > 0 && alcada.Detalhe.Any(d => d.Bandeira.IDBandeira == detalhe.First().Bandeira.IDBandeira &&
                                             d.RegiaoAdministrativa.IdRegiaoAdministrativa == detalhe.First().RegiaoAdministrativa.IdRegiaoAdministrativa &&
                                             d.Departamento.IDDepartamento == detalhe.First().Departamento.IDDepartamento));

            return result;
        }
    }
}
