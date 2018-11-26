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
        private readonly WSService wsService;
        private List<Song> availableSongs;
        private List<Match> availableMatches;
        private CreateRoomViewModel createRoomViewModel;
        private readonly ApplicationDbContext _context;
        public RoomController(ApplicationDbContext context)
        {
            _context = context;
            roomService = new RoomService(_context, new MatchService(_context, new UtilsService(_context), new SongService(_context)), new UtilsService(_context));
            wsService = new WSService( _context);
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
            //match1.RoomName = "Pieniężnianie";

            Match match2 = new Match();
            match2.Id = 2;
            //match2.RoomName = "JOLKA ONLY";

            availableMatches.Add(match1);
            availableMatches.Add(match2);

            createRoomViewModel = new CreateRoomViewModel(availableSongs);
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

        public IActionResult WaitForGameView(int roomID, int playerID)
        {
            Room room = _context.Rooms.Find(roomID);
            if (room != null)
            {
                _context.Entry(room).Collection(s => s.Players).Load();
            }


            return View(new WaitForGameViewModel(room,6,playerID)); 
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

            Player player = new Player(new ApplicationUser(), "User");
            _context.Add(player);
            _context.SaveChanges();
            if (room == null) //to taki mock, jeżeli zdupikujesz name pokoju
                room = roomService.CreateRoom(player.Id, name, songIds);

            player.Nickname = player.Nickname + player.Id.ToString();
            _context.Update(player);
            _context.SaveChanges();
            return RedirectToAction("WaitForGameView", new
            {
                roomID = room.Id,
                playerID = player.Id
            });
        }

        public IActionResult AddNewSong()
        {
            // TODO przechwycenie piosenki

            // dodanie piosenki

            // odświeżenie widoku - piosenka pojawi się na liście piosenek

            return null;
        }

        public IActionResult JoinRoom(int roomId)
        {
            //do testów bo na razie widoki nie zwracają id_playerów
            Player player = _context.Players.Find(GetPlayerId());
            roomService.JoinRoom(roomId, player.Id);
            //poinformuj reszte graczy
            WSMessage message = new WSMessage(WSMessageType.PLAYER_JOIN, player.Id.ToString() + " " + player.Nickname);
            wsService.SendToList(roomId, message);
            return RedirectToAction("WaitForGameView", new
            {
                roomID = roomId,
                playerID = player.Id
            });
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