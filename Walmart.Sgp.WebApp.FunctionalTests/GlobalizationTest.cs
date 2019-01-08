using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsQuery;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.WebApp.FunctionalTests
{
    [TestFixture]
    [Category("WebApp")]
    [Category("Globalization")]
    public class GlobalizationTest
    {
        #region Fields
        private static readonly Regex s_normalizeFieldNameRegex = new Regex("-(.)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
#if CI
        private static readonly string s_webAppDir = @"c:\TeamCity\buildAgent\work\walmart-sgp-reescrita\src\Walmart.Sgp.WebApp\Scripts\App";
#else
        private static readonly string s_webAppDir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\Walmart.Sgp.WebApp\Scripts\App";
#endif
        #endregion

        #region Methods
        [Test]
        public void FormFields_Title_UsingGlobalizationKey()
        {
            var formFieldsWithoutGlobalizationKey = new Dictionary<string, IList<string>>();
            var notGlobalizadedFormFieldsFiles = new List<string>();

            ForEachHtmlFile((fileName, html) =>
            {
                var formFields = html["form-field"];

                foreach(var ff in formFields)
                {
                    if (ff.HasAttribute("title"))
                    {
                        var title = ff.GetAttribute("title");

                        if (title != "" && !title.Contains("|translate"))
                        {
                            notGlobalizadedFormFieldsFiles.Add(fileName);
                            break;
                        }
                    }
                }
            });

            Assert.AreEqual(
                            0,
                            notGlobalizadedFormFieldsFiles.Count,
                            "As seguintes views HTML possuem form-field com a propriedade title sem utilizar uma chave de globalização: {0}", String.Join(", ", notGlobalizadedFormFieldsFiles));
        }

        [Test]
        public void RequiredHtmlFields_Globalization_Globalized()
        {
            var notGlobalizadedFields = new Dictionary<string, IList<string>>();

            ForEachHtmlFile((fileName, html) =>
            {
                var requiredFields = html["[required]"];

                foreach (var rf in requiredFields)
                {
                    var isFieldNotGlobalized = false;

                    if (rf.HasAttribute("name-start"))
                    {
                        AddToNotGlobalizedIfNeeded(fileName, rf.GetAttribute("name-start"), notGlobalizadedFields);
                        isFieldNotGlobalized = true;
                    }

                    if (rf.HasAttribute("name-end"))
                    {
                        AddToNotGlobalizedIfNeeded(fileName, rf.GetAttribute("name-end"), notGlobalizadedFields);
                        isFieldNotGlobalized = true;
                    }

                    if (!isFieldNotGlobalized)
                    {
                        var fieldName = rf.Attributes["name"] ?? rf.NodeName.ToLowerInvariant();

                        AddToNotGlobalizedIfNeeded(fileName, fieldName, notGlobalizadedFields);
                    }
                }
            });

            Assert.AreEqual(
                            0,
                            notGlobalizadedFields.Count,
                            "Os seguintes campos são requeridos nas views HTML, mas não possuem chave de globalização no Texts.resx: {0}", BuildRequiredFieldsMessage(notGlobalizadedFields));
        }

        private void AddToNotGlobalizedIfNeeded(string fileName, string fieldName, Dictionary<string, IList<string>> notGlobalizadedFields)
        {
            // Ignora campos com índice ou com nome dinâmico
            if (fieldName.Contains("[") || fieldName.Contains("{{"))
            {
                return;
            }

            var fieldNameForTextsKey = s_normalizeFieldNameRegex.Replace(fieldName, (m) =>
            {
                return m.Groups[1].Value.ToUpperInvariant();
            });

            if (GlobalizationHelper.GetText(fieldNameForTextsKey, false) == null)
            {
                if (!notGlobalizadedFields.ContainsKey(fileName))
                {
                    notGlobalizadedFields.Add(fileName, new List<string>());
                }

                notGlobalizadedFields[fileName].Add("{0}({1})".With(fieldName, fieldNameForTextsKey));
            }
        }

        private string BuildRequiredFieldsMessage(Dictionary<string, IList<string>> notGlobalizadedFields)
        {
            return String.Join(@"   |   ", notGlobalizadedFields.Select(n => "{0}: {1} ".With(n.Key, String.Join(", ", n.Value))));
        }
        #endregion

        #region Helpers
        private static void ForEachHtmlFile(Action<string, CQ> assertHtml)
        {
            var htmlFiles = Directory.GetFiles(s_webAppDir, "*.html", SearchOption.AllDirectories);
            Assert.AreNotEqual(0, htmlFiles.Length);

            foreach (var hf in htmlFiles)
            {
                var fileName = Path.GetFileName(hf);
                CQ html = File.ReadAllText(hf);

                assertHtml(fileName, html);
            }
        }
        #endregion
    }
}
