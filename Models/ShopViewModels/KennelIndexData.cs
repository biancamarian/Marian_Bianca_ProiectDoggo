using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProiectDoggo.Models.ShopViewModels
{
    public class KennelIndexData
    {
        public IEnumerable<Kennel> Kennels { get; set; }
        public IEnumerable<Doggo> Doggoes { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
