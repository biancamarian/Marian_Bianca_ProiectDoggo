using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProiectDoggo.Models
{
    public class Kennel
    {
        public int KennelID { get; set; }
        [Required]
        [Display(Name = "Nume Canisa")]
        [StringLength(50)]
        public string KennelName { get; set; }
        [StringLength(70)]
        public string Address { get; set; }

        public ICollection<KennelDoggo> KennelDogs { get; set; }
    }
}
