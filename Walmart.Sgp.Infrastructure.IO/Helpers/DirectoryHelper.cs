using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.IO.Helpers
{
    /// <summary>
    /// Helpers para manipular diretórios.
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Remove um diretório, se estiver vazio, e verifica/remove diretórios pai até o número de diretórios pai especificados se também estão vazios.
        /// </summary>
        /// <param name="directoryName">O caminho completo para o diretório.</param>
        /// <param name="parents">O número de pais.</param>
        /// <remarks>
        /// Exemplo: se especificado o diretório C:\Foo\Bar\Xyz\Abc com número de pais = 2, remove Abc (o diretório), Xyz (o primeiro pai), e Bar (o segundo pai), caso estejam vazios, deixando apenas C:\Foo.
        /// </remarks>
        public static void DeleteDirectoriesWhenEmpty(string directoryName, int parents)
        {
            string thePath = directoryName;
            int counter = 0;

            while (counter <= parents)
            {
                if (Directory.Exists(thePath))
                {
                    if (!Directory.EnumerateFileSystemEntries(thePath).Any())
                    {
                        Directory.Delete(thePath);
                    }
                    else
                    {
                        break;
                    }
                }

                thePath = Path.GetDirectoryName(thePath);
                counter++;
            }
        }
    }
}
