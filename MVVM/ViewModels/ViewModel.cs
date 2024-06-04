using NAudio.Wave;
using PropertyChanged;
using rinconLosCuatroTPVDesktop.MVVM.Models;
using rinconLosCuatroTPVDesktop.Services;
using RinconLosCuatroTPVDesktop;

using RinconLosCuatroTPVDesktop.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace rinconLosCuatroTPVDesktop.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {

        public ObservableCollection<Producto> ProductList { get; set; }

        public ObservableCollection<Producto> FilteredProductList { get; set; }

        public WebSocketService WebSocketService { get; set; }
        public ObservableCollection<Order> Pedidos { get; set; }

        public ServicioHttp Service { get; set; }

        public List<string> ListaTipos { get; set; }

        public Boolean isEditMode { get; set; }

        public Producto ActualProduct { get; set; }
        public ViewModel()
        {
            Service = new ServicioHttp();
            getProducts();
            ListaTipos = new List<string> { "Comida", "Bebida" };
            getOrders();
            WebSocketService = new WebSocketService();
            WebSocketService.OrderAddedEvent += RefreshOrders;

        }

        private async void RefreshOrders(object sender, EventArgs e)
        {
           
            try
            {
               List<Order> orders = await Service.getAllOrdersOfToday();
               
                Device.BeginInvokeOnMainThread(async () =>
                {
                    foreach (Order item in orders)
                    {
                        if (!isOrderAlreadyInList(item))
                        {
                            Pedidos.Add(item);
                        }
                    }

                    await newOrderSound();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        async public void getProducts()
        {
            var products = await Service.getAllProducts();
            ProductList = new ObservableCollection<Producto>(products);
            FilteredProductList = ProductList;
        }

        private Boolean isOrderAlreadyInList(Order order)
        {
            Boolean flag = false;
            foreach (Order currentOrder in Pedidos)
            {
                if (order.Id == currentOrder.Id)
                {
                    flag =  true;
                }

            }

            return flag;
        }

        async public Task getOrders()
        {
            var orders = await Service.getAllOrdersOfToday();
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Pedidos = new ObservableCollection<Order>(orders);
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

        public async void addProduct(string name,string description, Int32 price, bool availability, string type, string image)
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

        public async void updateProduct(Producto producto, string name, string description, Int16 price, bool availability, string type, string image)
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
            using (var audioFile = new AudioFileReader("C:\\Users\\Vanguard\\source\\repos\\RinconLosCuatroTPVDesktop\\RinconLosCuatroTPVDesktop\\Resources\\Sounds\\new-order.mp3"))
            {
                using(var outputDevice = new WaveOutEvent())
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
    }
}
