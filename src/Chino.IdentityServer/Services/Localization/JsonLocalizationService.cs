using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nekonya;

namespace Chino.IdentityServer.Services.Localization
{
    /// <summary>
    /// 从Json可配置的本地化Key/Value服务
    /// </summary>
    public class JsonLocalizationService : IJsonLocalizationService
    {
        private IConfiguration m_JsonLocalization;

        public JsonLocalizationService(IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("localization.json")
                .AddJsonFile($"localization.{environment.EnvironmentName}.json", true, true);

            m_JsonLocalization = builder.Build();
        }


        public string this[string key, CultureInfo cultureInfo]
        {
            get
            {
                return GetText(key, cultureInfo, key);
            }
        }

        public string this[string key, CultureInfo cultureInfo, params string[] args]
        {
            get
            {
                return string.Format(GetText(key, cultureInfo, key), args);
            }
        }

        public string GetText(string key, string culture, string defaultValue = null)
        {
            if (key.IsNullOrEmpty())
                return defaultValue;
            string final_key = $"{key}:{culture}";
            string final_key_default = $"{key}:default";
            var value = m_JsonLocalization[final_key];
            if (value == null)
                return m_JsonLocalization.GetValue<string>(final_key_default, defaultValue);
            else
                return value;
        }

        public string GetText(string key, CultureInfo cultureInfo, string defaultValue = null)
        {
            if (key.IsNullOrEmpty())
                return defaultValue;
            string final_key_default = $"{key}:default";

            if (cultureInfo == null)
                return m_JsonLocalization.GetValue<string>(final_key_default, defaultValue);

            string final_key = $"{key}:{cultureInfo.Name}";
            var value = m_JsonLocalization[final_key];
            if (value == null)
            {
                if (cultureInfo.Parent != null)
                {
                    final_key = $"{key}:{cultureInfo.Parent.Name}"; //这里不要递归，会死循环
                    value = m_JsonLocalization[final_key];
                    if (value != null)
                        return value;
                }
                return m_JsonLocalization.GetValue<string>(final_key_default, defaultValue);
            }
            else
                return value;
        }


    }
}
