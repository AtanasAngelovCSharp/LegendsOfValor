using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LegendsOfValor_TheGuildTrials.Models.Contracts;

namespace LegendsOfValor_TheGuildTrials.Models
{
    public class Warrior : Hero
    {
        private static readonly string[] AllowedGuilds = { "WarriorGuild", "ShadowGuild" };

        public Warrior(string name, string runeMark)
            : base(name, runeMark, 60, 0, 100)
        {
        }

        public override void Train()
        {
            Power += 30;
            Stamina += 10;
        }

        public bool IsGuildAllowed(string guildName)
        {
            return AllowedGuilds.Contains(guildName);
        }
    }
}