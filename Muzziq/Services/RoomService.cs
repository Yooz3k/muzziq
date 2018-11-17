using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Services
{
    public class RoomService
    {
        private MatchService matchService;
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
            matchService = new MatchService(_context);
        }
        public Room CreateRoom(Room room, ApplicationDbContext _context)
        {
            Room updatedRoom = room;
            // TODO ustawianie właściciela pokoju
            // var userId = User.Identity.Name; // email

            // docelowo pobranie ownera z bazy jako Playera, który wykonał akcję
            Player moderator = new Player(new ApplicationUser(), "Jakiskoles");
            List<Player> players = new List<Player> { moderator };

            Match match = matchService.CreateMatch(updatedRoom.Name, _context);
            List<Match> matches = new List<Match> { match };

            updatedRoom.Players = players;
            updatedRoom.Matches = matches;
            //updatedRoom.OwnerId = ownerId;
            return updatedRoom;
        }

        public void JoinRoom(Room room, Player player, ApplicationDbContext _context)
        {
            room.Players.Add(player);
            _context.Update(room);
        }

        public void LeaveRoom(Room room, Player player, ApplicationDbContext _context)
        {
            room.Players.Remove(player);
            _context.Update(room);
        }
    }
}
