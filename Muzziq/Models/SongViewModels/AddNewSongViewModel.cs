using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Models.SongViewModels
{
    public class AddNewSongViewModel
    {
        public String Title { get; set; }
        public String Author { get; set; }
        public String Album { get; set; }
        public String Genre { get; set; }
        public int Year { get; set; }
        public IFormFile File { get; set; }
        public String Info { get; set; }

        public AddNewSongViewModel()
        {
            this.Title = "";
            this.Author = "";
            this.Album = "";
            this.Genre = "";
            this.Year = 2018;
            this.File = null;
            this.Info = "";
        }
    }
}
