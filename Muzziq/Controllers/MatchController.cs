using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Muzziq.Controllers
{
    public class MatchController : Controller
    {
        public IActionResult MatchView()
        {
            // TODO dużo rzeczy

            return View();
        }

        public IActionResult StartMatch()
        {
            // TODO przechwycenie, jaki mecz jest tworzony

            // utworzenie meczu

            // rozgrywka

            return View("MatchView");
        }

        public IActionResult EndMatch()
        {
            // TODO zapisanie rozgrywki

            // TODO zwrócenie listy wyników

            return View("MatchSummaryView");
        }
    }
}