using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Models
{
    public class Room
    {
        public Room()
        {
        }

        public Room(int ownerId, string name, List<Player> players, List<Match> matches)
        {
            OwnerId = ownerId;
            Name = name;
            Players = players;
            Matches = matches;
        }

        public int Id { get; set; }
        public int OwnerId{ get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public List<Match> Matches { get; set; }
    }
}
