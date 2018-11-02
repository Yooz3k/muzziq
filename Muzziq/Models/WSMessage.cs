using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Models
{
    public enum WSMessageType
    {
        TEXT,
        SCORE,
        AUDIO_START,
        AUDIO_END,
        OTHER,
    }
    public class WSMessage
    {
        private const string SEPARATOR = " ";
        public WSMessage(string message)
        {
            string firstWord = message.Split(SEPARATOR)[0];
            if (Enum.TryParse(firstWord, out WSMessageType messageType))
            {
                this.Type = messageType;
            }
            else
            {
                this.Type = WSMessageType.OTHER;
            }

            try
            {
                string text = message.Substring(message.Split(SEPARATOR)[0].Length);
                this.Text = text.Trim();
            }
            catch
            {
                this.Text = string.Empty;
            }
        }
        public WSMessage(WSMessageType type, string text)
        {
            this.Type = type;
            this.Text = text;
        }
        public WSMessageType Type { get; private set; }
        public string Text { get; private set; }
        public string MessageToSend {
            get
            {
                return Type + SEPARATOR + Text;
            }
        }

    }
}
