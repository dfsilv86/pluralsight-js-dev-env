using System;
using System.Linq;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.MultisourcingVendor.Specs
{
    /// <summary>
    /// Especificação referente a regra: Canal do Fornecedor Inválido.
    /// </summary>
    public class MultisourcingPossuiCanalValidoSpec : MultisourcingSpecBase<long>
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
            get { return Texts.InvalidVendorChannel; }
        }

        /// <summary>
        /// Valida se um grupo de multisourcing é válido.
        /// </summary>
        /// <param name="group">O grupo.</param>
        /// <returns>True caso válido, false caso contrário.</returns>
        protected override bool IsSatisfiedByGroup(IGrouping<long, Multisourcing> group)
        {
            var multisourcing = group.First();
            var tiposCanal = new[] { TipoCodigoReabastecimento.Dao, TipoCodigoReabastecimento.All };

            return multisourcing.Fornecedor == null || multisourcing.Fornecedor.Parametros.Any(fp => tiposCanal.Contains(fp.cdTipo));
        }
    }
}
