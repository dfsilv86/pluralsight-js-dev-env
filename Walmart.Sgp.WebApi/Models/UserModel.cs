using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian;
using Walmart.Sgp.WebApi.App_Start;

namespace Walmart.Sgp.WebApi.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public IEnumerable<Papel> Papeis { get; set; }

        public IEnumerable<Loja> Lojas { get; set; }

        public int? IdLoja { get; set; }

        public bool HasPermissions { get; set; }

        public IEnumerable<UserMenuInfo> Menus { get; set; }

        public IEnumerable<UserActionInfo> Actions { get; set; }

        public Papel Role { get; set; }

        public TipoPermissao TipoPermissao { get; set; }

        public CultureInfo Culture { get; set; }

        public int? CdLoja { get; set; }

        public int? BandeiraId { get; set; }

        public bool HasAccessToSingleStore
        {
            get
            {
                return this.IdLoja.HasValue && null != this.Lojas && this.Lojas.Count() == 1;
            }
        }
    }
}