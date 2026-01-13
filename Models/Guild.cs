using LegendsOfValor_TheGuildTrials.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Models
{
    public class Guild : IGuild
    {
        private readonly List<string> legion;
        private static readonly string[] AllowedGuilds = { "WarriorGuild", "SorcererGuild", "ShadowGuild" };
        private int wealth;
        private bool isFallen;

        public Guild(string name)
        {
            if (!AllowedGuilds.Contains(name))
            {
                throw new ArgumentException("Unknown guilds hold no place in this realm.");
            }

            Name = name;
            wealth = 5000;
            legion = new List<string>();
            isFallen = false;
        }

        public string Name { get; }

        public int Wealth
        {
            get => wealth;
            private set => wealth = value < 0 ? 0 : value;
        }

        public IReadOnlyCollection<string> Legion => legion.AsReadOnly();

        public bool IsFallen => isFallen;

        public void RecruitHero(IHero hero)
        {
            if (IsFallen) return;

            if (!legion.Contains(hero.RuneMark))
            {
                legion.Add(hero.RuneMark);
                Wealth -= 500;
                hero.JoinGuild(this);
            }
        }

        public void TrainLegion(ICollection<IHero> heroesToTrain)
        {
            if (IsFallen) return;

            foreach (var hero in heroesToTrain)
            {
                Wealth -= 200;
                hero.Train();
            }
        }

        public void WinWar(int goldAmount)
        {
            if (IsFallen) return;
            Wealth += goldAmount;
        }

        public void LoseWar()
        {
            isFallen = true;
            Wealth = 0;
        }
    }
}