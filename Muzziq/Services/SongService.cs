﻿using Microsoft.AspNetCore.Http;
using Muzziq.Data;
using Muzziq.Models.Entities;

namespace Muzziq.Services
{
    public interface ISongService
    {
        void SaveSong(Song song);
    }

    public class SongService : ISongService
    {
        private readonly ApplicationDbContext _context;

        public SongService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SaveSong(Song song)
        {
            _context.Songs.Add(song);
            _context.SaveChanges();
        }
    }
}