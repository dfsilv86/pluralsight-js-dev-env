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
        public void DateTime_ToLastMonthTime_UseExtensionMethod()
        {
            Validator.AssertForEachCsFile(
                (fileName, content) =>
                {
                    return !content.Contains(".AddMonths(1).AddMilliseconds");
                },
                "para obter o último momento do mês, utilize o extension method ToLastMonthTime");
        }       
    }
}
