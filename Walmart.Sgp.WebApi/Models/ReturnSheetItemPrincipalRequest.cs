using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.WebApi.Models
{
    public class ReturnSheetItemPrincipalRequest
    {
        public IEnumerable<ReturnSheetItemLoja> LojasAlteradas { get; set; }

        public int IdReturnSheet { get; set; }

        public decimal? PrecoVenda { get; set; }
    }
}
