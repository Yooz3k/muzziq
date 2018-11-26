using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Models
{
    public class Round
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public Answer CorrectAnswer { get; set; }
        public Dictionary<int, bool> UsersAnswered;
        // założenie: z serwera dostaję już pomieszaną listę odpowiedzi
        public List<Answer> Answers { get; set; }
        public int CurrentRoundNumber { get; set; }
        public Song Song { get; set; }

        public Round()
        {
            //Answer a1 = new Answer("odp 1");
            //Answer a2 = new Answer("odp 2");
            //Answer a3 = new Answer("odp 3");
            //Answer a4 = new Answer("odp 4");
            Answers = new List<Answer>();
            UsersAnswered = new Dictionary<int, bool>();
            CorrectAnswer = new Answer();
            //Answers.Add(a1);
            //Answers.Add(a2);
            //Answers.Add(a3);
            //Answers.Add(a4);
        }
    }
}
