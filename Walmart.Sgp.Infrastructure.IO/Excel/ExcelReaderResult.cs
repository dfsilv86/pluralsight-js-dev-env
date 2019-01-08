using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Representa o resultado de uma leitura de arquivo excel.
    /// </summary>
    public class ExcelReaderResult
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ExcelReaderResult" />.
        /// </summary>
        /// <param name="rows">As linhas lidas.</param>
        /// <param name="columnsNotFound">As colunas não encontradas no arquivo.</param>
        public ExcelReaderResult(IEnumerable<Row> rows, IEnumerable<ColumnMetadata> columnsNotFound)
        {
            Rows = rows ?? new Row[0];
            ColumnsNotFound = columnsNotFound ?? new ColumnMetadata[0];
        }
        
        /// <summary>
        /// Obtém as linhas lidas.
        /// </summary>
        public IEnumerable<Row> Rows { get; private set; }

        /// <summary>
        /// Obtém o metadado das colunas que não foram encontradas no arquivo.
        /// </summary>
        public IEnumerable<ColumnMetadata> ColumnsNotFound { get; private set; }

    }
}
