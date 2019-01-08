using System.Collections.Generic;

namespace Walmart.Sgp.WebApi.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public UserModel User { get; set; }        
    }
}