using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
    internal class Program
    {
        private static readonly SamuraiContext _context = new();

        private static void Main(string[] args)
        {
            // _context.Database.EnsureCreated();
            // GetSamurais("Before Add:");
            // AddSamurai();
            // GetSamurais("After Add:");
            // AddVariousTypes();
            QueryFilters();

            Console.Write("Press any key...");
            Console.ReadKey();
        }

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
    }
}