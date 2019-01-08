namespace Walmart.Sgp.WebApi.Models
{
    public class ChangePasswordRequest
    {
        public string UserName { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}