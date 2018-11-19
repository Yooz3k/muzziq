using Microsoft.AspNetCore.Http;
using Muzziq.Data;
using Muzziq.Models.Entities;

namespace Muzziq.Services
{
    public interface ISongService
    {
        void SaveSong(Song song);
        Song GetSongById(int id);
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

        public Song GetSongById(int id)
        {
            var song = _context.Songs.Find(id);
            return song;
        }
    }
}