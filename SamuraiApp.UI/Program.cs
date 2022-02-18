using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
    internal class Program
    {
        private static readonly SamuraiContext _context = new();
        private static readonly SamuraiContextNoTracking _contextNT = new();

        private static void Main(string[] args)
        {
            #region InteractingWithEFCoreDataModel
            // _context.Database.EnsureCreated();
            // GetSamurais("Before Add:");
            // AddSamurai();
            // GetSamurais("After Add:");
            // AddVariousTypes();
            // QueryFilters();
            // QueryAggregates();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            // MultipleDatabaseOperations();
            // RetrieveAndDeleteASamurai();
            //QueryAndUpdateBattles_Disconnected();
            #endregion

            #region InteractingWithRelatedData
            //InsertNewSamuraiWithAQuote();
            //Simpler_AddQuoteToExistingSamuraiNotTracked(1);
            //EagerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //ProjectSamuraisWithQuotes();
            ExplicitLoadQuotes();
            #endregion

            Console.Write("Press any key...");
            Console.ReadKey();
        }

        #region InteractingWithEFCoreDataModel
        private static void AddVariousTypes()
        {
            _context.AddRange(new Samurai { Name = "Shimada" },
                    new Samurai { Name = "Okamoto" },
                    new Battle { Name = "Battle of Anegawa" },
                    new Battle { Name = "Battle of Nagashino" });

            _context.SaveChanges();
        }

        private static void AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }
        private static void AddSamurais(Samurai[] samurais)
        {
            //AddRange can take an array or an IEnumerable e.g. List<Samurai>
            _context.Samurais.AddRange(samurais);
            _context.SaveChanges();
        }
        private static void GetSamurais()
        {
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void QueryFilters()
        {
            // var name = "Sampson";
            // var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            var filter = "J%";
            var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, filter)).ToList();
        }

        private static void QueryAggregates()
        {
            // var name = "Sampson";
            // var samurais = _context.Samurais.FirstOrDefault(s => s.Name == name);
            var samurais = _context.Samurais.Find(2);
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }
        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }
        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }

        private static void RetrieveAndDeleteASamurai()
        {
            var samurai = _context.Samurais.Find(3);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();
            } //context1 is disposed
            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 1);
            });
            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }
        #endregion

        #region InteractingWithRelatedData
        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kamebi Shimada",
                Quotes = new List<Quote>
                {
                    new Quote {Text = "I've come to save you"}
                }
            };

            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            //var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
            //var splitQuery = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
            //    .Include(s => s.Quotes.Where(q=>q.Text.Contains("Thanks"))).ToList();
            var filterPrimaryEntityWithInclude =
                _context.Samurais.Where(s => s.Name.Contains("Sampson"))
                        .Include(s => s.Quotes).FirstOrDefault();

        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            var idAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
        }
        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;

        }

        private static void ProjectSamuraisWithQuotes()
        {
            //var somePropsWithQuotes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, NumberOfQuotes = s.Quotes.Count })
            //    .ToList();
            //var somePropsWithQuotes = _context.Samurais
            //.Select(s => new { s.Id, s.Name, 
            //                   HappyQuotes = s.Quotes.Where(q=>q.Text.Contains("happy")) })
            //.ToList();
            var samuraisAndQuotes = _context.Samurais
            .Select(s => new {
                Samurai = s,
                HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
            })
            .ToList();
            var firstSamurai = samuraisAndQuotes[0].Samurai.Name += " The Happiest";
        }

        private static void ExplicitLoadQuotes()
        {
            //make sure there's a horse in the DB, then clear the context's change tracker
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            //-------------------
            var samurai = _context.Samurais.Find(1);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }
        #endregion
    }
}