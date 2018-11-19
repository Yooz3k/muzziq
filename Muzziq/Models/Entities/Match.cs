using System;
using System.Collections.Generic;

namespace Muzziq.Models.Entities
{
    public class Match
    {
        public Match()
        {
        }


        public int Id { get; set; }
        public int RoomId { get; set; }
        public int WinnerId { get; set; }
        public DateTime StartDate { get; set; }
        public List<Song> Songs { get; set; }
        public List<Result> Results { get; set; }
        public string Question { get; set; }
        public Answer CorrectAnswer { get; set; }
        public List<Answer> WrongAnswers { get; set; }
        public int TotalRoundsCount { get; set; }
        public int CurrentRoundNumber { get; set; }

    }
}