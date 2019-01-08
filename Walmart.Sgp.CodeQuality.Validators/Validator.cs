using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.UnitTests.Acessos;
using System.Reflection;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using NUnit.Framework;
using System.IO;
using CsQuery;

namespace Walmart.Sgp.CodeQuality.Validators
{
    public static class Validator
    {
        #region Fields
        private static readonly Regex s_removeCommentsRegex = new Regex("//.*$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        private static readonly Regex s_removeSuppressValidatorRegex = new Regex(".+//\\ssuppress-validator", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        private static Dictionary<string, string> s_filesCache = new Dictionary<string, string>();
#if CI
        public static readonly string DomainDir = AppDomain.CurrentDomain.BaseDirectory + @"\src\Walmart.Sgp.Domain";
        public static readonly string FrameworkDir = AppDomain.CurrentDomain.BaseDirectory + @"\src\Walmart.Sgp.Infrastructure.Framework";
        public static readonly string DataDir = AppDomain.CurrentDomain.BaseDirectory + @"\src\Walmart.Sgp.Infrastructure.Data";
        public static readonly string WebAppDir = AppDomain.CurrentDomain.BaseDirectory + @"\src\Walmart.Sgp.WebApp\Scripts\App";
        public static readonly string WebCommonDir = AppDomain.CurrentDomain.BaseDirectory + @"\src\Walmart.Sgp.WebApp\Scripts\Common";
        public static readonly string WebApiDir = AppDomain.CurrentDomain.BaseDirectory + @"\src\Walmart.Sgp.WebApi\Controllers";
#else
        public static readonly string DomainDir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\Walmart.Sgp.Domain";
        public static readonly string FrameworkDir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\Walmart.Sgp.Infrastructure.Framework";
        public static readonly string DataDir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\Walmart.Sgp.Infrastructure.Data";
        public static readonly string WebAppDir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\Walmart.Sgp.WebApp\Scripts\App";
        public static readonly string WebCommonDir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\Walmart.Sgp.WebApp\Scripts\Common";
        public static readonly string WebApiDir = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\Walmart.Sgp.WebApi\Controllers";
#endif
        #endregion

        #region Constructor
        static Validator()
        {
            var domainTestAssembly = typeof(UsuarioTest).Assembly;
            TestClasses = domainTestAssembly.GetTypes().Where(t => t.GetCustomAttributes<TestFixtureAttribute>().Any()).ToArray();

            var domainAssembly = typeof(Usuario).Assembly;
            DomainClasses = domainAssembly.GetTypes().Where(t => t.IsClass).ToArray();
            DomainServiceInterfaces = domainAssembly.GetTypes().Where(t => t.IsInterface && t.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase)).ToArray();

            DomainServiceClasses = DomainClasses.Where(t => t.Name.EndsWith("Service")).ToArray();
            DomainEntities = DomainClasses.Where(t => typeof(IEntity).IsAssignableFrom(t)).ToArray();
            SpecClasses = DomainClasses.Where(t => !t.IsAbstract && t.GetInterface("ISpec`1") != null).ToArray();
            DataGatewayInterfaces = domainAssembly.GetTypes().Where(t => t.IsInterface && t.Name.EndsWith("Gateway", StringComparison.OrdinalIgnoreCase)).ToArray();
        }
        #endregion

        #region Properties
        public static Type[] TestClasses { get; private set; }
        public static Type[] DomainClasses { get; private set; }
        public static Type[] DomainServiceClasses { get; private set; }
        public static Type[] DomainServiceInterfaces { get; private set; }
        public static Type[] DomainEntities { get; private set; }
        public static Type[] SpecClasses { get; private set; }
        public static Type[] DataGatewayInterfaces { get; private set; }
        #endregion

        #region Methods
        public static MethodInfo[] GetTestMethods(Type testClass)
        {
            return testClass.GetMethods().Where(t => t.GetCustomAttributes<TestAttribute>().Any()).ToArray();
        }

        public static MethodInfo[] GetPublicMethods(Type type)
        {
            return type.GetMethods();
        }

        public static MethodInfo[] GetAllMethods(Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static PropertyInfo[] GetPublicProperties(Type type)
        {
            if (type.Name.Contains("__AnonymousType"))
            {
                return new PropertyInfo[0];
            }

            return type.GetProperties();
        }

        public static void AssertForEachTypeName(Type[] types, Regex isValidClassNameRegex, string helpLink)
        {
            AssertForEachType(
               types,
               (@class) =>
               {
                   return isValidClassNameRegex.IsMatch(@class.Name);
               },
               helpLink);
        }

        public static void AssertForEachPropertyName(Type[] types, Regex isValidPropertyNameRegex, string helpLink)
        {
            AssertForEachProperty(
               types,
               GetPublicProperties,
               (p) =>
               {
                   return isValidPropertyNameRegex.IsMatch(p.Name);
               },
               helpLink);
        }

        public static void AssertForEachType(Type[] types, Func<Type, ValidationResult> isClassValid, string helpLink)
        {
            var wrongTypes = new List<Type>();

            foreach (var type in types)
            {
                if (!isClassValid(type))
                {
                    wrongTypes.Add(type);
                }
            }

            if (wrongTypes.Any())
            {
                Assert.Fail("Os tipos abaixo violam a qualidade de código do projeto ({0}): \n{1}"
                   .With(
                   helpLink,
                   String.Join("\n", wrongTypes.Select(c => c.Name))));
            }
        }

        public static void AssertForEachMethod(Type[] types, Func<Type, MethodInfo[]> getMethods, Func<MethodInfo, ValidationResult> isMethodValid, string helpLink)
        {
            var wrongMethods = new List<MethodInfo>();

            foreach (var type in types)
            {
                var typeName = type.Name;

                foreach (var method in getMethods(type))
                {
                    if (!isMethodValid(method))
                    {
                        wrongMethods.Add(method);
                    }
                }
            }

            if (wrongMethods.Any())
            {
                Assert.Fail("Os métodos abaixo violam a qualidade de código do projeto ({0}): \n{1}"
                    .With(
                        helpLink,
                        String.Join("\n", wrongMethods.Select(c => "{0}.{1}".With(c.DeclaringType.Name, c.Name)))));
            }
        }

        public static void AssertForEachProperty(Type[] types, Func<Type, PropertyInfo[]> getProperties, Func<PropertyInfo, ValidationResult> isPropertyValid, string helpLink)
        {
            var wrongProperties = new List<PropertyInfo>();

            foreach (var type in types)
            {
                var typeName = type.Name;

                foreach (var property in getProperties(type))
                {
                    if (!isPropertyValid(property))
                    {
                        wrongProperties.Add(property);
                    }
                }
            }

            if (wrongProperties.Any())
            {
                Assert.Fail("As propriedades abaixo violam a qualidade de código do projeto ({0}): \n{1}"
                    .With(
                        helpLink,
                        String.Join("\n", wrongProperties.Select(c => "{0}.{1}".With(c.DeclaringType.Name, c.Name)))));
            }
        }

        public static void ForEachHtmlFile(Action<string, CQ> assertHtml)
        {
            var htmlFiles = Directory.GetFiles(Validator.WebAppDir, "*.html", SearchOption.AllDirectories);
            Assert.AreNotEqual(0, htmlFiles.Length);

            foreach (var hf in htmlFiles)
            {
                var fileName = Path.GetFileName(hf);
                CQ html = ReadFileFromCache(hf);

                assertHtml(fileName, html);
            }
        }

        public static void AssertForEachHtmlFile(Func<string, CQ, ValidationResult> isHtmlValid, string helpLink)
        {
            AssertForEachFile(
              new string[] { WebCommonDir, WebAppDir },
              "*.html",
              (fileName, content) =>
              {
                  CQ html = content;
                  return isHtmlValid(fileName, html);
              },
              helpLink);
        }

        public static void AssertForEachJsFile(Func<string, string, ValidationResult> isJsValid, string helpLink)
        {
            AssertForEachFile(
                new string[] { WebCommonDir, WebAppDir },
                "*.js",
                isJsValid,
                helpLink);
        }

        public static void AssertForEachCsFile(Func<string, string, ValidationResult> isCsValid, string helpLink)
        {
            AssertForEachFile(
                new string[] { DomainDir, FrameworkDir },
                "*.cs",
                isCsValid,
                helpLink);
        }

        public static void AssertForEachDomainFile(Func<string, string, ValidationResult> isCsValid, string helpLink)
        {
            AssertForEachFile(
                new string[] { DomainDir },
                "*.cs",
                isCsValid,
                helpLink);
        }

        public static void AssertForEachSqlFile(Func<string, string, ValidationResult> isSqlValid, string helpLink)
        {
            AssertForEachFile(
                new string[] { DataDir },
                "*.sql",
                isSqlValid,
                helpLink,
                new string[] { "Databases\\Changelog" });
        }

        public static void AssertForEachFile(Func<string, string, ValidationResult> isFileValid, string helpLink)
        {
            AssertForEachFile(
                new string[] { DataDir, WebCommonDir, WebAppDir, DomainDir, FrameworkDir },
                "*.html;*.js;*.sql;*.cs",
                isFileValid,
                helpLink,
                new string[] { "Databases\\Changelog" });
        }

        public static void AssertForEachWebApiControllerFile(Func<string, string, ValidationResult> isValid, string helpLink)
        {
            AssertForEachFile(
                new string[] { WebApiDir },
                "*Controller.cs",
                isValid,
                helpLink);
        }

        public static string ReadWebAppFile(string relativeFilePath)
        {
            return ReadFileFromCache(Path.Combine(WebAppDir, relativeFilePath));
        }

        public static bool ExistsDataFile(string relativeFilePath)
        {
            return File.Exists(Path.Combine(DataDir, relativeFilePath));
        }

        public static string ReadDataFile(string relativeFilePath)
        {
            return ReadFileFromCache(Path.Combine(DataDir, relativeFilePath));
        }

        private static void AssertForEachFile(string[] rootDirs, string fileSearchPattern, Func<string, string, ValidationResult> isFileValid, string helpLink, string[] ignoreDirs = null)
        {
            var wrongFiles = new Dictionary<string, ValidationResult>();
            var files = new List<string>();
            var fileSearchPatterns = fileSearchPattern.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var rootDir in rootDirs)
            {
                foreach (var filePattern in fileSearchPatterns)
                {
                    var dirFiles = Directory.GetFiles(rootDir, filePattern, SearchOption.AllDirectories);

                    if (ignoreDirs != null)
                    {
                        dirFiles = dirFiles.Where(df => !ignoreDirs.Any(i => df.Contains(i))).ToArray();
                    }

                    if (fileSearchPatterns.Length == 0)
                    {
                        Assert.AreNotEqual(0, dirFiles.Length);
                    }

                    files.AddRange(dirFiles);
                }
            }

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                ValidationResult result = isFileValid(fileName, ReadFileFromCache(file));

                if (!result.Success && !wrongFiles.ContainsKey(fileName))
                {
                    wrongFiles.Add(fileName, result);
                }
            }


            if (wrongFiles.Any())
            {
                Assert.Fail("Os arquivos {0} abaixo violam a qualidade de código do projeto ({1}): \n{2}"
                    .With(
                        fileSearchPattern,
                        helpLink,
                        String.Join("\n", wrongFiles.Select(h => "{0}. {1}".With(h.Key, h.Value.Message)))));
            }
        }

        private static string ReadFileFromCache(string fileName)
        {
            if (!s_filesCache.ContainsKey(fileName))
            {
                var fileContent = File.ReadAllText(fileName);
                fileContent = s_removeSuppressValidatorRegex.Replace(fileContent, string.Empty);
                fileContent = s_removeCommentsRegex.Replace(fileContent, string.Empty);
                s_filesCache.Add(fileName, fileContent);
            }

            return s_filesCache[fileName];
        }
        #endregion

    }
}
