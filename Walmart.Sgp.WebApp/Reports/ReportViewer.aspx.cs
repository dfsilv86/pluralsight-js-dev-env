using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace Walmart.Sgp.WebApp.Reports
{
    /// <summary>
    /// Página de visualização de relatórios do SSRS
    /// </summary>
    public partial class ReportViewer : Page
    {
        private const string IDUsuario = "IDUsuario";
        private const string UsuarioCorrente = "UsuarioCorrente";
        private const string CultureCode = "cultureCode";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var appSettings = ConfigurationManager.AppSettings;

                SetViewerProperties(appSettings);
                SetReportCredentials(appSettings);

                List<ReportParameter> parameters = SetReportParameters();
                viewer.ServerReport.SetParameters(parameters);
                viewer.ServerReport.Refresh();
            }
        }

        //// BEGIN HACK: http://www.rajbandi.net/fixing-ssrs-report-viewer-control-date-picker-in-google-chrome/
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            DatePickers.Value = GetDateParameters();
        }

        private string GetDateParameters()
        {
            var parameters = new List<string>();

            foreach (ReportParameterInfo info in viewer.ServerReport.GetParameters())
            {
                if (info.DataType == ParameterDataType.DateTime)
                {
                    parameters.Add(string.Format("[{0}]", info.Prompt));
                }
            }

            return string.Join(",", parameters);
        }
        //// END HACK.

        private void SetReportCredentials(NameValueCollection appSettings)
        {
            viewer.ServerReport.ReportServerCredentials = new ReportViewerCredentials(appSettings["SGP:ReportServer:User"], appSettings["SGP:ReportServer:Password"], appSettings["SGP:ReportServer:Domain"]);
        }

        private void SetViewerProperties(NameValueCollection appSettings)
        {
            var currentReport = this.Request.QueryString["currentReport"];

            viewer.ProcessingMode = ProcessingMode.Remote;
            viewer.ServerReport.ReportServerUrl = new Uri(appSettings["SGP:ReportServer:Url"]);
            viewer.ServerReport.ReportPath = appSettings["SGP:ReportServer:Path"] + currentReport;
            viewer.Page.Culture = appSettings["SGP:ReportServer:Culture"];
            viewer.ServerReport.DisplayName = string.Format("{0}_{1:yyyyMMdd}_{1:HHmmss}", currentReport.Substring(3), DateTime.Now);
            viewer.KeepSessionAlive = true;
        }

        private List<ReportParameter> SetReportParameters()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            var reportParameters = viewer.ServerReport.GetParameters().ToList();

            if (reportParameters.Exists(o => o.Name == UsuarioCorrente))
            {
                parameters.Add(new ReportParameter(UsuarioCorrente, this.Request.QueryString["idCurrentUser"]));
            }

            if (reportParameters.Exists(o => o.Name == IDUsuario))
            {
                parameters.Add(new ReportParameter(IDUsuario, this.Request.QueryString["idCurrentUser"]));
            }

            if (reportParameters.Exists(o => o.Name == CultureCode))
            {
                parameters.Add(new ReportParameter(CultureCode, this.Request.QueryString["cultureCode"]));
            }

            return parameters;
        }        
    }
}