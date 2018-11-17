using System;
using System.Collections.Generic;

namespace Muzziq.Models.Entities
{
    public class Match
    {
        public Match(string roomName, int winnerId, DateTime endDate, List<Result> matchResults)
        {
            RoomName = roomName;
            WinnerId = winnerId;
            EndDate = endDate;
            MatchResults = matchResults;
        }

        public int Id { get; set; }
        public string RoomName { get; set; }
        public int WinnerId { get; set; }
        public DateTime EndDate { get; set; }
        public List<Result> MatchResults { get; set; }
    }
}