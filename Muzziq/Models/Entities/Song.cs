using System;
using System.ComponentModel.DataAnnotations;

namespace Muzziq.Models.Entities
{
    [Serializable]
    public class Song
    {
        public Song(string title, string author, string album, string genre, int year, byte[] binaryContent)
        {
            Title = title;
            Author = author;
            Album = album;
            Genre = genre;
            Year = year;
            BinaryContent = binaryContent;
        }

        public Song()
        {
        }

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Author { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public byte[] BinaryContent { get; set; }
    }
}