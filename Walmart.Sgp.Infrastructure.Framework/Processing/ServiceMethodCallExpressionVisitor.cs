using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Implementa o visitor pattern para localizar um MethodCallExpression que representa uma chamada para um serviço.
    /// </summary>
    public class ServiceMethodCallExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// Obtém a expressão que representa a chamada para o método de serviço.
        /// </summary>
        public MethodCallExpression ServiceMethodCallExpression { get; private set; }

        /// <summary>
        /// Visits the children of the <see cref="T:System.Linq.Expressions.MethodCallExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (null == this.ServiceMethodCallExpression)
            {
                this.ServiceMethodCallExpression = node;
            }

            return node;
        }
    }
}
