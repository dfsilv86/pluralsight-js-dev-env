using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Representa uma linha do arquivo.
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Obtém ou define Lista de colunas.
        /// </summary>
        public IEnumerable<Column> Columns { get; set; }

        /// <summary>
        /// Obtém ou define Índice da linha.
        /// </summary>
        public int Index { get; set; }
    }
}
