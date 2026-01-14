using Microsoft.AspNetCore.Identity;

namespace Last02.Web.Provider
{
    public class CustomProtectionTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public CustomProtectionTokenProviderOptions()
        {
            Name = "OTP";
        }
    }
}
