using System;

namespace parrot.Models
{
    public class Pet{
        public string animal_type { get; set; }
        public DateTime created { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public Tags tags { get; set; }
    }
}