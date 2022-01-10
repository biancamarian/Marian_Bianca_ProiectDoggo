using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDoggo.Models
{
    public class Doggo
    {
        public int DoggoID { get; set; }
        public string Breed { get; set; }
        public string Color{ get; set; }
        public string Gender { get; set; }
        [Column(TypeName ="decimal(10,2)")]
        public decimal Price { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<KennelDoggo> KennelDoggoes { get; set; }
    }
}
