using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Chino.Dtos.PaginatedList
{
    public class PaginatedListDto<T>
    {
        /// <summary>
        /// 当前页 索引序号
        /// </summary>
        [JsonPropertyName("index")]
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [JsonPropertyName("total")]
        public int TotalPages { get; set; }

        /// <summary>
        /// 总数据数
        /// </summary>
        [JsonPropertyName("count")]
        public long ItemCount { get; set; }

        /// <summary>
        /// 最终数据
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; }
    }
}
