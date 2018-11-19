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
        private WaitForGameModel waitForGameModel;

        private readonly RoomService roomService;

        private readonly ApplicationDbContext _context;
        public RoomController(ApplicationDbContext context)
        {
            _context = context;
            roomService = new RoomService(_context);

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

        public IActionResult WaitForGameView(Room room)
        {
            // TODO zwróć nazwę pokoju oraz listę graczy
            
            
            waitForGameModel = new WaitForGameModel(room.Name, room.Players);

            return View(waitForGameModel);
        }

        [HttpPost]
        public IActionResult CreateRoom(String name, int[] songIds)
        {

            // TODO przechwycenie danych z formularza - nazwa pokoju, wybrane utwory
            // ownerId też musi być przekazywany z frontu (po stworzeniu formularza do tworzenia graczy i ich poprawnego logowania)
            // ownerId to będzie ten co kliknął przycisk "Utwórz pokój"

            //jak już będzie się dało po ludzku dolączyć do gry to trzeba będzie co nieco pozmieniać tutaj
            Room[] room = _context.Rooms.Where(x => x.Name == name).ToArray();

            if(room[0] != null) //to taki mock, jeżeli wpiszesz to samo id to dołączasz do pokoju
                room[0] = roomService.CreateRoom(1, name, songIds, _context);

            List<Player> players = _context.Players.ToList();
            players.Sort();
            Player player2 = players.Last();
            Player player = new Player(new ApplicationUser(), "User" + (player2.Id + 1).ToString());
            _context.Add(player);
            players.Add(player);
            room[0].Players = players;
            room[0] = _context.Update(room[0]).Entity;
            // przekierowanie do WaitForGameView()

            return View("WaitForGameView", room);
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
            roomService.JoinRoom(roomId, playerId, _context);
            // przekierowanie do pokoju

            return View("WaitForGameView");
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}