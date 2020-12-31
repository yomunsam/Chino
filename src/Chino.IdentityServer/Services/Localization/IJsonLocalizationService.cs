using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Chino.IdentityServer.Services.Localization
{
    /// <summary>
    /// 从Json配置的本地化Key/Value服务
    /// </summary>
    public interface IJsonLocalizationService
    {
        string this[string key, CultureInfo cultureInfo] { get; }
        string this[string key, CultureInfo cultureInfo, params string[] args] { get; }

        string GetText(string key, string culture, string defaultValue = null);
        string GetText(string key, CultureInfo cultureInfo, string defaultValue = null);
    }
}
