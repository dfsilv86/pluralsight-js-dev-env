using System;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.WebApi.Models
{
    public class ItemRelacionamentoRequest
    {
        public RelacionamentoItemPrincipal RelacionamentoItemPrincipal { get; set; }

        public ItemDetalhe ItemDetalhe { get; set; }
    }
}