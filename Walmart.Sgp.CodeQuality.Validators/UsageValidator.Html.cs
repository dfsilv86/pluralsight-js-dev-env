using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsQuery;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.UnitTests.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.CodeQuality.Validators
{
    public partial class UsageValidator
    {
        [Test]
        public void Html_TbodyColspan_ShouldRespectTheadCols()
        {
            Validator.AssertForEachHtmlFile(
            (fileName, html) =>
            {                
                var tables = html["table"];

                if(tables.Length > 0 && tables.Find("table").Length == 0 && tables.Find("thead").Length > 0)
                {
                    if (tables.Find("tbody tr td[ng-if],tbody tr td[ng-repeat]").Length > 0)
                    {
                        // Ignora os que possuem ng-if ou ng-repeat, pois seria "quase" impossível determinar a quantidade nessa situação.
                        return true;
                    }

                    float theadColumnsCount = tables.Find("thead tr th").Sum(td => td.HasAttribute("colspan") ? Convert.ToInt32(td.GetAttribute("colspan")) : 1);
                    float tbodyColumnsCount = tables.Find("tbody tr:last-child td").Sum(td => td.HasAttribute("colspan") ? Convert.ToInt32(td.GetAttribute("colspan")) : 1);
                    
                    // TBODY com ng-if.
                    var theadsCount = tables.Find("thead").Length;
                    var tbodiesCount = tables.Find("tbody").Length; 
                    
                    var totalTbodyColumnsCount = tbodyColumnsCount / (tbodiesCount / theadsCount);

                    var success = theadColumnsCount == totalTbodyColumnsCount;
                    var result = new ValidationResult(success);
                    result.Message = "O colspan correto seria: {0}.".With(theadColumnsCount);

                    return result;
                }

                return true;
            },
            "O colspan da linha do tbody está diferente da quantidade de colunas definidas em thead.");
        }

        [Test]
        public void Html_Buttons_ShouldHaveRightIcon()
        {
            Validator.AssertForEachHtmlFile(
            (fileName, content) =>
            {
                var buttons = content["button"];

                foreach(var b in buttons)
                {
                    var text = b.InnerText;
                    var html = b.InnerHTML;

                    if ((text.Contains("export") && !html.Contains("save-file"))
                     || (text.Contains("remove") && !html.Contains("glyphicon-remove"))
                     || (text.Contains("create") && !html.Contains("glyphicon-file"))
                     || (text.Contains("search") && !html.Contains("glyphicon-search"))
                     || (text.Contains("back") && !html.Contains("glyphicon-arrow-left"))
                     || (text.Contains("save") && !html.Contains("glyphicon-save")))
                    {
                        return new ValidationResult(false, b.OuterHTML);
                    }
                }

                return true;
            },
            "Botão deve ter um ícone que corresponde a sua ação.");
        }

        [Test]
        [Ignore("DZ-O enumFilter não é a mesma coisa que o fixedValues - pros valores novos ele meramente compõe a string de tradução de um Enum do C#. Pode ser usado, mas os usos antigos que agora possuem equivalente no fixedValues podem ser passados pro fixedValuesFilter.")]
        public void Html_EnumFilter_Deprecated()
        {
            Validator.AssertForEachHtmlFile(
            (fileName, html) =>
            {
                // TODO: já estava em uso nesses arquivos quando a regra foi criada.
                // Remover quando possível
                if ("extrato-produto.view.html; modal-item-lookup.html; modal-loja-lookup.html; pesquisa-loja.view.html; detalhe-parametro-fornecedor.view.html; modal-fornecedor-lookup.html; pesquisa-fornecedor.view.html; pesquisa-parametro-fornecedor.view.html; detalhe-inventario.view.html; custos-item.view.html; custos-itens-relacionados.view.html; detalhe-parametro-item.view.html; informacoes-cadastrais-item.view.html; pesquisa-parametro-item.view.html; pesquisa-sugestao-pedido.view.html; process-notification-area.view.html; manage-process.view.html; manage-processes.view.html; view-process-logs.view.html;".Contains(fileName))
                {
                    return true;
                }

                return !html.Html().Contains("|enum:'");
            },
            "O filter 'enum' está depreciado, no lugar dele utilize o enum fixedValue");
        }

        [Test]
        public void Html_CodePlusDescription_ShouldUseProfileFilter()
        {
            var regex = new Regex(@"\{\{.+cd.+\}\} - \{\{.+(nm|ds).+\}\}", RegexOptions.Compiled);

            Validator.AssertForEachHtmlFile(
            (fileName, content) =>
            {
                var html = content.Html();
                var match = regex.Match(html);
                
                if(match.Success)
                {
                    return new ValidationResult(false, match.Groups[0].Value);
                }                

                return true;
            },
            "Combinações de código + descrição deveriam utilizar o filter de profile correspondente");
        }
    }
}
