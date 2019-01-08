using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Define a interface da classe de tradução das informações do Excel de importação para a entidade de domínio RelacaoItemLojaCD.
    /// </summary>
    public interface IRelacaoItemLojaCDVinculoExcelDataTranslator
    {
        /// <summary>
        /// Realiza a tradução das linhas em objetos do tipo RelacaoItemLojaCD.
        /// </summary>
        /// <param name="rows">Linhas do arquivo excel.</param>
        /// <returns>Retornar a lista contendo os objetos traduzidos e regras de negócio inválidas.</returns>
        IEnumerable<RelacaoItemLojaCDVinculo> Translate(IEnumerable<Row> rows);
    }
}
