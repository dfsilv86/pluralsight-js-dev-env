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
    [Category("CodeQuality.Usage")]
    public partial class UsageValidator
    {
        [Test]
        public void SpecClasses_Target_ShouldNotBeAValueType()
        {
            Validator.AssertForEachType(
                Validator.SpecClasses,
                (@class) =>
                {
                    var specBaseType = GetSpecBaseType(@class);

                    return !(specBaseType.IsGenericType && specBaseType.GetGenericArguments().Any(a => a.IsValueType && !a.IsGenericType));
                },
                "http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/diretrizes-gerais-de-codificacao#specs-specification-pattern");
        }        

        [Test]
        public void DataGatewayInterfaces_Methods_ShouldNotRedefineFindAll()
        {
            Validator.AssertForEachMethod(
                Validator.DataGatewayInterfaces,
                Validator.GetPublicMethods,
                (m) =>
                {
                    return !(m.Name.Equals("ObterTodos") && m.GetParameters().Length == 0);
                },
                "Não crie um método chamado ObterTodos sem argumentos na interface de gateway, pois já existe o IDataGateway.FindAll para isso. Ao invés, herde de IDataGateay<T>");
        }

        [Test]
        public void DomainServiceInterfaces_MethodsEndWithPorFiltro_ShouldUseAFiltroNamedClass()
        {

            Validator.AssertForEachMethod(
                Validator.DomainServiceInterfaces,
                Validator.GetPublicMethods,
                (m) =>
                {
                    // TODO: ignorados os que já estavam errados quando foi criado o validador.
                    // Corrigir assim que possível.
                    if ("IFornecedorParametroService.PesquisarPorFiltro; INotaFiscalService.PesquisarUltimasEntradasPorFiltros; IGradeSugestaoService.PesquisarEstruturadoPorFiltro; IItemDetalheService.PesquisarPorFiltroTipoReabastecimento; IItemDetalheService.PesquisarPorFiltro".Contains("{0}.{1}".With(m.DeclaringType.Name, m.Name)))
                    {
                        return true;
                    }

                    return !m.Name.Contains("PorFiltro") || m.GetParameters().Any(p => p.ParameterType.Name.Contains("Filtro"));
                },
                "Crie uma classe de filtro para passar os argumentos, assim evita um refactoring em todas as assinatura relacionadas caso seja necessário adicionar/remover um argumento de filtro.");
        }

        [Test]
        public void DomainServiceInterfaces_Methods_ShouldNotRedefineMethods()
        {
            var domainServiceInterfaceMethods = typeof(IDomainService<>).GetMethods();
            var usosJustificados = "IParametroService;";

            Validator.AssertForEachMethod(
                Validator.DomainServiceInterfaces,
                Validator.GetPublicMethods,
                (m) =>
                {
                    if (usosJustificados.Contains(m.DeclaringType.Name))
                    {
                        return true;
                    }

                    return !(domainServiceInterfaceMethods.Any(i => i.Name.Equals(m.Name) && i.GetParameters().Length == m.GetParameters().Length));
                },
                "Não crie métodos definidos em IDomainService<T> na interface do serviço. Ao invés, herde de IDomainService<T>");
        }

        [Test]
        public void DomainService_FindAll_ShouldNotOrderByAfterMaterialization()
        {
            Validator.AssertForEachCsFile(
                (fileName, content) => 
                {
                    return !content.Contains("FindAll().OrderBy");
                },
                "Não utilize FindAll().OrderBy, pois ao final de FindAll os dados já foram materalizados em memória. Ao invés, utilize Paging.OrderBy como parâmetro de FindAll.");
        }

        [Test]
        public void Domain_DateTimeParse_ShouldNotParseDateTime()
        {
            Validator.AssertForEachDomainFile(
                (fileName, content) =>
                {
                    var ignored = fileName.Contains("SugestaoPedidoCDService");
                    return ignored || !content.Contains("DateTime.Parse");
                },
                "Não utilize DateTime.Parse no domínio. Realizar parse de dados é uma responsabilidade da camada de UI.");
        }

        [Test]
        public void DomainService_WithMainGateway_ShouldInheriteFromEntityDomainServiceBase()
        {
            Validator.AssertForEachCsFile(
                (fileName, content) =>
                {
                    return !(fileName.EndsWith("Service.cs") && content.Contains("m_mainGateway"));
                },
                "Se o seu serviço de domínio utiliza um gateway principal, então adicione a herança para EntityDomainServiceBase e remove o campo privado m_mainGateway.");
        }

        [Test]
        public void Classes_CommentedCodeWithoutTodo_CommentedCodeShouldBeRemoved()
        {
            var regex = new Regex("////");

            Validator.AssertForEachCsFile(
                (fileName, content) =>
                {
                    return !(regex.Matches(content).Count > 0 && !content.Contains("// TODO:"));
                },
                "Códigos comentados que não são mais utilizados devem ser removidos: http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/diretrizes-gerais-de-codificacao#geral");
        }

        [Test]
        public void Classes_PrivateFieldsOfGatewaysAndServices_ShuldBeReadonly()
        {
            var regex = new Regex(@"private I.+(Gateway|Service)\s(.+);");

            Validator.AssertForEachCsFile(
                (fileName, content) =>
                {
                   var match = regex.Match(content);

                   if(match.Success)
                   {
                       return new ValidationResult(false, match.Groups[2].Value);
                   }
                   else
                   {
                       return true;
                   }
                },
                "Campos que são configurados apenas no construtor devem ser marcados como readonly");
        }

        [Test]
        public void Domain_Html_DomainCodeShouldNotManipulateHtmlCode()
        {
            var regex = new Regex(@"\</.+\>");

            Validator.AssertForEachDomainFile(
                (fileName, content) =>
                {
                    var match = regex.Match(content);

                    if (match.Success)
                    {
                        return new ValidationResult(false, match.Groups[0].Value);
                    }
                    else
                    {
                        return true;
                    }                 
                },
                "Não manipule HTML no domínio, manipulação de HTML é uma questão deve que ser resolvida na UI.");
        }

        private Type GetSpecBaseType(Type type)
        {
            var baseType = type.BaseType;

            if (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(SpecBase<>))
                {
                    return baseType;
                }
                else
                {
                    return GetSpecBaseType(baseType);
                }
            }

            return type;
        }
    }
}
