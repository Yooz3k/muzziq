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
        AUDIO,
        OTHER,
    }
    public class WSMessage
    {
        public WSMessageType Type { get; set; }
        public string Text { get; set; }
    }
}
