using System;
using System.Collections.Generic;

namespace Muzziq.Models.Entities
{
    public class Match
    {


        public int Id { get; set; }
        public int RoomId { get; set; }
        public int WinnerId { get; set; }
        public DateTime StartDate { get; set; }
        public List<Song> Songs { get; set; }
        public List<Result> Results { get; set; }
        public string Question { get; set; }
        public Answer CorrectAnswer { get; set; }
        public Dictionary<int, bool> UsersAnswered;
        // założenie: z serwera dostaję już pomieszaną listę odpowiedzi
        public List<Answer> Answers { get; set; }
        public int TotalRoundsCount { get; set; }
        public int CurrentRoundNumber { get; set; }

        // FIXME nie powinien być w ogóle używany
        public Match()
        {
            // Id, RoomId, WinnerId, Question, TotalRoundsCount - ?
            Results = new List<Result>();
            StartDate = DateTime.Now;
            CorrectAnswer = new Answer();
            Answers = new List<Answer>();
            UsersAnswered = new Dictionary<int, bool>();
            Songs = new List<Song>();
            CurrentRoundNumber = 0;

            
            Question = "";
            Answer a1 = new Answer("odp 1");
            Answer a2 = new Answer("odp 2");
            Answer a3 = new Answer("odp 3");
            Answer a4 = new Answer("odp 4");
            Answers.Add(a1);
            Answers.Add(a2);
            Answers.Add(a3);
            Answers.Add(a4);


        }
        public Match(Room room, List<Song> songs, int roomId, int totalRoundsCount)
        {
            var results = new List<Result>();
            foreach (var player in room.Players)
            {
                results.Add(new Result(player, this, 0, 0));
            }
            Results = results;
            Songs = songs;
            RoomId = roomId;
            TotalRoundsCount = totalRoundsCount;
            StartDate = DateTime.Now;
            CorrectAnswer = new Answer();
            Answers = new List<Answer>();
            UsersAnswered = new Dictionary<int, bool>();
            CurrentRoundNumber = 0;
        }
    }
}