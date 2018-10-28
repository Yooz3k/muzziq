using System.ComponentModel.DataAnnotations;

namespace Muzziq.Models.Entities
{
    public class Song
    {
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