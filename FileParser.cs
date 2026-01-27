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
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split('\t');
                if (parts.Length < 3)
                    continue;

                var weapon = new Weapon
                {
                    Name = parts[0].Trim(),
                    Color = parts[1].Trim(),
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

                var parts = line.Split('\t');
                if (parts.Length < 3)
                    continue;

                var dungeon = new Dungeon
                {
                    Name = parts[0].Trim(),
                };

                // 解析可掉落的副词条
                var subTraitsStr = parts[1].Trim();
                foreach (var ch in subTraitsStr)
                {
                    dungeon.AvailableSubTraits.Add(ch);
                }

                // 解析可掉落的技能词条
                var skillTraitsStr = parts[2].Trim();
                foreach (var ch in skillTraitsStr)
                {
                    dungeon.AvailableSkillTraits.Add(ch);
                }

                dungeons.Add(dungeon);
            }

            return dungeons;
        }
    }
}
