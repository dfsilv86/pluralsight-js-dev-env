using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Processing;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Processing
{
    [TestFixture]
    [Category("Framework")]
    [Category("Processing")]
    public class ServiceMethodCallExpressionVisitorTest
    {
        [Test]
        public void Visit_Expression_MethodCallExpression()
        {
            Expression<Func<string>> call = () => 1.GetType().GetMethods().FirstOrDefault().Name;

            ServiceMethodCallExpressionVisitor target = new ServiceMethodCallExpressionVisitor();

            target.Visit(call);

            Assert.IsNotNull(target.ServiceMethodCallExpression);

            Assert.AreEqual("FirstOrDefault", target.ServiceMethodCallExpression.Method.Name);
        }
    }
}
