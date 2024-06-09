using rinconLosCuatroTPVDesktop.MVVM.Models;
using RinconLosCuatroTPVDesktop.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace rinconLosCuatroTPVDesktop.Services
{

    public class ServicioHttp
    {
        private HttpClient HttpClient { get; set; }

        public ServicioHttp()
        {
            HttpClient = new HttpClient();
            HttpClient.BaseAddress = new Uri("http://127.0.0.1:3000/");
        }

        public async Task<List<Producto>> getAllProducts()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("products/getAll");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            var listaProducto = JsonSerializer.Deserialize<List<Producto>>(json);
            return listaProducto;

        }

        public async Task<Producto> addProduct(string name, string description, Double price, bool availability, string type, string image)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(name), "name");

            formData.Add(new StringContent(description), "description");
            formData.Add(new StringContent(price.ToString()), "price");
            formData.Add(new StringContent(availability.ToString()), "availability");
            formData.Add(new StringContent(type), "type");

            var fileStreamContent = new StreamContent(File.OpenRead(image));
            formData.Add(fileStreamContent, "image", Path.GetFileName(image));
            Producto producto = null;

            try
            {
                var response = await HttpClient.PostAsync("products/add", formData);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                producto = JsonSerializer.Deserialize<Producto>(json);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return producto;

        }

        public async void deleteProduct(Producto product)
        {
            try
            {
                string endpoint = "products/delete/" + product.Id;
                var response = await HttpClient.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void changeAvailability(Producto product)
        {
            try
            {
                string endpoint = "products/changeAvailability/" + product.Id;
                var response = await HttpClient.PatchAsync(endpoint, null);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<Producto> updateProduct(Producto product, string image)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(product.Name), "name");
            formData.Add(new StringContent(product.Description), "description");
            formData.Add(new StringContent(product.Price.ToString()), "price");
            formData.Add(new StringContent(product.Availability.ToString()), "availability");
            formData.Add(new StringContent(product.Type), "type");
            var fileStreamContent = new StreamContent(File.OpenRead(image));
            formData.Add(fileStreamContent, "image", Path.GetFileName(image));
            Producto producto = null;
            try
            {
                string endpoint = "products/updateProduct/" + product.Id;
                var response = await HttpClient.PatchAsync(endpoint, formData);
                string json = await response.Content.ReadAsStringAsync();
                producto = JsonSerializer.Deserialize<Producto>(json);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return producto;
        }

        public async Task<List<Order>> getAllOrdersOfToday()
        {
            HttpResponseMessage response = await HttpClient.GetAsync("order/getOrdersOfToday");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            List<Order> lista = JsonSerializer.Deserialize<List<Order>>(json);
            return lista;
        }

        public async void completeOrder(Order order)
        {
            string endpoint = "order/completeOrder/" + order.Id;

            var json = JsonSerializer.Serialize(new { orderStatus = order.OrderStatus.ToString() });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PatchAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Check> createCheck()
        {
            string endpoint = "check/add";
            var response = await HttpClient.PostAsync(endpoint, null);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            Check check = JsonSerializer.Deserialize<Check>(json);

            return check;
        }

        public async Task<Check> getCheckOfToday()
        {
            string endpoint = "check/getCheckOfToday";
            var response = await HttpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            var check = JsonSerializer.Deserialize<Check>(json);

            return check;
        }

        public async void updateCheck(Check check)
        {
            string endpoint = "check/update/" + check.Id;
            var jsonContent = new StringContent(JsonSerializer.Serialize(new
            {
                totalPrice = check.TotalPrice,
                checkStatus = check.CheckStatus,
                orders = check.Orders
            }), Encoding.UTF8, "application/json");

            var response = await HttpClient.PatchAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();

        }   
    }
}
