using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting.Internal;

namespace Muzziq.Models.Entities
{
    public class Player
    {
        public Player()
        {
        }

        public Player(ApplicationUser user, string nickname)
        {
            User = user;
            Nickname = nickname;
        }

        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string Nickname { get; set; }
    }
}