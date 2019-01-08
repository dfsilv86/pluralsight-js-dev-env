using System.Collections.Generic;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.IO.Excel;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Define a interface da classe de tradução das informações do Excel de importação para a entidade de domínio Multisourcing
    /// </summary>
    public interface IMultisourcingExcelDataTranslator
    {
        /// <summary>
        /// Realiza a tradução das linhas em objetos do tipo Multisourcing
        /// </summary>
        /// <param name="rows">Linhas do arquivo excel.</param>
        /// <returns>Retornar a lista contendo os objetos traduzidos e regras de negócio inválidas.</returns>
        IEnumerable<Multisourcing> Translate(IEnumerable<Row> rows); 
    }
}
