using Microsoft.EntityFrameworkCore;
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
        void StartMatch(int matchId);
        void EndMatch(Match match);
        Match CreateMatch(int roomId, List<Song> songs, int totalRoundsCount);
    }

    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilsService _utilsService;

        public MatchService(ApplicationDbContext context, IUtilsService utilsService)
        {
            _context = context;
            _utilsService = utilsService;
        }

        public Match CreateMatch(int roomId, List<Song> songs, int totalRoundsCount)
        {
            var room = _utilsService.GetRoomById(roomId);
            //TODO songs;
            var match = new Match(room, GetAllSongsFromDatabase(), roomId, totalRoundsCount);

           

            _context.Matches.Add(match);
            _context.SaveChanges();
            return match;
        }

        public void StartMatch(int roomId)
        {
            // TODO 
            // przygotować Match do rozgrywki (poustawiać graczy czy coś)
            var match = CreateMatch(roomId, null, 5);
            //var match = _context.Matches.Find(matchId);

            StartRound(0, match);
        }

        // TODO
        // Runda 0 odbywa się przy naciśnięciu "StartMatch"
        // każda kolejna aż do maks zdefiniowanej przy tworzeniu rozgrywki będzie trafiać bezpośrednio tutaj
        private void StartRound(int roundNumber, Match match)
        {
            Song song = GetRandomSongFromList(match.Songs);
            SendSongToClients(song);

            Random rnd = new Random();
            int questionType = rnd.Next(5);

            SetRandomQuestionForSong(match, questionType, song);

            // TODO dodać poprawną odpowiedź do Answers
            SetAnswers(questionType, match);

            GetReadyToStart();

            DisplayQuestion(match.Question);
            DisplayAnswers(match);

            StartPlayingSong(song.BinaryContent);

            StartCountingTimeToAnswer();
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
            // To jest tylko do tworzenia niepoprawnych odpowiedzi
            List<Song> songs = new List<Song>();
            Song song1 = new Song();
            song1.Album = "Discohłosta";
            song1.Author = "Waloszek";
            song1.Genre = "Disco Polo";
            song1.Title = "Wóda zryje banie";
            song1.Year = 2018;

            Song song2 = new Song();
            song2.Album = "Song 2";
            song2.Author = "Blur";
            song2.Genre = "Rock";
            song2.Title = "Song 2";
            song2.Year = 2000;

            Song song3 = new Song();
            song3.Album = "Song 3";
            song3.Author = "AUTH3";
            song3.Genre = "Rock3";
            song3.Title = "Song 3";
            song3.Year = 2011;

            Song song4 = new Song();
            song4.Album = "Song 4";
            song4.Author = "AUTH4";
            song4.Genre = "Rock4";
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

        private void DisplayAnswers(Match match)
        {
            var correctAnswer = match.CorrectAnswer;
            var answers = match.Answers;
            // TODO 
            // wyswietlić na froncie wszystkie odpowiedzi (websocket)
            // w zasadzie nie wiem czy potrzebujemy w tym miejscu rozroznienia na poprawne i niepoprawne odpowiedzi
            // ale jak juz jest to nie zaszkodzie
        }

        private void GetReadyToStart()
        {
            // TODO
            // odliczanie 5, 4, 3, 2, 1 do wyświetlenia pytania i puszczenia muzyki
        }

        private Song GetRandomSongFromList(List<Song> songs)
        {
            Random rnd = new Random();
            int r = rnd.Next(songs.Count);
            return songs[r];
        }

        private void SetRandomQuestionForSong(Match match, int questionType, Song song)
        {
            switch (questionType)
            {
                case 0:
                    match.Question = "Jaki tytuł nosi ten utwór?";
                    match.CorrectAnswer.Content = song.Title;
                    break;
                case 1:
                    match.Question = "Podaj autora tego utworu";
                    match.CorrectAnswer.Content = song.Author;
                    break;
                case 2:
                    match.Question = "Z jakiego albumu pochodzi ten utwór?";
                    match.CorrectAnswer.Content = song.Album;
                    break;
                case 3:
                    match.Question = "Do jakiego gatunku muzycznego należy ten kawałek?";
                    match.CorrectAnswer.Content = song.Genre;
                    break;
                case 4:
                    match.Question = "W którym roku powstała ta piosenka?";
                    match.CorrectAnswer.Content = song.Year.ToString();
                    break;
            }
        }

        private void SetAnswers(int questionType, Match match)
        {
            List<Song> answersSongs = GetRandomSongs(questionType, match.CorrectAnswer.Content, 3);

            foreach (Song song in answersSongs)
            {
                switch (questionType)
                {
                    case 0:
                        match.Answers.Add(new Answer(song.Title));
                        break;
                    case 1:
                        match.Answers.Add(new Answer(song.Author));
                        break;
                    case 2:
                        match.Answers.Add(new Answer(song.Album));
                        break;
                    case 3:
                        match.Answers.Add(new Answer(song.Genre));
                        break;
                    case 4:
                        match.Answers.Add(new Answer(song.Year.ToString()));
                        break;
                }
            }
        }

        public Boolean ParsePlayerAnswer(int matchId, int playerId, string playerAnswer)
        {
            var match = _utilsService.GetMatchById(matchId);
            var player = _utilsService.GetPlayerById(playerId);
            var playerWasCorrect = match.CorrectAnswer.Content == playerAnswer;
            if (playerWasCorrect)
            {
                OnCorrectAnswer(match, player);
            }
            else
            {
                OnInCorrectAnswer(match, player);
            }
            return playerWasCorrect;
        }

        public void OnCorrectAnswer(Match match, Player player)
        {
            foreach (var result in match.Results)
            {
                if (result.Player.Id == player.Id)
                {
                    result.CorrectAnswersCount++;
                    result.Points++; //TODO
                    break;
                }
            }

            EndRound(match);
        }

        public void OnInCorrectAnswer(Match match, Player player)
        {
            // TODO 
            // wyłączyć możliwość ponownego odpowiadania gracza {player} (websocket)
        }

        public void EndMatch(Match match)
        {
            Player winner = GetSortetResultList(match.Results)[0].Player;
            match.WinnerId = winner.Id;
            _context.Update(match);
            _context.SaveChanges();

            // TODO
            // ogłoś zwycięzcę
            // zaproponuj kojeną rozgrywkę w ramach pokoju
        }
        
        private void UpdateRanking(List<Result> results)
        {
            List<Result> sortedResults = GetSortetResultList(results);
            // kolejne zajęte miejsca w kolejnych miejscach listy 

            // TODO
            // wysłać na front
        }

        private List<Result> GetSortetResultList(List<Result> originalList)
        {
            List<Result> sortedResults = new List<Result>(originalList);
            sortedResults.Sort((x, y) => x.Points.CompareTo(y.Points));
            return sortedResults;
        }

        public void EndRound(Match match)
        {
            match.CurrentRoundNumber++;
            _context.Update(match);
            _context.SaveChanges();

            UpdateRanking(match.Results);

            if (match.CurrentRoundNumber == match.TotalRoundsCount)
            {
                EndMatch(match);
            }
            else
            {
                StartRound(match.CurrentRoundNumber, match);
            }
        }
    }
}
