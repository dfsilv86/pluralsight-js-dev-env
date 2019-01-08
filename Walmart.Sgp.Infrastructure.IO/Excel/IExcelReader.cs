using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Define a interface para as classes de leitura de arquivo no formato Excel.
    /// </summary>
    public interface IExcelReader
    {
        /// <summary>
        /// Faz a leitura do conteúdo do arquivo.
        /// </summary>
        /// <param name="filepath">Caminho do arquivo excel a ser lido.</param>
        /// <param name="columnsMetadata">Lista contendo a configuração das colunas que serão lidas.</param>
        /// <returns>Retorna o resultado da leitura do arquivo.</returns>
        ExcelReaderResult Read(string filepath, IEnumerable<ColumnMetadata> columnsMetadata);

        /// <summary>
        /// Faz a leitura do conteúdo do arquivo.
        /// </summary>
        /// <param name="stream">Stream contendo o arquivo excel a ser lido.</param>
        /// <param name="columnsMetadata">Lista contendo a configuração das colunas que serão lidas.</param>
        /// <returns>Retorna o resultado da leitura do arquivo.</returns>
        ExcelReaderResult Read(Stream stream, IEnumerable<ColumnMetadata> columnsMetadata);
    }
}
