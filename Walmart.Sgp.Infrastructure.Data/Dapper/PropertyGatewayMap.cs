////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Reflection;
////using System.Text;
////using System.Threading.Tasks;
////using Walmart.Sgp.Infrastructure.Framework.Domain;

////namespace Walmart.Sgp.Infrastructure.Data.Dapper
////{
////    public class PropertyGatewayMap<TAggregateRoot>
////        where TAggregateRoot : IAggregateRoot
////    {
////        private Func<TAggregateRoot, object> m_getProperty;
////        private Func<object> m_getGateway;

////        public PropertyGatewayMap(Func<TAggregateRoot, object> getProperty, Func<object> getGateway)
////        {
////            m_getProperty = getProperty;
////            m_getGateway = getGateway;
////        }

////        public bool CanCreateGateway(PropertyInfo property)
////        {
////            return true;
////        }

////        public object CreateGateway()
////        {
////            return m_getGateway();
////        }

////        public void SetPropertyValue(object value)
////        {
////            ExpressionHelper
////        }
////    }
////}
