using LegendsOfValor_TheGuildTrials.Core.Contracts;
using LegendsOfValor_TheGuildTrials.Models;
using LegendsOfValor_TheGuildTrials.Models.Contracts;
using LegendsOfValor_TheGuildTrials.Repositories;
using LegendsOfValor_TheGuildTrials.Repositories.Contratcs;
using LegendsOfValor_TheGuildTrials.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IHero> heroes;
        private readonly IRepository<IGuild> guilds;

        public Controller()
        {
            heroes = new HeroRepository();
            guilds = new GuildRepository();
        }

        public string AddHero(string heroTypeName, string heroName, string runeMark)
        {
            if (heroes.GetModel(runeMark) != null)
            {
                return string.Format(OutputMessages.HeroAlreadyExists, runeMark);
            }

            IHero hero;

            switch (heroTypeName)
            {
                case "Warrior": hero = new Warrior(heroName, runeMark); break;
                case "Sorcerer": hero = new Sorcerer(heroName, runeMark); break;
                case "Spellblade": hero = new Spellblade(heroName, runeMark); break;
                default: return string.Format(OutputMessages.InvalidHeroType, heroTypeName);
            }

            heroes.AddModel(hero);
            return string.Format(OutputMessages.HeroAdded, heroTypeName, heroName, runeMark);
        }

        public string CreateGuild(string guildName)
        {
            if (guilds.GetModel(guildName) != null)
            {
                return string.Format(OutputMessages.GuildAlreadyExists, guildName);
            }

            IGuild guild = new Guild(guildName);
            guilds.AddModel(guild);
            return string.Format(OutputMessages.GuildCreated, guildName);
        }

        public string RecruitHero(string runeMark, string guildName)
        {
            IHero hero = heroes.GetModel(runeMark);
            if (hero == null)
                return string.Format(OutputMessages.HeroNotFound, runeMark);

            IGuild guild = guilds.GetModel(guildName);
            if (guild == null)
                return string.Format(OutputMessages.GuildNotFound, guildName);

            if (!string.IsNullOrEmpty(hero.GuildName))
                return string.Format(OutputMessages.HeroAlreadyInGuild, hero.Name);

            if (guild.IsFallen)
                return string.Format(OutputMessages.GuildIsFallen, guildName);

            if (guild.Wealth < 500)
                return string.Format(OutputMessages.GuildCannotAffordRecruitment, guildName);

            if (!IsHeroCompatibleWithGuild(hero, guild))
                return string.Format(OutputMessages.HeroTypeNotCompatible, hero.GetType().Name, guildName);

            guild.RecruitHero(hero);
            return string.Format(OutputMessages.HeroRecruited, hero.Name, guildName);
        }

        public string TrainingDay(string guildName)
        {
            IGuild guild = guilds.GetModel(guildName);
            if (guild == null)
                return string.Format(OutputMessages.GuildNotFound, guildName);

            if (guild.IsFallen)
                return string.Format(OutputMessages.GuildTrainingDayIsFallen, guildName);

            var heroesInGuild = heroes.GetAll().Where(h => h.GuildName == guildName).ToList();
            int totalTrainingCost = heroesInGuild.Count * 200;

            if (guild.Wealth < totalTrainingCost)
                return string.Format(OutputMessages.TrainingDayFailed, guildName);

            guild.TrainLegion(heroesInGuild);
            return string.Format(OutputMessages.TrainingDayStarted, guildName, heroesInGuild.Count, totalTrainingCost);
        }

        public string StartWar(string attackerGuildName, string defenderGuildName)
        {
            IGuild attacker = guilds.GetModel(attackerGuildName);
            IGuild defender = guilds.GetModel(defenderGuildName);

            if (attacker == null || defender == null)
                return OutputMessages.OneOfTheGuildsDoesNotExist;

            if (attacker.IsFallen || defender.IsFallen)
                return OutputMessages.OneOfTheGuildsIsFallen;

            int attackerStrength = GetGuildStrength(attacker);
            int defenderStrength = GetGuildStrength(defender);
            int goldAmount = defender.Wealth;

            if (attackerStrength > defenderStrength)
            {
                attacker.WinWar(goldAmount);
                defender.LoseWar();
                return string.Format(OutputMessages.WarWon, attackerGuildName, defenderGuildName, goldAmount);
            }
            else
            {
                defender.WinWar(goldAmount);
                attacker.LoseWar();
                return string.Format(OutputMessages.WarLost, defenderGuildName, attackerGuildName, goldAmount);
            }
        }

        public string ValorState()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Valor State:");

            var orderedGuilds = guilds.GetAll().OrderByDescending(g => g.Wealth).ToList();

            foreach (var guild in orderedGuilds)
            {
                sb.AppendLine($"{guild.Name} (Wealth: {guild.Wealth})");

                var heroesInGuild = heroes.GetAll()
                    .Where(h => h.GuildName == guild.Name)
                    .OrderBy(h => h.Name)
                    .ToList();

                foreach (var hero in heroesInGuild)
                {
                    sb.AppendLine($"-Hero: [{hero.Name}] of the Guild '{hero.GuildName}' - RuneMark: {hero.RuneMark}");
                    sb.AppendLine($"--{hero.Essence()}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        private bool IsHeroCompatibleWithGuild(IHero hero, IGuild guild)
        {
            string heroType = hero.GetType().Name;

            return (heroType == "Warrior" && (guild.Name == "WarriorGuild" || guild.Name == "ShadowGuild")) ||
                   (heroType == "Sorcerer" && (guild.Name == "SorcererGuild" || guild.Name == "ShadowGuild")) ||
                   (heroType == "Spellblade" && (guild.Name == "WarriorGuild" || guild.Name == "SorcererGuild"));
        }

        private int GetGuildStrength(IGuild guild)
        {
            return heroes.GetAll()
                .Where(h => h.GuildName == guild.Name)
                .Sum(h => h.Power + h.Mana + h.Stamina);
        }
    }
}

