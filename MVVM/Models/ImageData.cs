using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace rinconLosCuatroTPVDesktop.MVVM.Models
{
    public class ImageData
    {
        [JsonPropertyName("type")]
        public String Type { get; set; }
        [JsonPropertyName("data")]
        public int[] Data { get; set; }
    }
}
