namespace Muzziq.Models.Entities
{
    public class Result
    {
        public Result()
        {
        }

        public Result(Player player, Match match, int correctAnswersCount, int points)
        {
            Player = player;
            Match = match;
            CorrectAnswersCount = correctAnswersCount;
            Points = points;
        }

        public int Id { get; set; }
        public Player Player { get; set; }
        public Match Match { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int Points { get; set; }
    }
}