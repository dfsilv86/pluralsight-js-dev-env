using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Domain.MultisourcingVendor
{
    /// <summary>
    /// Compara duas instâncias de multisourcing considerando apenas as propriedades IDRelacionamentoItemSecundario e IDCD.
    /// </summary>
    public class MultisourcingEqualityComparer : IEqualityComparer<Multisourcing>
    {
        /// <summary>
        ///  Realiza a comparação entre multisourcings considerando apenas as propriedades IDRelacionamentoItemSecundario e IDCD.
        /// </summary>
        /// <param name="x">O primeiro multisourcing a ser comparado.</param>
        /// <param name="y">O segundo multisourcing a ser comparado.</param>
        /// <returns>Retornar true caso os objetos sejam iguais, do contrário retorna false.</returns>
        public bool Equals(Multisourcing x, Multisourcing y)
        {
            return x.IDRelacionamentoItemSecundario == y.IDRelacionamentoItemSecundario && x.IDCD == y.IDCD;
        }

        /// <summary>
        /// Obtém o hashcode da instância.
        /// </summary>
        /// <param name="obj">A instância que será obtida o hashcode.</param>
        /// <returns>Retorna o hashcode da instância.</returns>
        public int GetHashCode(Multisourcing obj)
        {
            return obj.IDRelacionamentoItemSecundario.GetHashCode() + obj.IDCD.GetHashCode();
        }
    }
}
