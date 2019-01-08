using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.EstruturaMercadologica.Specs
{
    /// <summary>
    /// Especificação referente a Supercenter (Varejo, cdSistema=1) permite apenas arquivos finais.
    /// </summary>
    public class SupercenterPermiteApenasArquivosFinaisSpec : SpecBase<Loja>
    {
        /// <summary>
        /// Verifica se a loja informado satisfaz a especificação.
        /// </summary>
        /// <param name="target">A loja.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelo loja.
        /// </returns>
        public override SpecResult IsSatisfiedBy(Loja target)
        {            
            // cadLoja.aspx.cs linha 334
            if (target.cdSistema == 1 && target.TipoArquivoInventario != TipoArquivoInventario.Final)
            {
                return NotSatisfied(Texts.SupercenterAcceptsOnlyFinalInventoryFiles);
            }

            return Satisfied();
        }
    }
}
