namespace Walmart.Sgp.WebApi.Models
{
    using System;

    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public int? IdExternoPapel { get; set; }

        public int? IdLoja { get; set; }
    }
}