using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Models.MatchViewModels
{
    public class ChooseRoomViewModel
    {
        public List<Room> Rooms { get; set; }
        public int RoomCapacity { get; set; }
    }
}
