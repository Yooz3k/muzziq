using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Muzziq.Controllers
{
    public class MatchController : Controller
    {
        public IActionResult Match
            ()
        {
            return View();
        }

        public IActionResult MatchSummary()
        {
            return View();
        }
    }
}