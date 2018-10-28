using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting.Internal;

namespace Muzziq.Models.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string Nickname { get; set; }
    }
}