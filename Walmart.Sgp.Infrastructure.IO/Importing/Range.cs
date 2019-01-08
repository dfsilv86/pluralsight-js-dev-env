using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Importing
{
    /// <summary>
    /// Representa um intervalo em uma string ou arquivo.
    /// </summary>
    public struct Range
    {
        /// <summary>
        /// Inicia uma nova instância da estrutura <see cref="Range"/>.
        /// </summary>
        /// <param name="offset">O ponto de início.</param>
        /// <param name="length">O tamanho do intervalo.</param>
        public Range(int offset, int length)
            : this()
        {
            this.Offset = offset;
            this.Length = length;
        }

        /// <summary>
        /// Obtém o ponto de início.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// Obtém o tamanho do intervalo.
        /// </summary>
        public int Length { get; private set; }
        
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Range x, Range y)
        {
            return x.Offset == y.Offset && x.Length == y.Length;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Range x, Range y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Avança para o próximo intervalo de tamanho específicado.
        /// </summary>
        /// <param name="length">O quanto avançar.</param>
        /// <returns>O intervalo seguinte.</returns>
        public Range Advance(int length)
        {
            return new Range(this.Offset + this.Length, length);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Range))
            {
                return false;
            }

            Range range = (Range)obj;

            return this.Offset == range.Offset && this.Length == range.Length;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Offset.GetHashCode() ^ this.Length.GetHashCode();
        }
    }
}
