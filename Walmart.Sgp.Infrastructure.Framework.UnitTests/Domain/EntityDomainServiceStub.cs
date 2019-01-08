using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    public class EntityDomainServiceStub : EntityDomainServiceBase<EntityStub, IDataGateway<EntityStub>>
    {
        public EntityDomainServiceStub(IDataGateway<EntityStub> gateway) 
            : base(gateway)
        {

        }
    }
}
