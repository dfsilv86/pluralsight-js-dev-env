using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    public class DomainServiceStub : DomainServiceBase<int>
    {
        public DomainServiceStub(int mainGateway) : base(mainGateway)
        {
        }

        public void TestAssert(int target, params ISpec<int>[] specifications)
        {
            Assert(target, specifications);
        }

        public void TestAssert(IEnumerable<int> targets, params ISpec<int>[] specifications)
        {
            Assert(targets, specifications);
        }

        public void TestAssertAll(IEnumerable<int> targets, params ISpec<int>[] specifications)
        {
            AssertAll(targets, specifications);
        }
    }    
}
