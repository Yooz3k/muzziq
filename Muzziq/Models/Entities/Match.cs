using System;
using System.Collections.Generic;

namespace Muzziq.Models.Entities
{
    public class Match
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int ModeratorId { get; set; }
        public DateTime EndDate { get; set; }
        public List<Result> MatchResults { get; set; }
    }
}