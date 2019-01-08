using System;
using System.Linq;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: SAR do fornecedor inválido.
    /// </summary>
    public class MultisourcingDevePossuirSARFornecedorValidoSpec : MultisourcingSpecBase<long>
    {
        /// <summary>
        /// Obtém a chave para agrupamento.
        /// </summary>
        protected override Func<Multisourcing, long> Key
        {
            get { return m => m.Vendor9Digitos; }
        }

        /// <summary>
        /// A mensagem usada caso o grupo não seja válido.
        /// </summary>
        protected override string NotSatisfiedReason
        {
            get { return Texts.InvalidVendorSAR; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<long, Multisourcing> group)
        {
            var multisourcing = group.First();
            var tiposSAR = new[] { TipoSAR.Yes, TipoSAR.Required };

            return multisourcing.Fornecedor == null || multisourcing.Fornecedor.Parametros.Any(fp => tiposSAR.Contains(fp.tpStoreApprovalRequired));
        }
    }
}
