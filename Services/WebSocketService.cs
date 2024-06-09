using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using rinconLosCuatroTPVDesktop.MVVM.Models;
using SocketIOClient;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RinconLosCuatroTPVDesktop.Services
{
    public class WebSocketService
    {
        private SocketIOClient.SocketIO Socket { get; set; }
        public bool IsConnected { get; private set; }
        public string Error { get; private set; }

        public event EventHandler<Order> OrderAdded;

        public WebSocketService()
        {
            ConnectWebSocket().ConfigureAwait(false);
        }

        private async Task ConnectWebSocket()
        {

            try
            {
                Socket = new SocketIOClient.SocketIO("http://127.0.0.1:3000", new SocketIOOptions()
                {
                    Reconnection = true,
                    Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
                });

                Socket.OnConnected += (sender, e) =>
                {
                    IsConnected = true;
                    Console.WriteLine("Connected");
                };

                Socket.OnDisconnected += (sender, e) =>
                {
                    IsConnected = false;
                    Console.WriteLine("Disconnected");
                };

                Socket.OnReconnectAttempt += (sender, e) =>
                {
                    Console.WriteLine("Reconnecting");
                };

                Socket.On("orderAdded", (data) =>
                {
                    String cadena = data.ToString();
                    var order = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(cadena, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true
                    });

                    OrderAdded.Invoke(this, order[0]);

                });
                await Socket.ConnectAsync();


            }
            catch (Exception e)
            {
                Error = e.Message;
                Console.WriteLine("Error: " + Error);
            }
        }

        private class OrderWrapper
        {
            public Order Order { get; set; }
        }
    }
}
