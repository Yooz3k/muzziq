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
        private readonly IUtilsService _utilsService;

        public RoomService(ApplicationDbContext context, IMatchService matchService, IUtilsService utilsService)
        {
            _context = context;
            _matchService = matchService;
            _utilsService = utilsService;
        }
        public Room CreateRoom(int ownerId, String name, int[] songIds)
        {
            var room = new Room();

            List<Song> songs = new List<Song>();
            foreach (int songId in songIds)
            {
                songs.Add(_utilsService.GetSongById(songId));
            }

            var owner = _utilsService.GetPlayerById(ownerId);
            var players = new List<Player> { owner };

            room.Players = players;
            room.Matches = new List<Match>();
            room.OwnerId = ownerId;

            _context.Rooms.Add(room);
            _context.SaveChanges();
            return room;
        }

        public void JoinRoom(int roomId, int playerId)
        {
            var room = _utilsService.GetRoomById(roomId);
            var player = _utilsService.GetPlayerById(playerId);

            room.Players.Add(player);
            _context.Update(room);
            _context.SaveChanges();
        }

        public void LeaveRoom(Room room, Player player)
        {
            room.Players.Remove(player);
            _context.Update(room);
            _context.SaveChanges();
        }
    }
}
