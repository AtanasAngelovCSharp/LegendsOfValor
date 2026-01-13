using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LegendsOfValor_TheGuildTrials.Models.Contracts;

namespace LegendsOfValor_TheGuildTrials.Models
{
    public class Sorcerer : Hero
    {
        private static readonly string[] AllowedGuilds = { "SorcererGuild", "ShadowGuild" };

        public Sorcerer(string name, string runeMark)
            : base(name, runeMark, 40, 120, 0)
        {
        }

        public override void Train()
        {
            Power += 20;
            Mana += 25;
        }

        public bool IsGuildAllowed(string guildName)
        {
            return AllowedGuilds.Contains(guildName);
        }
    }
}