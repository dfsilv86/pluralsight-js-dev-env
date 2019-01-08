using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Classe utilizada para configurar as colunas que serão lidas.
    /// </summary>
    public class ColumnMetadata
    {
        /// <summary>
        /// Obtém ou define Nome da coluna no arquivo.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtém ou define Índice da coluna (inicia em 1).
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Obtém ou define Tipo da coluna.
        /// </summary>
        public Type ColumnType { get; set; }

        /// <summary>
        /// Obtém ou define Comprimento da informação.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Obtém ou define Quantidade de dígitos após a vírcula em um número decimal.
        /// </summary>
        public int? Scale { get; set; }

        /// <summary>
        /// Obtém ou define um valor que indica se IgnoreEmpty.
        /// </summary>
        public bool IgnoreEmpty { get; set; }

        /// <summary>
        /// Obtém ou define a lista de validações customizadas que serão executadas após validações de tipo de dados.
        /// </summary>
        public IEnumerable<ISpec<Column>> CustomValidate { get; set; }
    }
}
