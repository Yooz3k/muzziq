using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Models.RoomViewModels
{
    public class WaitForGameModel
    {
        public List<Player> Players { get; set; }
        public String MatchName;

        public WaitForGameModel(String matchName, List<Player> players)
        {
            this.MatchName = matchName;
            this.Players = players;
        }

        public WaitForGameModel() {
            this.Players = new List<Player>();
        }
    }
}
