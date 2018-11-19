using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;

namespace Muzziq.Services
{
    public interface IRoomService
    {
        void CreateRoom(int ownerId, String name, int[] songIds);
        void JoinRoom(int roomId, int playerId);
        void LeaveRoom(Room room, Player player);
    }

    public class RoomService : IRoomService
    {
        private readonly IMatchService _matchService;
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context, IMatchService matchService)
        {
            _context = context;
            _matchService = matchService;
        }
        public void CreateRoom(int ownerId, String name, int[] songIds)
        {
            var room = new Room();

            // TODO
            // docelowo pobranie ownera z bazy jako Playera, który wykonał akcję
            var owner = new Player(new ApplicationUser(), "Jakiskoles");
            //Player owner = _context.Players.Find(ownerId);

            var players = new List<Player> { owner };

            var match = _matchService.CreateMatch(name, _context);
            var matches = new List<Match> { match };

            room.Players = players;
            room.Matches = matches;
            room.OwnerId = ownerId;

            _context.Add(room);
        }

        public void JoinRoom(int roomId, int playerId)
        {
            var room = _context.Rooms.Find(roomId);
            var player = _context.Players.Find(playerId);

            room.Players.Add(player);
            _context.Update(room);
        }

        public void LeaveRoom(Room room, Player player)
        {
            room.Players.Remove(player);
            _context.Update(room);
        }
    }
}
