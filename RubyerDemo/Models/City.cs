using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RubyerDemo.Models
{
    /// <summary>
    /// 城市
    /// </summary>
    public class City
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("children")]
        public List<City> Children { get; set; }
    }
}
