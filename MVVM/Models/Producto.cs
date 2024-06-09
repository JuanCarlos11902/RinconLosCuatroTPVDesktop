using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using PropertyChanged;

namespace rinconLosCuatroTPVDesktop.MVVM.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Producto
    {
        [JsonPropertyName("_id")]
        public String Id { get; set; }
        [JsonPropertyName("name")]
        public String Name { get; set; }
        [JsonPropertyName("description")]
        public String Description { get; set; }
        [JsonPropertyName("price")]
        public Double Price { get; set; }
        [JsonPropertyName("availability")]
        public Boolean Availability { get; set; }
        [JsonPropertyName("type")]
        public String Type { get; set; }
        [JsonPropertyName("image")]
        public ImageData Image { get; set; }

    }
}
