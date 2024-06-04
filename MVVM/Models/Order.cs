using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace rinconLosCuatroTPVDesktop.MVVM.Models
{
    public class Order
    {
        [JsonPropertyName("_id")]
        public String Id { get; set; }
        [JsonPropertyName("tableNumber")]
        public Int16 TableNumber { get; set; }
        [JsonPropertyName("products")]
        public List<Producto> Productos { get; set; }
        [JsonPropertyName("totalPrice")]
        public Int16 TotalPrice { get; set; }
        [JsonPropertyName("orderDescription")]
        public String OrderDescription { get; set; }

        [JsonPropertyName("orderStatus")]
        public String OrderStatus { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

    }
}
