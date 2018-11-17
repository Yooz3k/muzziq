using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Services
{
    public class MatchService
    {
        private readonly ApplicationDbContext _context;
        public void StartMatch()
        {
            // TODO 
            // wylosuj np 5 piosenek i pytania
            // rozpocznij puszczanie muzyki
            // wystartuj czasomierz
            // async (czekaj na odpowiedzi graczy)
            // przy poprawnej odpowiedzi kolejna piosenka albo zablokowanie gracza, który odpowiedział błędnie
        }

        public void EndMatch()
        {
            // TODO
            // wyznacz zwycięzcę meczu
            // pokaż ranking
            // zaproponuj kojeną rozgrywkę w ramach pokoju
        }

        public Match CreateMatch(string roomName, ApplicationDbContext _context)
        {
           return new Match(roomName, 0, DateTime.Now, new List<Result>());
        }
    }
}
