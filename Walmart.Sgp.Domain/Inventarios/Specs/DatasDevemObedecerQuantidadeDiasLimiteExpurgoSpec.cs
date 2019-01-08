using System;
using System.Globalization;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.Inventarios.Specs
{
    /// <summary>
    /// Especificação referente a se as datas obedecem a quantidade de dias limite de expurgo.
    /// </summary>
    public class DatasDevemObedecerQuantidadeDiasLimiteExpurgoSpec : SpecBase<RangeValue<DateTime>>
    {
        #region Fields
        private readonly IParametroSistemaService m_parametroSistemaService;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DatasDevemObedecerQuantidadeDiasLimiteExpurgoSpec"/>.
        /// </summary>
        /// <param name="parametroSistemaService">O serviço de parâmetro sistema.</param>
        public DatasDevemObedecerQuantidadeDiasLimiteExpurgoSpec(IParametroSistemaService parametroSistemaService)
        {
            m_parametroSistemaService = parametroSistemaService;
        }
        #endregion

        /// <summary>
        /// Verifica se as datas especificadas satisfazem a especificação.
        /// </summary>
        /// <param name="target">As datas.</param>
        /// <returns>
        /// Se a especificação foi satisfeita pelas datas.
        /// </returns>
        public override SpecResult IsSatisfiedBy(RangeValue<DateTime> target)
        {
            var parametroSistema = m_parametroSistemaService.ObterPorNome("Transf_Dados_Hist_Inventario");
            int mesesLimiteExpurgo = Convert.ToInt32(parametroSistema.vlParametroSistema, CultureInfo.InvariantCulture);
            
            if (target.StartValue < DateTime.Now.AddMonths(-mesesLimiteExpurgo) ||
                target.EndValue < DateTime.Now.AddMonths(-mesesLimiteExpurgo))
            {
                return NotSatisfied(Texts.DatesMustObayPurgeLimit);
            }

            return Satisfied();
        }
    }
}
