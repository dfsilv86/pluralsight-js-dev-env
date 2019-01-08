using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Walmart.Sgp.Domain.EstruturaMercadologica;

namespace Walmart.Sgp.WebApi.Models
{
    public class RelacionamentoTransferenciaRequest
    {
        public long IDItemDetalheDestino { get; set; }

        public long IDItemDetalheOrigem { get; set; }

        public Loja[] Lojas { get; set; }
    }
}