using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Excel.Specs
{
    /// <summary>
    /// Especificação responsável por validar a extensão do arquivo Excel.
    /// </summary>
    public class ExcelExtensionSpec : SpecBase<FileVaultTicket>
    {
        private readonly string[] supportedExtensions = new[] { ".xlsx" };

        /// <summary>
        /// Verifica se a extensão satisfaz a especificação.
        /// </summary>
        /// <param name="target">O ticket do arquivo.</param>
        /// <returns>
        /// Se a especificação foi satisfeita.
        /// </returns>
        public override SpecResult IsSatisfiedBy(FileVaultTicket target)
        {
            if (supportedExtensions.Any(supportedExtension => supportedExtension.Equals(new FileInfo(target.FileName).Extension, StringComparison.OrdinalIgnoreCase)))
            {
                return Satisfied();
            }

            return NotSatisfied(Texts.ExtensionNotSupported);
        }
    }
}