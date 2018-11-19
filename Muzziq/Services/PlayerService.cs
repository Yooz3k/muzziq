using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;

namespace Muzziq.Services
{
    public interface IPlayerService
    {
        void CreateNewPlayer(string nickname, ApplicationUser user);
    }
    public class PlayerService : IPlayerService
    {
        private readonly ApplicationDbContext _context;

        public PlayerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateNewPlayer(string nickname, ApplicationUser user)
        {
            var newPlayer = new Player(user, nickname);
            _context.Players.Add(newPlayer);
            _context.SaveChanges();
        }
    }
}