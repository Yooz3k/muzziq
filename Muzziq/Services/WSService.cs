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
        void SendAll(string message);
        Task Recive(HttpContext context, Func<Task> next);
    }

    public class WSService : IWSService
    {
        private static IDictionary WebSockets = new Dictionary<string, WebSocket>();
        private const string SEPARATOR = " ";
        private const int BUFFER_SIZE = 4096;

        public async Task Recive(HttpContext context, Func<Task> next)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                string id = context.Request.Path.ToString();
                byte[] buffer;
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                WebSockets.Add(id, webSocket);

                WebSocketReceiveResult result = null;
                do
                {
                    buffer = new byte[BUFFER_SIZE];
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    string messageBufferSize = Encoding.UTF8.GetString(buffer);
                    string message = messageBufferSize.Substring(0, messageBufferSize.IndexOf('\0'));
                    WSMessage wsMessage = new WSMessage
                    {
                        Type = getMessageType(message),
                        Text = getMessageText(message)
                    };
                    Console.WriteLine("wsMessage.Type: " + wsMessage.Type);
                    Console.WriteLine("wsMessage.Text: " + wsMessage.Text);
                    Dispatch(wsMessage);
                } while (result != null && !result.CloseStatus.HasValue);
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                WebSockets.Remove(id);
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

        private void Dispatch(WSMessage message)
        {
            switch (message.Type)
            {
                case WSMessageType.TEXT: break;
                case WSMessageType.SCORE: break;
                case WSMessageType.AUDIO: break;
                case WSMessageType.OTHER: break;
            }
        }

        private WSMessageType getMessageType(string message)
        {
            string firstWord = message.Split(SEPARATOR)[0];
            if (Enum.TryParse(firstWord, out WSMessageType messageType))
            {
                return messageType;
            }
            else
            {
                return WSMessageType.OTHER;
            }
        }

        private string getMessageText(string message)
        {
            try
            {
                string text = message.Substring(message.Split(SEPARATOR)[0].Length);
                return text.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
