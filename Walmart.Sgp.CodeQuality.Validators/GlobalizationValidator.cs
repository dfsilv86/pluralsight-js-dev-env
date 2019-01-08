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

namespace Walmart.Sgp.CodeQuality.Validators
{
    [TestFixture]
    [Category("CodeQuality")]
    [Category("CodeQuality.Globalization")]    
    public class GlobalizationValidator
    {
        #region Fields
        private static readonly Regex s_normalizeFieldNameRegex = new Regex("-(.)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex s_jsGetTextRegex = new Regex(@"globalization\.getText\('\S+'\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Methods
        [Test]
        public void FormFields_Title_UsingGlobalizationKey()
        {
            var formFieldsWithoutGlobalizationKey = new Dictionary<string, IList<string>>();
            var notGlobalizadedFormFieldsFiles = new List<string>();

            Validator.ForEachHtmlFile((fileName, html) =>
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

            Validator.ForEachHtmlFile((fileName, html) =>
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

        [Test]
        public void HtmlView_Texts_AllTextsShouldBeGlobalized()
        {
            var removeSpacesRegex = new Regex(@"\s*", RegexOptions.Compiled);
            Validator.AssertForEachHtmlFile(
            (fileName, dom) =>
            {
                var elements = dom["td,button"];

                foreach (var e in elements)
                {
                    var text = removeSpacesRegex.Replace(e.InnerText, "");

                    if (text.Length > 0 && !text.Contains("{{"))
                    {
                        return new ValidationResult(false, "O texto não globalizado foi: '{0}'".With(e.InnerText));
                    }
                }

                return true;
            },
            "Todos os textos devem estar globalizados no arquivo Texts.resx.");
        }

        [Test]
        public void Js_Globalization_StaticTextsUsingPropertiesDirectly()
        {
            Validator.AssertForEachJsFile(
            (fileName, js) =>
            {
                return !s_jsGetTextRegex.IsMatch(js);
            },
            "Se o valor da chave de globalização é estático, então utilize globalization.texts.nomeChaveDeGlobalizacao ao invés de globalization.getText('nomeChaveDeGlobalizacao')");
        }

        [Test]
        public void Js_Toast_MessagesShouldBeGlobalized()
        {
            var toastMessageNotGlobalizedRegex = new Regex("(toast|toastService)\\.(success|warning|error)\\s*\\(('|\")", RegexOptions.Compiled);
            Validator.AssertForEachJsFile(
            (fileName, js) =>
            {
                return !toastMessageNotGlobalizedRegex.IsMatch(js);
            },
            "Mensagens devem ser globalizadas via arquivo Texts.resx e utilizadas via globalization.texts.chaveDaMensagem.");
        }

        [Test]
        public void HtmlView_Translate_ShouldNotEndWithDot()
        {
            Validator.AssertForEachHtmlFile(
            (fileName, dom) =>
            {
                var html = dom.Html();

                return !html.Contains("|translate|capitalize}}.") && !html.Contains("|translate}}.");
            },
            "Não adicione nenhuma pontuação ao final de um texto globalizado, pois a pontuação também deve vir da globalização.");
        }

        [Test]
        public void Html_Globalization_FixedValueFilterShouldExists()
        {
            var sgpFixedValueFileContent = Validator.ReadWebAppFile("sgp.fixed-values.js");
            var regex = new Regex(@"\|fixedValue:\s*'([A-Z0-9]+)'", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Validator.AssertForEachHtmlFile(
            (fileName, dom) =>
            {
                var match = regex.Match(dom.Html());

                if(match.Success)
                {
                    var fixedValueName = match.Groups[1].Value;

                    if(!sgpFixedValueFileContent.Contains(fixedValueName))
                    {
                        return new ValidationResult(false, "FixedValue: {0}".With(fixedValueName));
                    }                    
                }

                return true;                
            },
            "O filtro fixedValue está utilizando um valor que não está definido no arqivo sgp.fixed-value.js");
        }


        [Test]
        public void Js_Globalization_TextsUsedNotExistsOnTextsResourcesFile()
        {
            var globalizationKeyRegex = new Regex(@"globalization\.texts(\.)([a-z0-9]+)(.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var globalizationGetTextRegex = new Regex("globalization\\.getText\\s*\\((\"|')([a-z0-9]+)(\"|')\\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            Validator.AssertForEachJsFile(
            (fileName, js) =>
            {
                var matches = new List<Match>(globalizationKeyRegex.Matches(js).Cast<Match>());
                matches.AddRange(globalizationGetTextRegex.Matches(js).Cast<Match>());

                foreach(Match match in matches)
                {
                    var key = match.Groups[2].Value;

                    if (GlobalizationHelper.GetText(key, false) == null)
                    {
                        return new ValidationResult(false, "Chave: {0}".With(key));
                    }
                }

                return true;
            },
            "Chave de globalização utilizada no javascript (globalization.texts.*), mas não existe no arquivo Texts.resx.");
        }


        [Test]
        public void Js_Globalization_TextsKeyShouldStartWithLowerCase()
        {
            var globalizationKeyRegex = new Regex(@"globalization\.texts\.([A-Z]([a-zA-Z0-9]*))", RegexOptions.Compiled);
            Validator.AssertForEachJsFile(
            (fileName, js) =>
            {
                var match = globalizationKeyRegex.Match(js);

                return new ValidationResult(!match.Success, "Chave: {0}".With(match.Groups[1].Value));
            },
            "Chave de globalização utilizada no javascript (globalization.texts.*) deve iniciar com uma letra minúscula.");
        }

        [Test]
        public void Domain_MustBeInformedFields_ShouldBeGlobalized()
        {
            var regex = new Regex(@"Assert\(new \{ (.+) \}", RegexOptions.Compiled);
            Validator.AssertForEachCsFile(
            (fileName, content) =>
            {
                var matches = regex.Matches(content);

                foreach(Match m in matches)
                {
                    var fields = m.Groups[1].Value
                        .Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(f => f.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries).First())
                        .Select(f => f.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last());
                    
                    foreach (var f in fields)
                    {
                        if (GlobalizationHelper.GetText(f, false) == null)
                        {
                            return new ValidationResult(false, "Chave: {0}".With(f));
                        }
                    }
                }

                return true;
            },
            "Chave de globalização utilizada em AllMustBeInformedSpec/AtLeastOneMustBeInformedSpec não globalizada no arquivo Texts.resx.");
        }
        #endregion

        #region Helpers
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
    }
}
