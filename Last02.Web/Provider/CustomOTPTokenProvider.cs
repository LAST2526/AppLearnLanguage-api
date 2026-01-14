using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Last02.Web.Provider
{
    public class CustomOTPTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public CustomOTPTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
        {
        }

        public override async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return await manager.GetTwoFactorEnabledAsync(user);
        }
    }
}
