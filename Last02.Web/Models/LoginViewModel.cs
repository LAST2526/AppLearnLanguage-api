using System.ComponentModel.DataAnnotations;

namespace Last02.Web.Models
{
    public class LoginViewModel
    {
        public string? Username { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
