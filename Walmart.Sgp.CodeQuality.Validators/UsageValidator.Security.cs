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
        public void Action_Commit_OnlyInWriteActions()
        {
            var countWriteActionsRegex = new Regex("(HttpPost|HttpPut|HttpDelete)", RegexOptions.Compiled);
            var countCommitRegex = new Regex("Commit", RegexOptions.Compiled);

            Validator.AssertForEachWebApiControllerFile(
            (fileName, content) =>
            {
                var writeActounsCount = countWriteActionsRegex.Matches(content).Count;
                var commitRegex = countCommitRegex.Matches(content).Count;

                return commitRegex <= writeActounsCount;
            },
            "O número de commits é maior do que o número de actions de escrita. Apenas actions de escrita (POST|PUT|DELETE) devem realizar Commit.");
        }
    }
}
