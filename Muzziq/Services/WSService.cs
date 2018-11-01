using Microsoft.AspNetCore.Http;
using Muzziq.Models;
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
    public interface IWSService
    {
        void SendAll(WSMessage message);
        Task Recive(HttpContext context, Func<Task> next);
    }

    public class WSService : IWSService
    {
        private static Dictionary<Guid, WebSocket> WebSockets = new Dictionary<Guid, WebSocket>();
        private const int BUFFER_SIZE = 4096;

        public async Task Recive(HttpContext context, Func<Task> next)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                Guid uid = Guid.NewGuid(); //whatever
                byte[] buffer;
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                WebSockets.Add(uid, webSocket);
                Console.WriteLine("Add webSocket with uid: " + uid);
                WebSocketReceiveResult result = null;
                do
                {
                    buffer = new byte[BUFFER_SIZE];
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    string messageBufferSize = Encoding.UTF8.GetString(buffer);
                    string message = messageBufferSize.Substring(0, messageBufferSize.IndexOf('\0'));
                    WSMessage wsMessage = new WSMessage(message);
                    Console.WriteLine("wsMessage.Type: " + wsMessage.Type);
                    Console.WriteLine("wsMessage.Text: " + wsMessage.Text);
                    Dispatch(wsMessage, uid);
                    SendAll(new WSMessage(WSMessageType.SCORE, "Zdobyles 100 punktow"));
                } while (result != null && !result.CloseStatus.HasValue);
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                Console.WriteLine("Remove webSocket with uid: " + uid);
                WebSockets.Remove(uid);
            }
            else
            {
                await next();
            }
        }

        public async void SendAll(WSMessage message)
        {
            foreach (WebSocket webSocket in WebSockets.Values)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(message.MessageToSend);
                await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async void Send(WSMessage message, Guid uid)
        {
            WebSocket webSocket = WebSockets[uid];
            byte[] bytes = Encoding.ASCII.GetBytes(message.MessageToSend);
            await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private void Dispatch(WSMessage message, Guid uid)
        {
            switch (message.Type)
            {
                case WSMessageType.TEXT: break;
                case WSMessageType.SCORE: break;
                case WSMessageType.AUDIO: break;
                case WSMessageType.OTHER: break;
            }
        }


    }
}
