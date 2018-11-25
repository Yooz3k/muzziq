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

        public int OwnerId { get; set; }

        public WaitForGameViewModel(Room room, int roomCapacity)
        {
            this.Room = room;
            this.RoomCapacity = roomCapacity;
        }

        public WaitForGameViewModel(Room room)
        {
            this.Room = room;
        }

        public WaitForGameViewModel(Room room, int roomCapacity, int ownerId)
        {
            this.Room = room;
            this.RoomCapacity = roomCapacity;
            this.OwnerId = ownerId;
        }
    }
}
