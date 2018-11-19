using System;
using System.Collections.Generic;

namespace Muzziq.Models.Entities
{
    //To musiało powstać, bo EF nie jest w stanie przechowywać List<string> w bazie
    public class Answer
    {
        public Answer()
        {
        }

        public Answer(string content)
        {
            Content = content;
        }

        public int Id { get; set; }
        public string Content { get; set; }
    }
}