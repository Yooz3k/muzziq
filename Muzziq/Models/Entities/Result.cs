namespace Muzziq.Models.Entities
{
    public class Result
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public Match Match { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int Points { get; set; }
    }
}