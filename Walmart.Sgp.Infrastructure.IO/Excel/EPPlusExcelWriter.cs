using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Implementação do escritor do excel utilizando a lib EPPlus.
    /// </summary>
    public class EPPlusExcelWriter : IExcelWriter
    {
        private const int HeaderRowIndex = 1;
        private const string DefaultWorksheet = "Plan1";

        /// <summary>
        /// Faz a escrita do conteúdo no arquivo.
        /// </summary>
        /// <param name="filepath">Caminho do arquivo excel a ser escrito.</param>
        /// <param name="rows">Conteúdo que será escrito no arquivo.</param>
        public void Write(string filepath, IEnumerable<Row> rows)
        {
            var fileInfo = new FileInfo(filepath);

            using (var package = new ExcelPackage(fileInfo))
            {
                WriteInternal(package, rows);
                package.Save();
            }
        }

        /// <summary>
        /// Faz a escrita do conteúdo no arquivo.
        /// </summary>
        /// <param name="rows">Conteúdo que será escrito no arquivo.</param>
        /// <returns>Retorna planilha preenchida.</returns>
        public Stream Write(IEnumerable<Row> rows)
        {
            return Write(new MemoryStream(), rows);
        }

        /// <summary>
        /// Faz a escrita das linhas em um novo stream utilizando como base o stream recebido por parâmetro.
        /// </summary>
        /// <param name="inputStream">Stream da planilha que será utilizada como base.</param>
        /// <param name="rows">Linhas que serão escritas no novo Stream.</param>
        /// <returns>Retorna novo stream com as linhas que foram escritas.</returns>
        public Stream Write(Stream inputStream, IEnumerable<Row> rows)
        {
            using (var package = new ExcelPackage(inputStream))
            {
                WriteInternal(package, rows);
                package.Save();

                package.Stream.Position = 0;
                return package.Stream;
            }
        }

        private static void CreateHeader(ExcelWorksheet worksheet, Row row)
        {
            foreach (var column in row.Columns)
            {
                worksheet.Cells[HeaderRowIndex, column.Metadata.Index].Value = column.Metadata.Name;
            }
        }

        private static void WriteInternal(ExcelPackage package, IEnumerable<Row> rows)
        {
            if (package.Workbook.Worksheets.Count == 0)
            {
                package.Workbook.Worksheets.Add(DefaultWorksheet);
            }

            var worksheet = package.Workbook.Worksheets.First();

            CreateHeader(worksheet, rows.First());

            foreach (var row in rows)
            {
                foreach (var column in row.Columns)
                {
                    worksheet.Cells[row.Index, column.Metadata.Index].Value = column.Value;
                }
            }

            worksheet.Cells.AutoFitColumns();
        }
    }
}