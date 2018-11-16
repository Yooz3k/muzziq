using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Muzziq.Controllers
{
  
    public class RoomController : Controller
    {
        public IActionResult ChooseRoom()
        {
            return View();
        }

        public IActionResult CreateRoom()
        {
            return View();
        }

        public IActionResult WaitForGamePlayer()
        {
            return View();
        }

        public IActionResult WaitForGameUser()
        {
            return View();
        }
    }
}