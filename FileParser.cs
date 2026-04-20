using System.Text.RegularExpressions;

namespace EndfieldRecommendation
{
    public static class FileParser
    {
        public static List<Weapon> ParseWeapons(string filePath)
        {
            var weapons = new List<Weapon>();
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"武器文件未找到: {filePath}");
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                var completed = trimmedLine.StartsWith('#');
                if (completed)
                    trimmedLine = trimmedLine.TrimStart('#').TrimStart();

                var parts = trimmedLine.Split('\t');
                if (parts.Length < 3)
                    continue;

                var weapon = new Weapon
                {
                    Name = parts[0].Trim(),
                    Color = parts[1].Trim(),
                    IsCompletedPerfectFarm = completed,
                };

                var traits = parts[2].Trim();
                if (traits.Length >= 3)
                {
                    weapon.MainTrait = traits[0];
                    weapon.SubTrait = traits[1];
                    weapon.SkillTrait = traits[2];
                }

                weapons.Add(weapon);
            }

            return weapons;
        }

        public static List<Dungeon> ParseDungeons(string filePath)
        {
            var dungeons = new List<Dungeon>();
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"副本文件未找到: {filePath}");
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // 支持 Tab 或多空格分隔；最后一列为整数权重
                var tokens = Regex.Split(line.Trim(), @"\s+")
                    .Where(t => t.Length > 0)
                    .ToList();

                if (tokens.Count < 3)
                    continue;

                var dungeon = new Dungeon();

                if (tokens.Count >= 4 && int.TryParse(tokens[^1], out var weight))
                {
                    dungeon.Name = string.Join(" ", tokens.Take(tokens.Count - 3));
                    dungeon.Weight = weight;
                    var subTraitsStr = tokens[^3];
                    var skillTraitsStr = tokens[^2];
                    foreach (var ch in subTraitsStr)
                        dungeon.AvailableSubTraits.Add(ch);
                    foreach (var ch in skillTraitsStr)
                        dungeon.AvailableSkillTraits.Add(ch);
                }
                else
                {
                    // 兼容无权重列的旧格式（视为权重 0）
                    dungeon.Name = tokens[0];
                    foreach (var ch in tokens[1])
                        dungeon.AvailableSubTraits.Add(ch);
                    foreach (var ch in tokens[2])
                        dungeon.AvailableSkillTraits.Add(ch);
                    dungeon.Weight = 0;
                }

                dungeons.Add(dungeon);
            }

            return dungeons;
        }
    }
}
