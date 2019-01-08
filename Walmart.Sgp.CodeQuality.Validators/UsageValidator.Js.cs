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
        public void Js_ConsoleLog_ShouldUseLog()
        {
            Validator.AssertForEachJsFile(
            (fileName, js) =>
            {
                if(fileName.Equals("app.js"))
                {
                    return true;
                }

                return !js.Contains("console.log");
            },
            "Não utilize console.log, ao invés, utilize $log.");
        }

        [Test]
        public void Js_Debugger_ShouldNotLeaveDebugger()
        {            
            Validator.AssertForEachJsFile(
            (fileName, js) =>
            {
                return !js.Contains("debugger");
            },
            "Não faça commit de código .js com debugger.");
        }
    }
}
