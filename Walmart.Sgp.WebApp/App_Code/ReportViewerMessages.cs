using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Reporting.WebForms;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

public class ReportViewerMessages : IReportViewerMessages, IReportViewerMessages2
{
    #region Properties
    public string BackButtonToolTip
    {
        get
        {
            return Texts.Back;
        }
    }

    public string ChangeCredentialsText
    {
        get
        {
            return null;
        }
    }

    public string ChangeCredentialsToolTip
    {
        get
        {
            return null;
        }
    }

    public string CurrentPageTextBoxToolTip
    {
        get
        {
            return Texts.CurrentPage;
        }
    }

    public string DocumentMap
    {
        get
        {
            return null;
        }
    }

    public string DocumentMapButtonToolTip
    {
        get
        {
            return null;
        }
    }

    public string ExportButtonText
    {
        get
        {
            return null;
        }
    }

    public string ExportButtonToolTip
    {
        get
        {
            return Texts.Export;
        }
    }

    public string ExportFormatsToolTip
    {
        get
        {
            return null;
        }
    }

    public string FalseValueText
    {
        get
        {
            return null;
        }
    }

    public string FindButtonText
    {
        get
        {
            return Texts.Find;
        }
    }

    public string FindButtonToolTip
    {
        get
        {
            return null;
        }
    }

    public string FindNextButtonText
    {
        get
        {
            return Texts.Next;
        }
    }

    public string FindNextButtonToolTip
    {
        get
        {
            return null;
        }
    }

    public string FirstPageButtonToolTip
    {
        get
        {
            return Texts.FirstPage;
        }
    }

    public string InvalidPageNumber
    {
        get
        {
            return Texts.InvalidPageNumber;
        }
    }

    public string LastPageButtonToolTip
    {
        get
        {
            return Texts.LastPage;
        }
    }

    public string NextPageButtonToolTip
    {
        get
        {
            return Texts.NextPage;
        }
    }

    public string NoMoreMatches
    {
        get
        {
            return null;
        }
    }

    public string NullCheckBoxText
    {
        get
        {
            return Texts.DoNotInformValue;
        }
    }

    public string NullValueText
    {
        get
        {
            return null;
        }
    }

    public string PageOf
    {
        get
        {
            return Texts.Of.ToLowerInvariant();
        }
    }

    public string ParameterAreaButtonToolTip
    {
        get
        {
            return Texts.ShowHideParameters;
        }
    }

    public string PasswordPrompt
    {
        get
        {
            return null;
        }
    }

    public string PreviousPageButtonToolTip
    {
        get
        {
            return Texts.PreviousPage;
        }
    }

    public string PrintButtonToolTip
    {
        get
        {
            return Texts.Print;
        }
    }

    public string ProgressText
    {
        get
        {
            return Texts.Loading;
        }
    }

    public string RefreshButtonToolTip
    {
        get
        {
            return Texts.Refresh;
        }
    }

    public string SearchTextBoxToolTip
    {
        get
        {
            return Texts.SearchTextOnReport;
        }
    }

    public string SelectAValue
    {
        get
        {
            return Texts.SelectAValue;
        }
    }

    public string SelectAll
    {
        get
        {
            return Texts.SelectAll;
        }
    }

    public string SelectFormat
    {
        get
        {
            return null;
        }
    }

    public string TextNotFound
    {
        get
        {
            return Texts.TextNotFound;
        }
    }

    public string TodayIs
    {
        get
        {
            return null;
        }
    }

    public string TrueValueText
    {
        get
        {
            return null;
        }
    }

    public string UserNamePrompt
    {
        get
        {
            return null;
        }
    }

    public string ViewReportButtonText
    {
        get
        {
            return Texts.ViewReport;
        }
    }

    public string ZoomControlToolTip
    {
        get
        {
            return Texts.Zoom;
        }
    }

    public string ZoomToPageWidth
    {
        get
        {
            return Texts.PageWidth;
        }
    }

    public string ZoomToWholePage
    {
        get
        {
            return Texts.WholePage;
        }
    }

    public string ClientNoScript
    {
        get
        {
            return null;
        }
    }

    public string ClientPrintControlLoadFailed
    {
        get
        {
            return null;
        }
    }

    public string ParameterDropDownToolTip
    {
        get
        {
            return null;
        }
    }
    #endregion

    #region Methods
    public string ParameterMissingSelectionError(string parameterPrompt)
    {
        return null;
    }

    public string ParameterMissingValueError(string parameterPrompt)
    {
        return Texts.ParameterMissingValueError.With(GlobalizationHelper.GetText(parameterPrompt, false) ?? parameterPrompt);
    }

    public string CredentialMissingUserNameError(string dataSourcePrompt)
    {
        return null;
    }

    public string GetLocalizedNameForRenderingExtension(string format)
    {
        return null;
    }
    #endregion
}