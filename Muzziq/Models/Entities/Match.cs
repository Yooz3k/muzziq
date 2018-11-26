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
        public int TotalRoundsCount { get; set; }

        // FIXME nie powinien być w ogóle używany
        public Match()
        {
            // Id, RoomId, WinnerId, Question, TotalRoundsCount - ?
            Results = new List<Result>();
            StartDate = DateTime.Now;

            


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
        }
    }
}