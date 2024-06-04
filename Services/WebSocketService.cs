using Newtonsoft.Json;
using rinconLosCuatroTPVDesktop.MVVM.Models;
using SocketIOClient;

namespace RinconLosCuatroTPVDesktop.Services
{
    public class WebSocketService
    {
        private SocketIOClient.SocketIO Socket { get; set; }
        public bool IsConnected { get; private set; }
        public string Error { get; private set; }

        public event EventHandler<Order> OrderAdded;

        public event EventHandler OrderAddedEvent;

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

                    OrderAddedEvent?.Invoke(this, EventArgs.Empty);

                });

                await Socket.ConnectAsync();


            }
            catch (Exception e)
            {
                Error = e.Message;
                Console.WriteLine("Error: " + Error);
            }
        }

        protected virtual void OnOrderAdded(Order order)
        {
            try
            {
                OrderAdded?.Invoke(this, order);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private class OrderWrapper
        {
            public Order Order { get; set; }
        }
    }
}
