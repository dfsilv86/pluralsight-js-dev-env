using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.FileVault;

namespace Walmart.Sgp.Infrastructure.Web.Extensions
{
    /// <summary>
    /// File attachment extensions
    /// </summary>
    public static class FileAttachmentExtensions
    {
        /// <summary>
        /// Send the byte array content over HTTP current response.
        /// </summary>
        /// <param name="data">The content to be send.</param>
        /// <param name="fileName">The suggested file name to the HTTP client.</param>
        public static void SendOverHttp(this byte[] data, string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                fileName = Guid.NewGuid().ToString();
            }

            fileName = Path.GetFileName(fileName);

            var response = HttpContext.Current.Response;
            response.ContentType = MimeMapping.GetMimeMapping(fileName);
            response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
            response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
            response.BinaryWrite(data);
        }

        /// <summary>
        /// Armazena o arquivo no file vault e cria uma resposta HTTP que retorna headers de file vault que serão interceptados pelo file-vault.interceptor.js.
        /// </summary>
        /// <param name="content">O conteúdo a ser armazenado no file vault.</param>
        /// <param name="fileName">O nome do arquivo.</param>
        /// <param name="fileVaultService">O serviço de file vault.</param>
        /// <returns>A resposta http.</returns>
        public static HttpResponseMessage WithFileVault(this Stream content, string fileName, IFileVaultService fileVaultService)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            var inputFile = new IntermediateStreamFile(fileName, content);
            var ticket = fileVaultService.Store(inputFile);
            result.Headers.Add("x-file-vault-ticket-id", HttpUtility.UrlEncode(ticket.Id));
            result.Headers.Add("x-file-vault-ticket-created-date", ticket.CreatedDate.ToUniversalTime().ToString("o"));

            return result;
        }

        /// <summary>
        /// Cria uma resposta HTTP com o arquivo informado como conteúdo.
        /// </summary>
        /// <param name="content">O conteúdo do arquivo.</param>
        /// <param name="fileName">O nome do arquivo.</param>
        /// <returns>A resposta http.</returns>
        public static HttpResponseMessage AsAttachment(this Stream content, string fileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StreamContent(content);

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = fileName;

            result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName) ?? "application/octet-stream");
            result.Content.Headers.ContentType.CharSet = "x-user-defined";

            result.Headers.AddCookies(new CookieHeaderValue[] { new CookieHeaderValue("fileDownload", "true") });

            return result;
        }
    }
}
