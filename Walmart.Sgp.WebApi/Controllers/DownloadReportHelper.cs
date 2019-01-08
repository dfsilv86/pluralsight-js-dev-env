using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Logging;
using Walmart.Sgp.Infrastructure.IO.FileVault;
using Walmart.Sgp.Infrastructure.Web.Extensions;

namespace Walmart.Sgp.WebApi.Controllers
{
    public static class DownloadReportHelper
    {
        public static HttpResponseMessage DownloadExcel(HttpRequestMessage request, string reportName, string fileName, Dictionary<string, object> parameters, IFileVaultService fileVaultService)
        {
            if (!Path.HasExtension(fileName))
            {
                fileName = fileName + ".xls";
            }

            var appSettings = ConfigurationManager.AppSettings;

            NetworkCredential networkCredential = new NetworkCredential(appSettings["SGP:ReportServer:User"], appSettings["SGP:ReportServer:Password"]);

            WebClient webClient = new WebClient();
            webClient.Credentials = networkCredential;

            string reportUrl = string.Format("{0}{1}{2}rs:Command=Render&rs:Format=Excel", appSettings["SGP:ReportServer:Url"], reportName.ToString(), BuildParameters(parameters));
            
            try
            {
                var downloadedFile = webClient.OpenRead(reportUrl);

                return downloadedFile.WithFileVault(fileName, fileVaultService);
            }
            catch (WebException ex)
            {
                LogService.Error(Texts.ReportServerCallError, reportUrl, ex.Message);

                throw new WebException(Texts.ReportServerUnavailable, ex);
            }
        }

        private static string BuildParameters(Dictionary<string, object> parameters)
        {
            string reportParameters = string.Empty;

            foreach (var parameter in parameters)
            {
                if (parameter.Value == null)
                {
                    reportParameters += string.Format("&{0}:IsNull=True", parameter.Key, parameter.Value);
                }
                else
                {
                    reportParameters += string.Format("&{0}={1}", parameter.Key, parameter.Value);
                }
            }

            reportParameters += "&";

            return reportParameters;
        }
    }
}