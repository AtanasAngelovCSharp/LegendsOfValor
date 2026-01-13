using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LegendsOfValor_TheGuildTrials.Models.Contracts;

namespace LegendsOfValor_TheGuildTrials.Models
{
    public class Spellblade : Hero
    {
        private static readonly string[] AllowedGuilds = { "WarriorGuild", "SorcererGuild" };

        public Spellblade(string name, string runeMark)
            : base(name, runeMark, 50, 60, 60)
        {
        }

        public override void Train()
        {
            Power += 15;
            Mana += 10;
            Stamina += 10;
        }

        public bool IsGuildAllowed(string guildName)
        {
            return AllowedGuilds.Contains(guildName);
        }
    }
}