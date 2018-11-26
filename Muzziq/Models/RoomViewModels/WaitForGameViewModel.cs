using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Models.RoomViewModels
{
    public class WaitForGameViewModel
    {
        public Room Room { get; set; }
        public int RoomCapacity { get; set; }
        public int ownerID { get; set; }
        public int PlayerID { get; set; }
        public int RoomID { get; set; }
        public WaitForGameViewModel(Room room, int roomCapacity, int playerID)
        {
            this.Room = room;
            this.RoomID = room.Id;
            this.ownerID = room.OwnerId;
            this.RoomCapacity = roomCapacity;
            this.PlayerID = playerID;

        }
    }
}
