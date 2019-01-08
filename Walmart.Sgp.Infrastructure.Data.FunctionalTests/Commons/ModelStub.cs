using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Data.FunctionalTests.Commons
{
    public class ModelStub : EntityBase
    {        
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
