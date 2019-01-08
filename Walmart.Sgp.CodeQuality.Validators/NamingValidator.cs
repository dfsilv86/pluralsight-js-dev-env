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
    [TestFixture]
    [Category("CodeQuality")]
    [Category("CodeQuality.Naming")]
    public class NamingValidator
    {
        #region Fields
        private static Regex s_validEntityNameRegex = new Regex("(^(?:(?!Model).)*$)|SugestaoPedidoModel", RegexOptions.Compiled);
        private static Regex s_validSpecNameRegex = new Regex("([A-Z].+)([A-Z].+)([A-Z].+)([A-Z].+)+Spec", RegexOptions.Compiled);
        private static Regex s_validPropertyNameRegex = new Regex("(^[a-z]+[a-z0-9]*$)|Original_vlEstoque|Original_qtdPackCompra", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Methods
        [Test]
        public void TestClasses_Names_RespectNamingPattern()
        {
            Validator.AssertForEachType(
                Validator.TestClasses,
                (@class) =>
                {
                    var className = @class.Name;
                    return className.EndsWith("Test") & !className.Contains("_");
                },
                "http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/testes-unitarios");
        }

        [Test]
        public void TestMethods_Names_RespectNamingPattern()
        {
            Validator.AssertForEachMethod(
                Validator.TestClasses,
                Validator.GetTestMethods,
                (testMethod) =>
                {
                    return testMethod.Name.Split('_').Length == 3 && !testMethod.Name.Contains("__");
                },
                "http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/testes-unitarios");
        }

        [Test]
        public void EntityClasses_Names_RespectNamingPattern()
        {
            Validator.AssertForEachTypeName(
                Validator.DomainEntities,
                s_validEntityNameRegex,
                "http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/diretrizes-gerais-de-codificacao#nome-de-entidades-e-classes");
        }

        [Test]
        public void DomainMethods_Names_DoesNotStartsWithVerificar()
        {
            Validator.AssertForEachMethod(
                Validator.DomainClasses,
                Validator.GetPublicMethods,
                (domainMethod) =>
                {
                    var methodName = domainMethod.Name;

                    return !methodName.StartsWith("Verificar") && !methodName.StartsWith("Check");
                },
                "http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/Nomenclatura#quanto-ao-nome-de-m%C3%A9todos");
        }

        [Test]
        public void DomainMethods_Names_DoesNotUseUITerms()
        {
            Validator.AssertForEachMethod(
                Validator.DomainClasses,
                Validator.GetAllMethods,
                (domainMethod) =>
                {
                    var methodName = domainMethod.Name;

                    return !methodName.EndsWith("Tela") && !methodName.EndsWith("VM");
                },
                "Não utilize termos de UI no domínio: http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/Nomenclatura#quanto-ao-nome-de-m%C3%A9todos");
        }

        [Test]
        public void Properties_Names_RespectNamingPattern()
        {
            Validator.AssertForEachPropertyName(
                Validator.DomainClasses,
                s_validPropertyNameRegex,
                "http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/Nomenclatura#quanto-ao-nome-de-propriedades");
        }

        [Test]
        public void SpecClasses_Names_RespectNamingPattern()
        {
            Validator.AssertForEachTypeName(
                Validator.SpecClasses,
                s_validSpecNameRegex,
                "http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/Nomenclatura#quanto-%C3%A0s-specs-specication-pattern");
        }

        [Test]
        public void AllFiles_Typo_FixTheTypo()
        {
            var regex = new Regex("(cagetory)");

            Validator.AssertForEachFile(
                (fileName, content) =>
                {
                    var match = regex.Match(content);

                    if(match.Success)
                    {
                        return new ValidationResult(false, match.Groups[1].Value);
                    }

                    return true;
                },
                "Corrigja o typo: l");
        }
        #endregion
    }
}
