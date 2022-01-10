using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProiectDoggo.Models
{
    public class KennelDoggo
    {
        public int KennelID { get; set; }
        public int DoggoID { get; set; }
        public Kennel Kennel { get; set; }
        public Doggo Doggo { get; set; }
    }
}
