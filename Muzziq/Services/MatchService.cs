using Muzziq.Data;
using Muzziq.Models;
using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Services
{
    public interface IMatchService
    {
        void StartMatch();
        void EndMatch();
        Match CreateMatch(string roomName, ApplicationDbContext _context);
    }

    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void StartMatch()
        {
            // TODO 
            // przygotować Match do rozgrywki (poustawiać graczy czy coś)
            StartRound(0);

        }

        // TODO
        // Runda 0 odbywa się przy naciśnięciu "StartMatch"
        // każda kolejna aż do maks zdefiniowanej przy tworzeniu rozgrywki będzie trafiać bezpośrednio tutaj
        private void StartRound(int roundNumber)
        {
            Song song = GetRandomSong();
            SendSongToClients(song);

            Random rnd = new Random();
            int questionType = rnd.Next(5);

            Tuple<string, string> questionAnswerTuple = GetRandomQuestionForSong(questionType, song);
            String question = questionAnswerTuple.Item1;
            String answer = questionAnswerTuple.Item2;

            List<String> wrongAnswers = GetWrongAnswers(questionType, answer);

            GetReadyToStart();

            DisplayQuestion(question);
            DisplayAnswers(answer, wrongAnswers);

            StartPlayingSong(song.BinaryContent);

            StartCountingTimeToAnswer();

            // async (czekaj na odpowiedzi graczy)
            // przy poprawnej odpowiedzi ogłoszenie zwycięzcy i kolejna runda; przy błędzie zablokowanie gracza
        }

        private void DisplayAnswers(string correctAnswer, List<string> wrongAnswers)
        {
            // TODO 
            // wyswietlić na froncie wszystkie odpowiedzi (websocket)
            // w zasadzie nie wiem czy potrzebujemy w tym miejscu rozroznienia na poprawne i niepoprawne odpowiedzi
            // ale jak juz jest to nie zaszkodzie
        }

        private List<string> GetWrongAnswers(int questionType, string correctAnswer)
        {
            // TODO 
            // wygenerowac bledne odpowiedzi majace sens i niezawierajace poprawnej odpowiedzi

            // Propozycja ~ŁK
            // pobrać z bazy wszystkie piosenki i potem filtrować względem tytułu, autora itd
            // wybrać z przefiltrowanych 3 losowe i je umieścić w liście

            List<string> wrongAnswers = new List<string>();
            List<Song> wrongAnswersSongs = GetRandomSongs(questionType, correctAnswer, 3);

            foreach (Song song in wrongAnswersSongs)
            {
                switch (questionType)
                {
                    case 0:
                        wrongAnswers.Add(song.Title);
                        break;
                    case 1:
                        wrongAnswers.Add(song.Author);
                        break;
                    case 2:
                        wrongAnswers.Add(song.Album);
                        break;
                    case 3:
                        wrongAnswers.Add(song.Genre);
                        break;
                    case 4:
                        wrongAnswers.Add(song.Year.ToString());
                        break;
                }
            }
            return wrongAnswers;
        }

        private List<Song> GetRandomSongs(int questionType, string correctAnswer, int songsCount)
        {
            //TODO 
            // TUTAJ POJAWIA SIE PROBLEM TAKI, ZE MOGA WYLOSOWAC SIE 2 TAKIE SAME BLEDNE ODPOWIEDZI 
            List<Song> filteredSongs = GetAllSongsFromDatabase();

            switch (questionType)
            {
                case 0:
                    filteredSongs = filteredSongs.Where(song => !song.Title.Equals(correctAnswer)).ToList();
                    break;
                case 1:
                    filteredSongs = filteredSongs.Where(song => !song.Author.Equals(correctAnswer)).ToList();
                    break;
                case 2:
                    filteredSongs = filteredSongs.Where(song => !song.Album.Equals(correctAnswer)).ToList();
                    break;
                case 3:
                    filteredSongs = filteredSongs.Where(song => !song.Genre.Equals(correctAnswer)).ToList();
                    break;
                case 4:
                    filteredSongs = filteredSongs.Where(song => !song.Year.ToString().Equals(correctAnswer)).ToList();
                    break;
            }

            Random rnd = new Random();
            filteredSongs = filteredSongs.OrderBy(item => rnd.Next()).ToList();
            if (filteredSongs.Count >= songsCount)
            {
                filteredSongs = filteredSongs.GetRange(0, songsCount);
            }

            return filteredSongs;
        }

        private List<Song> GetAllSongsFromDatabase()
        {
            //TODO 
            //Zamienić na faktycznie pobieranie z bazy!
            List<Song> songs = new List<Song>();
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

            Song song3 = new Song();
            song3.Album = "Song 3";
            song3.Author = "AUTH3";
            song3.Genre = "Rock3";
            song3.Id = 3;
            song3.Title = "Song 3";
            song3.Year = 2011;

            Song song4 = new Song();
            song4.Album = "Song 4";
            song4.Author = "AUTH4";
            song4.Genre = "Rock4";
            song4.Id = 4;
            song4.Title = "Song 4";
            song4.Year = 1996;

            songs.Add(song1);
            songs.Add(song2);
            songs.Add(song3);
            songs.Add(song4);

            return songs;
        }

        private void StartPlayingSong(byte[] songBinaryContent)
        {
            // TODO 
            // Rozpocznij oddtwarzanie muzyki u klientów (websocket?)
        }

        private void StartCountingTimeToAnswer()
        {
            //TODO 
            // rozpocznij odliczanie do końca piosenki czy tam ogólny czas na odpowiedz
            // Moim zdaniem można to sobie darować ~ ŁK
        }

        private void SendSongToClients(Song song)
        {
            // TODO 
            // wyslanie piosenki do klientów (websocket)
            // Co jeśli któryś ma internet na korbkę i nie dostanie przed startem? ~ ŁK
            // To wtedy ma problem, nie możemy wstrzymywać pozostałych, było dyskutowane na konsach ~MJ
        }

        private void DisplayQuestion(string question)
        {
            // TODO
            // wysłać na front (websocket?)
        }

        private void GetReadyToStart()
        {
            // TODO
            // odliczanie 5, 4, 3, 2, 1 do wyświetlenia pytania i puszczenia muzyki
        }

        private Song GetRandomSong()
        {
            //TODO 
            // pobranie piosenki z wcześniej zdefiniowany przy tworzeniu meczu
            // (available songs w (RoomController))
            Song song = new Song();
            song.Album = "Discohłosta";
            song.Author = "Waloszek";
            song.Genre = "Disco Polo";
            song.Id = 1;
            song.Title = "Wóda zryje banie";
            song.Year = 2018;

            return song;
        }
        private Tuple<string, string> GetRandomQuestionForSong(int questionType, Song song)
        {
            string question = "";
            string answer = "";
            switch (questionType)
            {
                case 0:
                    question = "Jaki tytuł nosi ten utwór?";
                    answer = song.Title;
                    break;
                case 1:
                    question = "Podaj autora tego utworu";
                    answer = song.Author;
                    break;
                case 2:
                    question = "Z jakiego albumu pochodzi ten utwór?";
                    answer = song.Album;
                    break;
                case 3:
                    question = "Do jakiego gatunku muzycznego należy ten kawałek?";
                    answer = song.Genre;
                    break;
                case 4:
                    question = "W którym roku powstała ta piosenka?";
                    answer = song.Year.ToString();
                    break;
            }
            return Tuple.Create(question, answer);
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
            //TODO zamienić DateTime.Now na coś z sensem, a najlepiej zamienić w Match endTime na startTime xD
            return _context.Add(new Match(roomName, -1, DateTime.Now, new List<Result>())).Entity;
        }
    }
}
