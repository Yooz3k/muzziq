using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;
using Muzziq.Models.RoomViewModels;
using Muzziq.Services;

namespace Muzziq.Controllers
{
  
    public class RoomController : Controller
    {
        // listy na potrzeby testów interfejsu, docelowo te obiekty nie będą trzymane w listach, ale w bazie danych
        // piosenki już w bazie danych
        private List<Match> availableMatches;

        private readonly RoomService roomService;

        private readonly ApplicationDbContext _context;
        public RoomController(ApplicationDbContext context)
        {
            _context = context;
            roomService = new RoomService(_context, new MatchService(_context, new UtilsService(_context)), new UtilsService(_context));

            availableMatches = new List<Match>();

            Match match1 = new Match();
            match1.Id = 1;
            //match1.RoomName = "Pieniężnianie";

            Match match2 = new Match();
            match2.Id = 2;
            //match2.RoomName = "JOLKA ONLY";

            availableMatches.Add(match1);
            availableMatches.Add(match2);
        }

       

        public IActionResult ChooseRoomView()
        {
            Room room1 = new Room();
            room1.Name = "ekipa z zielonego jeepa";
            room1.Players = new List<Player>();
            room1.Players.Add(new Player(null, "1"));
            room1.Players.Add(new Player(null, "2"));

            Room room2 = new Room();
            room2.Name = "weź pigułke";
            room2.Players = new List<Player>();
            room2.Players.Add(new Player(null, "3"));
            room2.Players.Add(new Player(null, "4"));

            List<Room> rooms = new List<Room>
            {
                room1,
                room2
            };

            rooms = _context.Rooms.ToList();
            rooms.ForEach((room) => _context.Entry(room).Collection(s => s.Players).Load());
            ViewData["Rooms"] = rooms;
            //ViewData["Rooms"] = rooms; ^--- DOCELOWO TO MA BYĆ

            return View();
        }

        public IActionResult CreateRoomView()
        {
            // TOOD zwróć listę dostępnych piosenek
            CreateRoomViewModel model = new CreateRoomViewModel(_context.Songs.ToList());
            
            return View(model);
        }

        public IActionResult WaitForGameView(int roomID)
        {
            // TODO zwróć nazwę pokoju oraz listę graczy
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

            // TODO przechwycenie danych z formularza - nazwa pokoju, wybrane utwory
            // ownerId też musi być przekazywany z frontu (po stworzeniu formularza do tworzenia graczy i ich poprawnego logowania)
            // ownerId to będzie ten co kliknął przycisk "Utwórz pokój"

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
            return RedirectToAction("WaitForGameView", new
            {
                roomID = room.Id
            });
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

            return RedirectToAction("WaitForGameView", new
            {
                roomID = roomId
            });
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}