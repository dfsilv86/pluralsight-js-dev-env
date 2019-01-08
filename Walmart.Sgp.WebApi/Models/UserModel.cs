using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Pstore.Domain;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.IO.Importing.WebGuardian;
using Walmart.Sgp.WebApi.App_Start;
using Pstore.Domain.Anuncio;
using Walmart.Sgp.Domain.Acessos;
using Pstore.Domain.Dispensa;

namespace Pstore.WebApi.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public Papel Role { get; set; }

        public Dispensa Dispensa { get; set; }

        public IEnumerable<Anuncio> Anuncios { get; set; }

        public IEnumerable<UserMenuInfo> Menus { get; set; }

        public IEnumerable<UserActionInfo> Actions { get; set; }

        public CultureInfo Culture { get; set; }       
    }
}