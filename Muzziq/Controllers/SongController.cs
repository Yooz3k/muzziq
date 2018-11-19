using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Muzziq.Data;
using Muzziq.Models.Entities;
using Muzziq.Services;

namespace Muzziq.Controllers
{
    public class SongController : Controller
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        public IActionResult AddNewSongView()
        {
            return View();
        }

        [HttpPost("uploadSong")]
        public async Task<IActionResult> AddNewSong(string title, string author, string album, string genre, int year, IFormFile file)
        {
            if (file is null || !file.ContentType.Contains("audio"))
                return View("AddNewSongView"); //ToDo: return view 'upload not successful' or sth
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                var song = new Song(title, author, album, genre, year, bytes);
                _songService.SaveSong(song);
            }
            return View("AddNewSongView");
        }
    }
}