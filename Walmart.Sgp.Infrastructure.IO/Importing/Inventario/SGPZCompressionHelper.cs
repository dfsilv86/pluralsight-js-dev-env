using System;
using System.IO;
using ICSharpCode.SharpZipLib.LZW;

namespace Walmart.Sgp.Infrastructure.IO.Importing.Inventario
{
    /// <summary>
    /// SGPZ compression helper.
    /// </summary>
    public static class SGPZCompressionHelper
    {
        /// <summary>
        /// Realiza a extração.
        /// </summary>
        /// <param name="arquivo">O arquivo.</param>
        /// <param name="destino">O destino</param>
        public static void Extrair(string arquivo, string destino)
        {
            Stream stream = null;
            FileStream fileStream = null;

            try
            {
                byte[] buffer = new byte[4096];

                // TODO: encontrar uma alternativa ou importar o SharpZip inteiro
                stream = new LzwInputStream(File.Open(arquivo, FileMode.Open));
                fileStream = File.Create(destino);

                int bytesLidos;

                while ((bytesLidos = stream.Read(buffer, 0, buffer.Length)) > 0)
                { 
                    fileStream.Write(buffer, 0, bytesLidos);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }
    }
}
