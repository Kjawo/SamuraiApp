using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class Battle
    {
        public int BattleID { get; set; }
        public string Name { get; set; }
        public List<Samurai> Samurais { get; set; }
    }
}
