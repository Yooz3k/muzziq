using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Muzziq.Services
{
    public enum WSMessageType
    {
        CONNECT,
        TEXT,
        DISCONNECT
    }
    public interface IWSService
    {
        void SendAll(string message);
        Task Recive(HttpContext context, Func<Task> next);
    }

    public class WSService : IWSService
    {
        private IList WebSockets = new List<WebSocket>();

        public async Task Recive(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path == "/ws1" || context.Request.Path == "/ws2")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    byte[] buffer = new byte[1024 * 4];
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    this.WebSockets.Add(webSocket);
                    //Console.WriteLine(webSocket);
                    WebSockets.Add(webSocket);
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, true, CancellationToken.None);
                    while (!result.CloseStatus.HasValue)
                    {
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, true, CancellationToken.None);
                        string message = Encoding.ASCII.GetString(buffer);
                        Console.WriteLine(message);
                        //TODO Dispatch message
                        this.SendAll(message);
                    }
                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    WebSockets.Remove(webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await next();
            }
        }

        public async void SendAll(string message)
        {
            foreach (WebSocket webSocket in WebSockets)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
