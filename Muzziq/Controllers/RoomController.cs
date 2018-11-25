using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;
using Muzziq.Models.MatchViewModels;
using Muzziq.Models.RoomViewModels;
using Muzziq.Services;

namespace Muzziq.Controllers
{

    public class RoomController : Controller
    {
        private readonly RoomService roomService;

        private readonly ApplicationDbContext _context;
        public RoomController(ApplicationDbContext context)
        {
            _context = context;
            roomService = new RoomService(_context, new MatchService(_context, new UtilsService(_context)), new UtilsService(_context));
        }
               
        public IActionResult ChooseRoomView()
        {
            List<Room> rooms = _context.Rooms.ToList();
            rooms.ForEach((room) => _context.Entry(room).Collection(s => s.Players).Load());

            ChooseRoomViewModel chooseRoomViewModel = new ChooseRoomViewModel
            {
                Rooms = rooms,
                RoomCapacity = 6 //TODO: jako stała
            };

            return View(chooseRoomViewModel);
        }

        public IActionResult CreateRoomView()
        {
            CreateRoomViewModel model = new CreateRoomViewModel(_context.Songs.ToList());

            return View(model);
        }

        public IActionResult WaitForGameView(int roomID)
        {
            Room room = _context.Rooms.Find(roomID);
            if (room != null)
            {
                _context.Entry(room).Collection(s => s.Players).Load();
            }


            return View(new WaitForGameViewModel(room, 6));
        }

        [HttpPost]
        public IActionResult CreateRoom(String name, int[] songIds)
        {
            //Dane z formularza: name to nazwa pokoju, songIds to id-ki utworów

            // ownerId też musi być przekazywany z frontu (po stworzeniu formularza do tworzenia graczy i ich poprawnego logowania)
            // ownerId to będzie ten co kliknął przycisk "Utwórz pokój"

            //^
            //|
            //ID gracza nie jest przechwytywane we froncie, tylko tutaj.
            //Jeżeli nawigacja przejdzie do tej metody, to mamy gracza, który stworzył pokój.
            int playerId = GetPlayerId();

            Room room = _context.Rooms.Where(x => x.Name == name).FirstOrDefault();

            List<Player> players = _context.Players.ToList();
            players.Sort((ply1, ply2) => ply1.Id >= ply2.Id ? -1 : 1);
            Player player2 = players.FirstOrDefault();
            Player player = new Player(new ApplicationUser(), "User" + (player2?.Id + 1).ToString());
            _context.Add(player);
            _context.SaveChanges();
            if (room == null) //to taki mock, jeżeli wpiszesz to samo id to dołączasz do pokoju
                room = roomService.CreateRoom(player.Id, name, songIds);

            _context.SaveChanges();

            WaitForGameViewModel waitForGameView = new WaitForGameViewModel(room, 6, player.Id); //TODO: 6 powinno lecieć z góry

            return View("WaitForGameView", waitForGameView);
        }

        public IActionResult AddNewSong()
        {
            // TODO przechwycenie piosenki

            // dodanie piosenki

            // odświeżenie widoku - piosenka pojawi się na liście piosenek

            return null;
        }

        public IActionResult JoinRoom(int roomId, int playerId)
        {
            roomService.JoinRoom(roomId, playerId);

            Room room = new UtilsService(_context).GetRoomById(roomId);

            return View("WaitForGameView", new WaitForGameViewModel(room));
        }

        public IActionResult Test()
        {
            return View();
        }

        private int GetPlayerId()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return new UtilsService(_context).getPlayerByUserId(userId);
        }
    }
}