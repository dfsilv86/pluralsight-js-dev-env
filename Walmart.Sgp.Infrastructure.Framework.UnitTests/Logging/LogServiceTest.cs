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
    public class LogServiceTest
    {
        [Test]
        public void AllMethods_Args_Log()
        {            
            // Console.
            LogService.Initialize(new ConsoleLogStrategy());
            LogService.Debug("A");
            LogService.Debug("{0}", "B");
            LogService.Info("A");
            LogService.Info("{0}", "B");
            LogService.Warning("A");
            LogService.Warning("{0}", "B");
            LogService.Error("A");
            LogService.Error("{0}", "B");

            // Mock.
            var strategy = MockRepository.GenerateMock<ILogStrategy>();
            strategy.Expect(e => e.Debug("test debug"));
            strategy.Expect(e => e.Info("test info"));
            strategy.Expect(e => e.Warning("test warning"));
            strategy.Expect(e => e.Error("test error"));

            LogService.Initialize(strategy);
            LogService.Debug("test debug");
            LogService.Info("test info");
            LogService.Warning("test warning");
            LogService.Error("test error");

            strategy.VerifyAllExpectations();
        }
    }
}
