using Microsoft.Maui.Controls;
using NAudio.Wave;
using PropertyChanged;
using rinconLosCuatroTPVDesktop.MVVM.Models;
using rinconLosCuatroTPVDesktop.Services;
using RinconLosCuatroTPVDesktop;
using RinconLosCuatroTPVDesktop.MVVM.Models;
using RinconLosCuatroTPVDesktop.Services;
using SocketIOClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace rinconLosCuatroTPVDesktop.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {

        private SocketIOClient.SocketIO Socket { get; set; }

        public ObservableCollection<Producto> ProductList { get; set; }

        public ObservableCollection<Producto> FilteredProductList { get; set; }

        public WebSocketService WebSocketService { get; set; }
        public ObservableCollection<Order> Pedidos { get; set; } = new ObservableCollection<Order>();

        public ServicioHttp Service { get; set; }

        public List<string> ListaTipos { get; set; }

        public bool isEditMode { get; set; }

        public Producto ActualProduct { get; set; }

        public Check Check { get; set; }
        public ViewModel()
        {
            init();
        }

        public async void init()
        {
            Service = new ServicioHttp();
            getProducts();
            Pedidos.Add(new Order("hidden"));
            ListaTipos = new List<string> { "Comida", "Bebida" };
            await getOrders();
            getCheckOfToday();
            ConnectWebSocket().ConfigureAwait(false);
        }

        private async Task ConnectWebSocket()
        {

            try
            {
                Socket = new SocketIOClient.SocketIO("https://floating-caverns-13553-5b60c3be1747.herokuapp.com/", new SocketIOOptions()
                {
                    Reconnection = true,
                    Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
                });

                Socket.OnConnected += (sender, e) =>
                {
                    Console.WriteLine("Connected");
                };

                Socket.OnDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Disconnected");
                };

                Socket.OnReconnectAttempt += (sender, e) =>
                {
                    Console.WriteLine("Reconnecting");
                };

                Socket.On("orderAdded", async (data) =>
                {
                    string cadena = data.ToString();
                    var order = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(cadena, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true
                    });
                    Pedidos.Add(order[0]);
                    await newOrderSound();
                });
                await Socket.ConnectAsync();


            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        async public void getProducts()
        {
            var products = await Service.getAllProducts();
            ProductList = new ObservableCollection<Producto>(products);
            FilteredProductList = ProductList;
        }

        async public Task getOrders()
        {
            var orders = await Service.getAllOrdersOfToday();
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                foreach (var order in orders)
                {
                    Pedidos.Add(order);
                }
            });
        }

        public void filterProducts(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                FilteredProductList = ProductList;
            }

            List<Producto> tempList = ProductList.Where(p => p.Name.Contains(filter)).ToList();
            FilteredProductList = new ObservableCollection<Producto>(tempList);

        }

        public async void addProduct(string name,string description, Double price, bool availability, string type, string image)
        {

            string cadena = "";
            if (name == null)
            {
                cadena += "Debes asignarle un nombre al producto";
            }
            if (description == null)
            {
                description = "";
            }
            if (price == 0)
            {
                cadena += Environment.NewLine + "- Asígnale un precio al producto";
            }
            if (type.Equals(""))
            {
                cadena += Environment.NewLine + "- Asígnale un tipo al producto (Comida/Bebida)";
            }
            if(image == null)
            {
                cadena += Environment.NewLine + "- Asígnale una imagen al producto";
            }

            if (cadena.Equals(""))
            {
                Producto producto = await Service.addProduct(name,description,price,availability,type,image);
                this.ProductList.Add(producto);
                this.FilteredProductList = ProductList;
                //Crear un alert con el mensaje de que se ha añadido el producto con un displayAlert
                await Application.Current.MainPage.DisplayAlert("Producto añadido", "El producto ha sido añadido correctamente", "Aceptar");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }

            else
            {
                await Application.Current.MainPage.DisplayAlert("Error al intentar insertar un producto", cadena, "Aceptar");
            }

        }

        public void deleteProduct(Producto product)
        {
           ProductList.Remove(product);
           FilteredProductList.Remove(product);

            Service.deleteProduct(product);
        }

        public void changeAvailability(Producto product)
        {
                Service.changeAvailability(product);
        }

        public async void updateProduct(Producto producto, string name, string description, Double price, bool availability, string type, string image)
        {

            string imageString = "";
            if (producto != null)
            {
                if (name != null)
                {
                    producto.Name = name;
                }
                if (description != null)
                {
                    producto.Description = description;
                }
                if (price != 0)
                {
                    producto.Price = price;
                }
                if (type != null)
                {
                    producto.Type = type;
                }
                if (image != null)
                {
                    imageString = image;
                }

                Producto product = await Service.updateProduct(producto, imageString);

                foreach (var producto1 in ProductList)
                {
                    if (producto1.Id == product.Id)
                    {
                        producto1.Name = product.Name;
                        producto1.Description = product.Description;
                        producto1.Price = product.Price;
                        producto1.Availability = product.Availability;
                        producto1.Type = product.Type;
                        producto1.Image = product.Image;

                    }
                }

            }
        }

        private async Task newOrderSound()
        {
            try
            {
                using (var audioFile = new AudioFileReader("C:\\Users\\Vanguard\\source\\repos\\RinconLosCuatroTPVDesktop\\RinconLosCuatroTPVDesktop\\Resources\\Sounds\\new-order.mp3"))
                {
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Play();

                        while (outputDevice.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async void completeOrder(Order order)
        {
            order.OrderStatus = "completed";
            Service.completeOrder(order);
            Check.Orders.Add(order);
            Check.TotalPrice += order.TotalPrice;
            Pedidos.Remove(order);
            updateCheck();
        }

        public async void getCheckOfToday()
        {
            Check = await Service.getCheckOfToday();
            if (Check.Orders.Count == 0 && Check.TotalPrice == 0 && Check.CheckStatus.Equals("notCreated"))
            {
                Check = await Service.createCheck();
            }
        }

        public async void updateCheck()
        {
            Service.updateCheck(Check);
        }   
    }
}
