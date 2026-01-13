using LegendsOfValor_TheGuildTrials.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LegendsOfValor_TheGuildTrials.Models
{
    public abstract class Hero : IHero
    {
        private string name;
        private string runeMark;

        protected Hero(string name, string runeMark, int power, int mana, int stamina)
        {
            Name = name;
            RuneMark = runeMark;
            Power = power;
            Mana = mana;
            Stamina = stamina;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("A nameless soul cannot take form in this realm.");
                name = value;
            }
        }

        public string RuneMark
        {
            get => runeMark;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("A hero cannot awaken without a RuneMark etched in fate.");
                runeMark = value;
            }
        }

        public string GuildName { get; private set; }

        public int Power { get; protected set; }

        public int Mana { get; protected set; }

        public int Stamina { get; protected set; }

        public void JoinGuild(IGuild guild)
        {
            GuildName = guild.Name;
        }

        public abstract void Train();

        public string Essence()
        {
            return $"Essence Revealed - Power [{Power}] Mana [{Mana}] Stamina [{Stamina}]";
        }

        public override string ToString()
        {
            return $"Hero: [{Name}] of the Guild '{GuildName}' - RuneMark: {RuneMark}";
        }
    }
}