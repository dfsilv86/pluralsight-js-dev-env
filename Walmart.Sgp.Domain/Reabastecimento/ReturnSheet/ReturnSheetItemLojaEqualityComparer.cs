using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Walmart.Sgp.Domain.Reabastecimento
{
    /// <summary>
    /// Responsável por comparar dois objetos do tipo ReturnSheetItemLoja.
    /// </summary>
    public class ReturnSheetItemLojaEqualityComparer : IEqualityComparer<ReturnSheetItemLoja>
    {
        /// <summary>
        /// Realiza a comparação entre um ReturnSheetItemLoja enviado pela interface e outro ReturnSheetItemLoja persistido, considerando IdItemDetalhe e IdLoja como chave.
        /// </summary>
        /// <param name="x">O objeto ReturnSheetItemLoja persistido.</param>
        /// <param name="y">O objeto ReturnSheetItemLoja enviado pela interface.</param>
        /// <returns>Retorna true caso os objetos sejam iguais, do contrário retorna false.</returns>
        public bool Equals(ReturnSheetItemLoja x, ReturnSheetItemLoja y)
        {
            return x.IdItemDetalhe == y.IdItemDetalheEntrada && x.IdLoja == y.IdLoja;
        }

        /// <summary>
        /// Obtem o HashCode da instância corrente.
        /// </summary>
        /// <param name="obj">A instância.</param>
        /// <returns>Retorna o HashCode da instância.</returns>
        public int GetHashCode(ReturnSheetItemLoja obj)
        {
            return obj.IdItemDetalhe.GetHashCode() + 
                obj.IdItemDetalheEntrada.GetHashCode() + 
                obj.IdLoja.GetHashCode();
        }
    }
}
