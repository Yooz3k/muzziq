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
        private List<Song> availableSongs;
        private List<Match> availableMatches;
        private CreateRoomViewModel createRoomViewModel;

        private readonly IRoomService _roomService;

        private readonly ApplicationDbContext _context;
        public RoomController(ApplicationDbContext context, IRoomService roomService)
        {
            _context = context;
            _roomService = roomService;

            availableSongs = new List<Song>();
            availableMatches = new List<Match>();

            Song song1 = new Song();
            song1.Album = "Discohłosta";
            song1.Author = "Waloszek";
            song1.Genre = "Disco Polo";
            song1.Id = 1;
            song1.Title = "Wóda zryje banie";
            song1.Year = 2018;

            Song song2 = new Song();
            song2.Album = "Song 2";
            song2.Author = "Blur";
            song2.Genre = "Rock";
            song2.Id = 2;
            song2.Title = "Song 2";
            song2.Year = 2000;

            availableSongs.Add(song1);
            availableSongs.Add(song2);

            Match match1 = new Match();
            match1.Id = 1;
            match1.RoomName = "Pieniężnianie";

            Match match2 = new Match();
            match2.Id = 2;
            match2.RoomName = "JOLKA ONLY";

            availableMatches.Add(match1);
            availableMatches.Add(match2);

            createRoomViewModel = new CreateRoomViewModel(availableSongs);
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

            //ViewData["Rooms"] = _context.Rooms; <--- DOCELOWO TO MA BYĆ
            ViewData["Rooms"] = rooms;

            return View();
        }

        public IActionResult CreateRoomView()
        {
            // TOOD zwróć listę dostępnych piosenek
            
            return View(createRoomViewModel);
        }

        public IActionResult WaitForGameView()
        {
            // TODO zwróć nazwę pokoju oraz listę graczy

            return View();
        }

        [HttpPost]
        public IActionResult CreateRoom(String name, int[] songIds)
        {
            // TODO przechwycenie danych z formularza - nazwa pokoju, wybrane utwory
            // ownerId też musi być przekazywany z frontu (po stworzeniu formularza do tworzenia graczy i ich poprawnego logowania)
            // ownerId to będzie ten co kliknął przycisk "Utwórz pokój"
            int ownerId = 1;
            _roomService.CreateRoom(ownerId, name, songIds);

            // przekierowanie do WaitForGameView()

            return View("WaitForGameView");
        }

        public IActionResult AddNewSong()
        {
            // TODO przechwycenie piosenki

            // dodanie piosenki

            // odświeżenie widoku - piosenka pojawi się na liście piosenek

            return null;
        }

        public IActionResult JoinRoom()
        {
            // TODO przechwycenie ID pokoju i id gracza
            int playerId = 1;
            int roomId = 2;
            _roomService.JoinRoom(roomId, playerId);
            // przekierowanie do pokoju

            return View("WaitForGameView");
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}