using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Nekonya;

namespace Chino.IdentityServer.Configures
{
    public class CountryCodeConfiguration
    {
        [JsonPropertyName("data")]
        public Item[] Data { get; set; }

        public Dictionary<int, Item> LCID_Dict { get; private set; } = new Dictionary<int, Item>();

        public Dictionary<string, Item> Culture_Dict { get; private set; } = new Dictionary<string, Item>();
        public Dictionary<string, Item> DialingCodeWithoutPlus_Dict { get; private set; } = new Dictionary<string, Item>();

        public class Item
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("dial_code")]
            public string DialingCode { get; set; }

            [JsonPropertyName("code")]
            public string Code { get; set; }

            /// <summary>
            /// 微软Locale ID
            /// </summary>
            [JsonPropertyName("LCID")]
            public int LCID { get; set; }

            [JsonPropertyName("culture")]
            public string CultureName { get; set; }

            /// <summary>
            /// 没有"+"号的国家代码
            /// </summary>
            public string DialingCodeWithoutPlus { get; set; }
        }


        public static CountryCodeConfiguration GetConfiguration()
        {
            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "countryCodes.json"));
            var obj = JsonSerializer.Deserialize<CountryCodeConfiguration>(json);
            List<Item> tempList = new List<Item>();
            foreach(var item in obj.Data)
            {
                if (!item.DialingCode.IsNullOrEmpty())
                {
                    if (item.DialingCode.StartsWith('+'))
                        item.DialingCodeWithoutPlus = item.DialingCode.Substring(1, item.DialingCode.Length - 1);
                    else
                        item.DialingCodeWithoutPlus = item.DialingCode;

                    tempList.Add(item);
                    obj.DialingCodeWithoutPlus_Dict.AddIfNotExist(item.DialingCodeWithoutPlus, item);
                }

                if (!item.CultureName.IsNullOrEmpty())
                    obj.Culture_Dict.AddIfNotExist(item.CultureName, item);

                if (item.LCID != 0)
                    obj.LCID_Dict.AddIfNotExist(item.LCID, item);
            }
            obj.Data = tempList
                .OrderBy(item => item.Name)
                .ToArray();
            return obj;
        }

    }
}
