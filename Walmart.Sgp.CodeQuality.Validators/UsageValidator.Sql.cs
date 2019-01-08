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
        private static readonly Regex s_cteRegex = new Regex("WITH .+ AS", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex s_betweenWithDateRegex = new Regex("BETWEEN .*(dt|dh|data).* AND .*(dt|dh|data).*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        [Test]
        public void Sql_Join_ShouldUseWithNoLock()
        {
            Validator.AssertForEachSqlFile(
            (fileName, sql) =>
            {
                return sql.Contains("AuditRecord") || s_cteRegex.IsMatch(sql) || !(sql.Contains("JOIN") && !sql.Contains("NOLOCK"));
            },
            "Utilize WITH(NOLOCK) nos joins das consultas.");
        }

        [Test]
        public void Sql_Date_ShouldNotBeConverted()
        {
            Validator.AssertForEachSqlFile(
            (fileName, sql) =>
            {
                return !sql.Contains("CONVERT(DATE");
            },
            "Não faça conversões de data na query SQL, mas envie o parâmetro pronto direto do código C#: http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/diretrizes-gerais-de-codificacao#data-e-hora");
        }

        [Test]
        public void Sql_Date_ShouldNotUseBetween()
        {
            Validator.AssertForEachSqlFile(
            (fileName, sql) =>
            {
                return !s_betweenWithDateRegex.IsMatch(sql);
            },
            "Não utilize BETWEEN para intervalos de datas, no lugar utilize >= e  <. Mais detalhes em: http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/diretrizes-gerais-de-codificacao#data-e-hora");
        }

        [Test]
        public void Sql_Where_ShouldCompareEquivalent()
        {
            var regex = new Regex(@"[a-z]+\.id([a-z]+)\s=\s@cd\1", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Validator.AssertForEachSqlFile(
            (fileName, sql) =>
            {
                var match = regex.Match(sql);
                
                if (match.Success)
                {
                    return new ValidationResult(false, match.Value);
                }

                return true;
            },
            "A cláusula WHERE parece estar comparando valores que não são equivalentes");
        }

        [Test]
        public void Sql_Header_HeaderShouldHaveCommentedInputParametersSamples()
        {
            var isValidRegex = new Regex(@"^\s*/\*", RegexOptions.Compiled);
            var getArgumentsRegex = new Regex(@"@\S+", RegexOptions.Compiled);

            Validator.AssertForEachSqlFile(
            (fileName, sql) =>
            {
                if(isValidRegex.IsMatch(sql))
                {
                    var arguments = getArgumentsRegex.Matches(sql).Cast<Match>().GroupBy(m => m.Groups[1].Value);
                    
                    // Cada argumento deve aparecer, pelo menos, 3 vezes no arquivo .sql: na declaração, no exemplo de set e na própria consulta.
                    // Para consultas que não possuem argumento, como Parametro.ObterEstruturado.sql, então deve ter 0.
                    if (arguments.Any(g => g.Count() >= 3 ) || arguments.Count() == 0)
                    {
                        return true;
                    }
    
                }

                return false;
            },
            "adicione ao cabeçalho do arquivo .sql a declaração e setagem de exemplo dos parâmetros de input. Mais detalhes em: http://git.cwi.com.br/walmart/walmart-sgp-reescrita/wikis/banco-de-dados#cabe%C3%A7alho-dos-arquivos-sql-do-walmartsgpinfrastructuredata");
        }
    }
}
