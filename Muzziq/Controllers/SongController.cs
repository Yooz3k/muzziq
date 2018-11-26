using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Muzziq.Data;
using Muzziq.Models.Entities;
using Muzziq.Models.SongViewModels;
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
            AddNewSongViewModel model = new AddNewSongViewModel();
            return View(model);
        }

        public async Task<IActionResult> SaveNewSong(AddNewSongViewModel model)
        {
            if (!AllDataProvided(model))
            {
                return View("AddNewSongView", model); //ToDo: return view 'upload not successful' or sth
            }
            using (var memoryStream = new MemoryStream())
            {
                await model.File.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                var song = new Song(model.Title, model.Author, model.Album, model.Genre, model.Year, bytes);
                _songService.SaveSong(song);
                model.Info = "Dodano";
            }
            return View("AddNewSongView", model);
        }

        private bool AllDataProvided(AddNewSongViewModel model)
        {
            String missingDataInfo = "";

            if (model.File is null || !model.File.ContentType.Contains("audio"))
            {
                missingDataInfo += "Podaj plik w formacie .mp3.";
            }
            if (model.Title is null || model.Title.Length == 0)
            {
                missingDataInfo += "Podaj tytuł.";
            }
            if (model.Genre is null || model.Genre.Length == 0)
            {
                missingDataInfo += "Podaj gatunek.";
            }
            if (model.Album is null || model.Album.Length == 0)
            {
                missingDataInfo += "Podaj album.";
            }
            if (model.Author is null || model.Author.Length == 0)
            {
                missingDataInfo += "Podaj autora.";
            }
            if (model.Year < 1000 || model.Year > 2020)
            {
                missingDataInfo += "Raczej mało prawdopodobne, że opublikowano piosenkę przed Zjazdem Gnieźnieńskim lub po 2020 roku.";
            }
            model.Info = missingDataInfo;
            return (missingDataInfo.Equals(""));
        }
    }
}