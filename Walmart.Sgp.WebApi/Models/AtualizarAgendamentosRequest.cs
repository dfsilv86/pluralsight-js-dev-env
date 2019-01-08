using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Walmart.Sgp.WebApi.Models
{
    public class AtualizarAgendamentosRequest
    {
        public DateTime DtAgendamento { get; set; }

        public int[] InventarioAgendamentoIDs { get; set; }
    }
}