using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        public void HtmlView_AngularJs_ShouldRespectAngularJsGuidelines()
        {
            Validator.AssertForEachHtmlFile(
            (fileName, html) => 
            {
                var ignored = fileName.Contains("pesquisa-sugestao-pedido-cd.view.html") && html["[ng-show],[ng-hide],[ng-controller]"].Length == 0;
                
                return html["[ng-show],[ng-hide],[ng-init],[ng-controller]"].Length == 0 || ignored;
            },
            "http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/diretrizes-gerais-de-codificacao#angularjs");
        }

        [Test]
        public void HtmlView_NgClickWithSearch_ShouldPassPageParameter()
        {
            Validator.AssertForEachHtmlFile(
            (fileName, html) =>
            {
                var ngClicks = html["[ng-click]"];

                return !ngClicks.Any(n => n.GetAttribute("ng-click").Equals("search()"));
            },
            "ng-click utilizando search deve passar o parâmetro de página: search(1)");
        }    

        [Test]
        public void Js_Timeout_ShouldNotBeUsed()
        {
            Validator.AssertForEachJsFile(
            (fileName, js) =>
            {
                return !js.Contains("$timeout.") && !js.Contains("$timeout(");
            },
            "Não utilize $timeout, na maioria dos casos existe uma solução melhor.");
        }
    }
}
