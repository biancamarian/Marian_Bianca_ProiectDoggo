using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProiectDoggo.Models;

namespace ProiectDoggo.Data
{
    public class DbInitializer
    {
        public static void Initialize(ShopContext context)
        {
            context.Database.EnsureCreated();
            if (context.Doggoes.Any())
            {
                return; // BD a fost creata anterior
            }
            var doggoes = new Doggo[]
            {
 new Doggo{Breed="Pechinez Imperial",Color="Bej",Gender="M",Price=Decimal.Parse("2500")},
 new Doggo{Breed="Rottweiler",Color="Scortisoara",Gender="M",Price=Decimal.Parse("7000")},
 new Doggo{Breed="Doberman",Color="Cafeniu",Gender="F",Price=Decimal.Parse("8500")},
 new Doggo{Breed="Pomeranian",Color="Alb",Gender="F",Price=Decimal.Parse("5000")},
 new Doggo{Breed="Metis",Color="Diverse culori",Gender="F",Price=Decimal.Parse("100")},
            };
            foreach (Doggo d in doggoes)
            {
                context.Doggoes.Add(d);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

 new Customer{CustomerID=174321,Name="Elvira Popescu",Adress="Str. Ghioceilor nr.78, Baia Mare, Maramures", BirthDate=DateTime.Parse("1979-10-26")},
 new Customer{CustomerID=137198,Name="Vera Matei",Adress="Str. Petofi Sandor nr.2, Cluj, Cluj-Napoca", BirthDate=DateTime.Parse("1966-09-06")},
 new Customer{CustomerID=222783,Name="Nelu Sorinel",Adress="Str. Narciselor, nr.134, Baia Mare, Maramures", BirthDate=DateTime.Parse("1966-09-01")},
 new Customer{CustomerID=750291,Name="Sorina Achim",Adress="Str. Victorie, nr. 1829, Baia Mare, Maramures", BirthDate=DateTime.Parse("1997-08-03")},

 };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
 new Order{DoggoID=4,CustomerID=174321,OrderDate=DateTime.Parse("02-25-21")},
 new Order{DoggoID=3,CustomerID=137198, OrderDate=DateTime.Parse("07-04-21")},
 new Order{DoggoID=1,CustomerID=174321, OrderDate=DateTime.Parse("12-20-21")},
 new Order{DoggoID=2,CustomerID=137198, OrderDate=DateTime.Parse("10-25-21")},
 new Order{DoggoID=5,CustomerID=174321, OrderDate=DateTime.Parse("06-18-21")},
 new Order{DoggoID=5,CustomerID=222783, OrderDate=DateTime.Parse("04-30-21")},

            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();

            var kennels = new Kennel[]
            {
new Kennel{KennelName="Barbarosa", Address="Str. Mihail Sadoveanu nr.78 Somcuta, Maranmures"},
new Kennel{KennelName="Iubidubadu", Address="Str. Motilor nr. 123 Baia Mare, Maramures"},
new Kennel{KennelName="Rawf", Address="Str. Copiilor nr. 25 Baia Mare, Maramures"}
            };

            foreach(Kennel k in kennels)
            {
                context.Kennels.Add(k);
            }
            context.SaveChanges();

            var kenneldoggoes = new KennelDoggo[]
            {
new KennelDoggo{DoggoID=doggoes.Single( d => d.Breed == "Rottweiler").DoggoID, KennelID=kennels.Single(k => k.KennelName == "Barbarosa").KennelID},
new KennelDoggo{DoggoID=doggoes.Single( d => d.Breed == "Pechinez Imperial").DoggoID, KennelID=kennels.Single(k => k.KennelName == "Iubidubadu").KennelID},
new KennelDoggo{DoggoID=doggoes.Single( d => d.Breed == "Doberman").DoggoID, KennelID=kennels.Single(k => k.KennelName == "Barbarosa").KennelID},
new KennelDoggo{DoggoID=doggoes.Single( d => d.Breed == "Pomeranian").DoggoID, KennelID=kennels.Single(k => k.KennelName == "Iubidubadu").KennelID},
new KennelDoggo{DoggoID=doggoes.Single( d => d.Breed == "Metis").DoggoID, KennelID=kennels.Single(k => k.KennelName == "Rawf").KennelID},
            };

            foreach(KennelDoggo kd in kenneldoggoes)
            {
                context.KennelDoggoes.Add(kd);
            }
            context.SaveChanges();
        }
        
    }
}
