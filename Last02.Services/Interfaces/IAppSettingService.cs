using Google.Api;
using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Interfaces
{
    public interface IAppSettingService
    {
        //public Task<ResponseBase<AppSetting?>> GetAppSetting();
        public Task<List<string>> GetCourseLanguagesListAsync();
    }
}
