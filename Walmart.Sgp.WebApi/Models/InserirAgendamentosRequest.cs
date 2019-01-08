using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Walmart.Sgp.WebApi.Models
{
    public class InserirAgendamentosRequest
    {
        public DateTime DtAgendamento { get; set; }

        public int CdSistema { get; set; }

        public int IDBandeira { get; set; }

        public int? CdLoja { get; set; }

        public int? CdDepartamento { get; set; }
    }
}