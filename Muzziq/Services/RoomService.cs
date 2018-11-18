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
        public void CreateRoom(int ownerId, String name, int[] songIds, ApplicationDbContext _context)
        {
            Room room = new Room();

            // TODO
            // docelowo pobranie ownera z bazy jako Playera, który wykonał akcję
            Player owner = new Player(new ApplicationUser(), "Jakiskoles");
            //Player owner = _context.Players.Find(ownerId);

            List<Player> players = new List<Player> { owner };

            Match match = matchService.CreateMatch(name, _context);
            List<Match> matches = new List<Match> { match };

            room.Players = players;
            room.Matches = matches;
            room.OwnerId = ownerId;

            _context.Add(room);
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
