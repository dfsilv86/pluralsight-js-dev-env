using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Implementação do leitor do Excel utilizando a lib EPPlus.
    /// </summary>
    public class EPPlusExcelReader : IExcelReader
    {
        private const int HeaderRowIndex = 1;
        private const int FirstDataRowIndex = 2;

        /// <summary>
        /// Faz a leitura do conteúdo do arquivo.
        /// </summary>
        /// <param name="filepath">Caminho do arquivo a ser lido..</param>
        /// <param name="columnsMetadata">Lista contendo a configuração das colunas que serão lidas.</param>
        /// <returns>Retorna o resultado da leitura do arquivo.</returns>
        public ExcelReaderResult Read(string filepath, IEnumerable<ColumnMetadata> columnsMetadata)
        {
            using (var package = new ExcelPackage(new FileInfo(filepath)))
            {
                return ReadInternal(package, columnsMetadata);
            }
        }

        /// <summary>
        /// Faz a leitura do conteúdo do arquivo.
        /// </summary>
        /// <param name="stream">Stream contendo o arquivo excel a ser lido.</param>
        /// <param name="columnsMetadata">Lista contendo a configuração das colunas que serão lidas.</param>
        /// <returns>Retorna o resultado da leitura do arquivo.</returns>
        public ExcelReaderResult Read(Stream stream, IEnumerable<ColumnMetadata> columnsMetadata)
        {
            using (var package = new ExcelPackage(stream))
            {
                return ReadInternal(package, columnsMetadata);
            }
        }

        private static ExcelReaderResult ReadInternal(ExcelPackage package, IEnumerable<ColumnMetadata> columnsMetadata)
        {
            var worksheet = package.Workbook.Worksheets.First();

            var result = GetColumnsRealIndex(columnsMetadata, worksheet);
            var rows = new List<Row>();

            for (int rowIndex = FirstDataRowIndex; rowIndex <= worksheet.Dimension.End.Row; rowIndex++)
            {
                var columns = new List<Column>();

                foreach (var columnIndex in result.ColumnsRealIndex)
                {
                    columns.Add(new Column
                    {
                        Metadata = columnIndex.ColumnMetadata,
                        Value = worksheet.Cells[rowIndex, columnIndex.Index].Text
                    });
                }

                rows.Add(new Row
                {
                    Index = rowIndex,
                    Columns = columns
                });
            }

            return new ExcelReaderResult(rows, result.ColumnsNotFound);
        }

        private static ColumnRealIndexResult GetColumnsRealIndex(IEnumerable<ColumnMetadata> columnsMetadata, ExcelWorksheet worksheet)
        {
            var columnsRealIndex = new List<ColumnRealIndex>();
            var columnsNotFound = new List<ColumnMetadata>();

            foreach (var columnMetadata in columnsMetadata)
            {
                var columnRealIndex = FindColumnByNameAndIndex(worksheet, columnMetadata);

                if (columnRealIndex == null)
                {
                    columnsNotFound.Add(columnMetadata);
                }
                else
                {
                    columnsRealIndex.Add(columnRealIndex);
                }
            }

            return new ColumnRealIndexResult(columnsRealIndex, columnsNotFound);
        }

        private static ColumnRealIndex FindColumnByNameAndIndex(ExcelWorksheet worksheet, ColumnMetadata columnMetadata)
        {
            for (int columnIndex = worksheet.Dimension.Start.Column; columnIndex <= worksheet.Dimension.End.Column; columnIndex++)
            {
                if (CompareColumnName(worksheet.Cells[HeaderRowIndex, columnIndex].Text, columnMetadata.Name) && columnIndex == columnMetadata.Index)
                {
                    return new ColumnRealIndex
                    {
                        ColumnMetadata = columnMetadata,
                        Index = columnIndex
                    };
                }
            }

            return null;
        }

        private static bool CompareColumnName(string columnExcel, string columnMetadata)
        {
            return string.Compare(columnExcel.Trim(), columnMetadata.Trim(), true, RuntimeContext.Current.Culture) == 0;
        }

        private class ColumnRealIndex
        {
            public ColumnMetadata ColumnMetadata { get; set; }

            public int Index { get; set; }
        }

        private class ColumnRealIndexResult
        {
            public ColumnRealIndexResult(IEnumerable<ColumnRealIndex> columnsRealIndex, IEnumerable<ColumnMetadata> columnsNotFound)
            {
                ColumnsRealIndex = columnsRealIndex ?? new ColumnRealIndex[0];
                ColumnsNotFound = columnsNotFound ?? new ColumnMetadata[0];
            }

            public IEnumerable<ColumnRealIndex> ColumnsRealIndex { get; private set; }

            public IEnumerable<ColumnMetadata> ColumnsNotFound { get; private set; }
        }
    }
}
