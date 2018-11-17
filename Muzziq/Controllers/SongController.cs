using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Muzziq.Controllers
{
    public class SongController : Controller
    {
        public IActionResult AddNewSongView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewSong(String title, String author, String album, String genre, String year)
        {
            // TODO logika dodania nowej piosenki do bazy danych

            return View("AddNewSongView");
        }
    }
}