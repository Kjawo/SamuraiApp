using System;
using System.Collections.Generic;

namespace SamuraiApp.Domain
{
    public class Battle
    {
        public int BattleID { get; set; }
        public string Name { get; set; }
        public List<Samurai> Samurais { get; set; } = new List<Samurai>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}