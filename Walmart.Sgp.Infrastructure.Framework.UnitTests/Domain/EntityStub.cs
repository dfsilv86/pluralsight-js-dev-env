using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    public class EntityStub : EntityBase, IAggregateRoot, IStampContainer
    {
        public int Value { get; set; }

        public DateTime DhCriacao { get; set; }

        public DateTime? DhAtualizacao { get; set; }

        public int? CdUsuarioCriacao { get; set; }

        public int? CdUsuarioAtualizacao { get; set; }

        public EntityNoAggregateRootStub Child { get; set; }
    }
}