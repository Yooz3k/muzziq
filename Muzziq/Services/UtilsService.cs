﻿using Microsoft.EntityFrameworkCore;
using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Muzziq.Services
{
    public interface IUtilsService
    {
        Room GetRoomById(int roomId);
        Match GetMatchById(int matchId);
        Player GetPlayerById(int playerId);
        Song GetSongById(int songId);
        Round GetRoundById(int roundId);
    }

    public class UtilsService : IUtilsService
    {
        private readonly ApplicationDbContext _context;

        public UtilsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Match GetMatchById(int matchId)
        {
            return _context.Matches.Find(matchId);
        }

        public Player GetPlayerById(int playerId)
        {
            return _context.Players.Find(playerId);
        }

        public Room GetRoomById(int roomId)
        {
            return _context.Room.Where(r => r.Id == roomId).Include(r => r.Players).FirstOrDefault();
        }

        public Song GetSongById(int songId)
        {
            return _context.Songs.Find(songId);
        }

        public int GetPlayerByUserId(string userId)
        {
            //TODO: wyszukanie id playera dla określonego id usera

            var player = _context.Players
                .FirstOrDefault(p => p.User.Id.Equals(userId));

            return player.Id;
        }

        public Round GetRoundById(int roundId)
        {
            return _context.Rounds
                .FirstOrDefault(r => r.Id.Equals(roundId));
        }
    }
}