using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Logging;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Logging
{
    [TestFixture]
    [Category("Framework")]
    public class LogSectionTest
    {
        [Test]
        public void Log_WithParentSection_SectionLog()
        {            
            var strategy = MockRepository.GenerateMock<ILogStrategy>();
            strategy.Expect(e => e.Info("<PARENT>"));
            strategy.Expect(e => e.Info("\tparent 1"));
            strategy.Expect(e => e.Info("\t<CHILDREN1>"));
            strategy.Expect(e => e.Info("\t\tchildren 1.1"));
            strategy.Expect(e => e.Info("\t\tchildren 1.2"));
            strategy.Expect(e => e.Info("\t\tchildren 1.3.1"));
            strategy.Expect(e => e.Info("\t\tchildren 1.3.2"));
            strategy.Expect(e => e.Info("\t</CHILDREN1>"));
            strategy.Expect(e => e.Info("</PARENT>"));

            LogService.Initialize(strategy);

            using (var parent = new LogSection("Parent"))
            {
                parent.Log("parent 1");

                using (var children1 = new LogSection("Children1", parent))
                {
                    children1.Log("children 1.1");
                    children1.Log("children 1.2");
                    children1.Log("children 1.3.1{0}children 1.3.2".With(Environment.NewLine));
                }
            }
            

            strategy.VerifyAllExpectations();
        }
    }
}
