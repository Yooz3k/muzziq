using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;

namespace Muzziq.Services
{
    public interface IRoomService
    {
        Room CreateRoom(int ownerId, String name, int[] songIds);
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
        public Room CreateRoom(int ownerId, String name, int[] songIds)
        {
            Player owner = _context.Players.Find(ownerId);

            var players = new List<Player> { owner };

            var match = _matchService.CreateMatch(name, _context);
            var matches = new List<Match> { match };

            Room room = new Room(ownerId, name, players, matches);

            return _context.Add(room).Entity;
        }

        public void JoinRoom(int roomId, int playerId)
        {
            var room = _context.Room.Find(roomId);
            _context.Entry(room).Collection(s => s.Players).Load();
            var player = _context.Players.Find(playerId);

            room.Players.Add(player);
            _context.Update(room);
            _context.SaveChanges();
        }

        public void LeaveRoom(Room room, Player player)
        {
            room.Players.Remove(player);
            _context.Update(room);
        }
    }
}
