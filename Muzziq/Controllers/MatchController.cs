using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Muzziq.Data;
using Muzziq.Models.Entities;
using Muzziq.Services;

namespace Muzziq.Controllers
{
    public class MatchController : Controller
    {

        private readonly MatchService matchService;
        private readonly ApplicationDbContext _context;
        public MatchController(ApplicationDbContext context)
        {
            _context = context;
            matchService = new MatchService(_context);
        }

        public IActionResult MatchView()
        {
            // TODO dużo rzeczy

            return View();
        }

        public IActionResult StartMatch()
        {
            // TODO przechwycenie, jaki mecz jest tworzony
            // matchService.CreateMatch("Pokoj zwierzeń", _context);
            matchService.StartMatch();
            // utworzenie meczu

            // rozgrywka

            return View("MatchView");
        }

        public IActionResult EndMatch()
        {
            matchService.EndMatch();
            // TODO zapisanie rozgrywki

            // TODO zwrócenie listy wyników

            return View("MatchSummaryView");
        }
    }
}