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
        public void All_GitConflictMarkers_MergeConflictsShouldBeResolved()
        {            
            Validator.AssertForEachFile(
                (fileName, content) =>
                {
                    return !content.Contains("<<<<<<<") && !content.Contains(">>>>>>>");
                },
                "Conflitos de merge devem ser resolvidos.");
        }

        [Test]
        public void AllCs_DeepNamespaceAccess_ShouldUseUsing()
        {
            Validator.AssertForEachCsFile((fileName, content) =>
            {
                return !content.Contains("Walmart.Sgp.Infrastructure.Framework.Runtime.RuntimeContext");
                
            }, "Não utilize muitos namespaces aninhados para acessar um tipo. Utilize a cláusula using.");
        }

        [Test]
        [Ignore("DZ-para vários desses casos não existe alternativa")]
        public void AllCs_LongLines_AvoidLongUnreadbleCodeLines()
        {
            Validator.AssertForEachCsFile((fileName, content) =>
            {
                if(fileName.EndsWith("Generator.cs"))
                {
                    return true;
                }

                var longLine = content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Where(l => l.Contains("\"") && l.Trim().Length > 200)
                                      .FirstOrDefault();

                if(longLine == null)
                {
                    return true;
                }
                else
                {
                    return new ValidationResult(false, longLine);
                }
            }, "Evite linhas de códigos com mais de 200 chars.");
        }
    }
}
