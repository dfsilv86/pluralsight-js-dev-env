using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Define a interface para as classes de escrita de arquivo no formato Excel.
    /// </summary>
    public interface IExcelWriter
    {
        /// <summary>
        /// Faz a escrita do conteúdo no arquivo.
        /// </summary>
        /// <param name="filepath">Caminho do arquivo excel a ser escrito.</param>
        /// <param name="rows">Conteúdo que será escrito no arquivo.</param>
        void Write(string filepath, IEnumerable<Row> rows);

        /// <summary>
        /// Faz a escrita do conteúdo no arquivo.
        /// </summary>
        /// <param name="rows">Conteúdo que será escrito no arquivo.</param>
        /// <returns>Retorna planilha preenchida.</returns>
        Stream Write(IEnumerable<Row> rows);

        /// <summary>
        /// Faz a escrita das linhas em um novo stream utilizando como base o stream recebido por parâmetro.
        /// </summary>
        /// <param name="inputStream">Stream da planilha que será utilizada como base.</param>
        /// <param name="rows">Linhas que serão escritas no novo Stream.</param>
        /// <returns>Retorna novo stream com as linhas que foram escritas.</returns>
        Stream Write(Stream inputStream, IEnumerable<Row> rows);
    }
}