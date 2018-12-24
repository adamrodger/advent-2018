namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using MoreLinq;

    /// <summary>
    /// Solver for Day 24
    /// </summary>
    public class Day24
    {
        public int Part1(string[] input)
        {
            var survivors = this.Fight(input);
            return survivors.Sum(s => s.Units);
        }

        public int Part2(string[] input)
        {
            int boost = 0;

            while (true)
            {
                var survivors = this.Fight(input, boost);

                if (survivors.Any() && survivors.All(s => s.Team == Team.Immune))
                {
                    return survivors.Sum(s => s.Units);
                }

                boost++;
            }
        }

        private ICollection<Group> Fight(string[] input, int boost = 0)
        {
            var parts = input.Split(string.Empty).ToArray();
            var immuneTeam = parts[0].Skip(1).Select(s => new Group(Team.Immune, s, boost)).ToList();
            var infectionTeam = parts[1].Skip(1).Select(s => new Group(Team.Infection, s)).ToList();

            while (immuneTeam.Any() && infectionTeam.Any())
            {
                var everyone = infectionTeam.Union(immuneTeam)
                                            .OrderByDescending(t => (t.EffectivePower, t.Initiative))
                                            .ToList();
                var matches = new Dictionary<Group, Group>();
                var unmatched = everyone.ToHashSet();

                // pick targets
                foreach (Group attacker in everyone)
                {
                    var opponent = unmatched.Where(u => u.Team != attacker.Team)
                                            .Where(u => attacker.PotentialDamage(u) > 0)
                                            .MaxBy(u => (attacker.PotentialDamage(u), u.EffectivePower, u.Initiative))
                                            .FirstOrDefault();

                    if (opponent != null)
                    {
                        matches[attacker] = opponent;
                        unmatched.Remove(opponent);
                    }
                }

                // attack
                var attackers = matches.Keys.OrderByDescending(a => a.Initiative).ToList();
                int totalKills = 0;

                foreach (Group attacker in attackers)
                {
                    if (!attacker.Alive)
                    {
                        // died during previous attack this round
                        continue;
                    }

                    var opponent = matches[attacker];
                    var damage = attacker.PotentialDamage(opponent);
                    var kills = damage / opponent.HitPoints; // truncating divide, strips fraction
                    opponent.Units -= kills;
                    totalKills += kills;

                    if (!opponent.Alive)
                    {
                        immuneTeam.Remove(opponent);
                        infectionTeam.Remove(opponent);
                    }
                }

                if (totalKills == 0)
                {
                    // stalemate
                    return new List<Group>();
                }
            }

            return immuneTeam.Union(infectionTeam).ToList();
        }
    }

    public enum Team
    {
        Infection, Immune
    }

    public enum AttackType
    {
        // cold|slashing|bludgeoning|fire|radiation
        Cold, Slashing, Bludgeoning, Fire, Radiation
    }

    public class Group
    {
        private static readonly string[] AttackTypeStrings = Enum.GetNames(typeof(AttackType)).Select(s => s.ToLower()).ToArray();

        // 989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3
        private static readonly string ParseRegex = 
            $@"(\d+) units each with (\d+) hit points (.*) ?with an attack that does (\d+) ({string.Join("|", AttackTypeStrings)}) damage at initiative (\d+)";

        public Team Team { get; set; }
        public int Units { get; set; }
        public int HitPoints { get; set; }
        public ICollection<AttackType> Weaknesses { get; set; } = new AttackType[0];
        public ICollection<AttackType> Immunities { get; set; } = new AttackType[0];
        public AttackType Attack { get; set; }
        public int AttackPower { get; set; }
        public int Initiative { get; set; }

        public int EffectivePower => this.Units * this.AttackPower;
        public bool Alive => this.Units > 0;

        public Group(Team team, string input, int boost = 0)
        {
            var match = Regex.Match(input, ParseRegex);

            this.Team = team;
            this.Units = int.Parse(match.Groups[1].Value);
            this.HitPoints = int.Parse(match.Groups[2].Value);
            this.AttackPower = int.Parse(match.Groups[4].Value) + boost;
            this.Initiative = int.Parse(match.Groups[6].Value);

            if (match.Groups[3].Success)
            {
                string value = match.Groups[3].Value.Replace("(", "").Replace(")", "").Trim();
                var parts = value.Split(';').Select(s => s.Trim()).ToArray();

                foreach (string part in parts)
                {
                    if (part.StartsWith("immune to "))
                    {
                        this.Immunities = part.Replace("immune to ", string.Empty)
                                              .Split(new[] {  ", " }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(s => (AttackType)Enum.Parse(typeof(AttackType), s, true))
                                              .ToHashSet();
                    }
                    else
                    {
                        this.Weaknesses = part.Replace("weak to ", string.Empty)
                                              .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(s => (AttackType)Enum.Parse(typeof(AttackType), s, true))
                                              .ToHashSet();
                    }
                }
            }

            this.Attack = (AttackType)Enum.Parse(typeof(AttackType), match.Groups[5].Value, true);
        }

        public int PotentialDamage(Group enemy)
        {
            if (enemy.Immunities.Contains(this.Attack))
            {
                return 0;
            }

            if (enemy.Weaknesses.Contains(this.Attack))
            {
                return this.AttackPower * 2 * this.Units;
            }

            return this.AttackPower * this.Units;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Team: {this.Team}, Units: {this.Units}, Initiative: {this.Initiative}, EffectivePower: {this.EffectivePower}, Alive: {this.Alive}, Attack: {this.Attack}, AttackPower: {this.AttackPower}";
        }
    }
}
