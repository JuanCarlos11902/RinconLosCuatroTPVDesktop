using PropertyChanged;
using rinconLosCuatroTPVDesktop.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RinconLosCuatroTPVDesktop.MVVM.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Check
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("totalPrice")]
        public Double TotalPrice { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("orders")]
        public ObservableCollection<Order> Orders { get; set; }
        [JsonPropertyName("checkStatus")]
        public String CheckStatus { get; set; }


    }
}
