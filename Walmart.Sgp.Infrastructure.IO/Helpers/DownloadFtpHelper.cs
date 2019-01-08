using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.IO.Helpers
{
    /// <summary>
    /// Classe helper para download de arquivos de um servidor ftp.
    /// </summary>
    public static class DownloadFtpHelper
    {
        /// <summary>
        /// Lista arquivos presentes em um servidor FTP.
        /// </summary>
        /// <param name="diretorioServidor">A Uri que aponta para o servidor e diretório FTP.</param>
        /// <param name="credencialRede">Credenciais para o servidor.</param>
        /// <param name="mascaraFiltroArquivos">A máscara de pesquisa de arquivo (ou null para todos os arquivos).</param>
        /// <param name="isWindowsFtp">Indica se o servidor FTP é o servidor FTP do IIS no Windows.</param>
        /// <returns>Lista de arquivos encontrados.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public static IEnumerable<ArquivoFtp> EncontrarArquivos(Uri diretorioServidor, NetworkCredential credencialRede, string mascaraFiltroArquivos, bool isWindowsFtp)
        {
            List<ArquivoFtp> itensServidor = new List<ArquivoFtp>();

            mascaraFiltroArquivos = mascaraFiltroArquivos ?? string.Empty;

            diretorioServidor = FixUriIfVsFtpd(diretorioServidor, credencialRede);
            try
            {
                FtpWebRequest listRequest = (FtpWebRequest)FtpWebRequest.Create(diretorioServidor);
                listRequest.Credentials = credencialRede;
                listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                listRequest.UsePassive = true;
                listRequest.Proxy = null;

                using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(listResponse.GetResponseStream()))
                    {
                        string linha = streamReader.ReadLine(); // linha de total

                        if (!string.IsNullOrEmpty(linha))
                        {
                            linha = streamReader.ReadLine();
                        }

                        while (!string.IsNullOrEmpty(linha))
                        {
                            // TODO: não suporta nomes de arquivo com espaços.
                            var componentes = linha.TrimEnd().Split(' ');
                            string entryName = componentes.Last();

                            if (entryName != "." && entryName != ".." && EntryMatchesMask(entryName, mascaraFiltroArquivos + (mascaraFiltroArquivos.Contains("*") ? string.Empty : "*")))
                            {
                                var theEntry = Parse(diretorioServidor, isWindowsFtp, linha, componentes, entryName);

                                itensServidor.Add(theEntry);
                            }

                            linha = streamReader.ReadLine();
                        }

                        streamReader.Close();
                    }
                }

                return itensServidor;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(Texts.ErrorDuringFtpCommunication2.With(ex.Message), ex);
            }
        }

        /// <summary>
        /// Realiza o download dos arquivos informados.
        /// </summary>
        /// <param name="diretorioDestino">O diretório local onde os arquivos serão armazenados.</param>
        /// <param name="credencialRede">Credenciais para o servidor.</param>
        /// <param name="arquivosRemotos">A lista de arquivos no servidor.</param>
        /// <returns>A lista de arquivos locais salvos.</returns>
        public static IEnumerable<string> TransferirArquivos(string diretorioDestino, NetworkCredential credencialRede, params Uri[] arquivosRemotos)
        {
            List<string> arquivosLocais = new List<string>();

            try
            {
                using (WebClient ftpClient = new WebClient())
                {
                    ftpClient.Credentials = credencialRede;
                    ftpClient.Proxy = null;

                    foreach (Uri arquivoRemoto in arquivosRemotos)
                    {
                        string nomeArquivoLocal = WebUtility.UrlDecode(arquivoRemoto.Segments.Last());

                        string arquivoLocal = Path.Combine(diretorioDestino, nomeArquivoLocal);

                        if (!Directory.Exists(Path.GetDirectoryName(arquivoLocal)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(arquivoLocal));
                        }

                        ftpClient.DownloadFile(arquivoRemoto, arquivoLocal);

                        arquivosLocais.Add(arquivoLocal);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(Texts.ErrorDuringFtpCommunication3.With(ex.Message), ex);
            }

            return arquivosLocais;
        }

        private static Uri FixUriIfVsFtpd(Uri diretorioServidor, NetworkCredential credencialRede)
        {
            try
            {
                // vsFTPd (*nix?) requer uma barra extra depois do nome do host ou ip do servidor, ex: ftp://nome.servidor//primeirodiretorio/segundo/arquivo
                // http://stackoverflow.com/a/7988779
                // Essa barra extra não é necessária em outros casos (warftpd ou iis ftpd no Windows, por ex.)
                Uri raizServidor = new Uri("ftp://{0}{1}{2}/".With(diretorioServidor.UserInfo, !string.IsNullOrWhiteSpace(diretorioServidor.UserInfo) ? "@" : string.Empty, diretorioServidor.Authority));
                FtpWebRequest pwdRequest = (FtpWebRequest)FtpWebRequest.Create(raizServidor);
                pwdRequest.Credentials = credencialRede;
                pwdRequest.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;
                pwdRequest.UsePassive = true;
                pwdRequest.Proxy = null;

                using (FtpWebResponse pwdResponse = (FtpWebResponse)pwdRequest.GetResponse())
                {
                    if (pwdResponse.BannerMessage.Contains("vsFTPd"))
                    {
                        diretorioServidor = new Uri("ftp://{0}{1}{2}/{3}".With(diretorioServidor.UserInfo, !string.IsNullOrWhiteSpace(diretorioServidor.UserInfo) ? "@" : string.Empty, diretorioServidor.Authority, diretorioServidor.AbsolutePath));
                    }
                }

                return diretorioServidor;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(Texts.ErrorDuringFtpCommunication1.With(ex.Message), ex);
            }
        }

        private static bool EntryMatchesMask(string entryName, string mask)
        {
            // Servidores FTP não devem aceitar coringas em nomes de arquivo ou diretório, mesmo para listagem de arquivos;
            // https://tools.ietf.org/html/rfc3659#page-6 2.2.2 Wildcarding
            // Valida nome de arquivo de forma simples (suficiente até agora)
            var parts = mask.Split('*');

            if (parts.Length > 0 && !entryName.StartsWith(parts[0], StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (parts.Length > 1 && !entryName.EndsWith(parts[parts.Length - 1], StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        [SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength")]
        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        private static ArquivoFtp Parse(Uri arquivoRemoto, bool isWindowsFtp, string linha, string[] componentes, string nome)
        {
            DateTime data = DateTime.MinValue;

            string tmpData;

            if (isWindowsFtp)
            {
                tmpData = string.Concat(DateTime.Now.Year.ToString(CultureInfo.InvariantCulture), "-", linha.Substring(linha.LastIndexOf('.') + 1, 2), "-", linha.Substring(linha.LastIndexOf('.') + 3, 2));
            }
            else
            {
                tmpData = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture) + linha.Substring(linha.IndexOf(nome, StringComparison.OrdinalIgnoreCase) - 13, 12);
            }

            if (!DateTime.TryParse(tmpData, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out data))
            {
                // TODO: retornar null?
                data = new DateTime(9999, 12, 31); // SGPFtpHelper.cs linha 238
            }

            string tmpTamanho;

            // era assim no legado.
            if (componentes[componentes.Length - 4] == string.Empty)
            {
                if (isWindowsFtp)
                {
                    tmpTamanho = componentes[componentes.Length - 2];
                }
                else
                {
                    tmpTamanho = componentes[componentes.Length - 6];
                }
            }
            else
            {
                if (string.Empty == componentes[componentes.Length - 3])
                {
                    // Caso onde não tem o horário
                    // -rw-r--r--    1 0        0               0 Oct 17  2008 sysmon.log
                    //                                                  ^^
                    tmpTamanho = componentes[componentes.Length - 6];
                }
                else
                {
                    // Caso onde tem o horário
                    // -rw-rw-r--    1 250      200          4418 Aug 29 09:00 ssmZRm8cG.Z
                    tmpTamanho = componentes[componentes.Length - 5];
                }
            }

            var isDiretorio = linha[0] == 'd';

            if (isDiretorio)
            {
                // Caso seja um diretório, não precisamos do tamanho (o tamanho é o tamanho do inode?)
                // drwxrwx---    2 250      200          4096 Feb 19  2012 spi28.171056776
                tmpTamanho = "0";
            }

            return new ArquivoFtp { NomeArquivo = nome, DataArquivo = data, Uri = new Uri(arquivoRemoto, nome), IsDiretorio = isDiretorio, TamanhoArquivo = int.Parse(tmpTamanho, CultureInfo.InvariantCulture) };
        }
    }
}
