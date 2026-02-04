namespace EndfieldRecommendation
{
    public static class OutputFormatter
    {
        /// <summary>
        /// 输出完美掉落，按属性词条、名称排序，格式：【敏攻附】 宏愿、十二问
        /// </summary>
        public static void WritePerfectDropListGroupedByTraits(
            List<(Weapon weapon, double probability)> weaponsWithProb,
            string title,
            Weapon? targetWeapon = null)
        {
            if (weaponsWithProb.Count == 0)
            {
                Console.WriteLine($"{title}：无");
                return;
            }

            // 按词条分组
            var groups = weaponsWithProb
                .GroupBy(x => $"{x.weapon.MainTrait}{x.weapon.SubTrait}{x.weapon.SkillTrait}")
                .Select(g => new { Traits = g.Key, Weapons = g.ToList() })
                .ToList();

            // 排序：先按属性词条，再按名称（目标武器最前，红先金后，名称）
            var sortedGroups = groups
                .OrderBy(g => g.Traits)
                .ThenBy(g => g.Weapons.Min(w => w.weapon.Name))
                .ToList();

            Console.WriteLine($"{title}：");
            for (int i = 0; i < sortedGroups.Count; i++)
            {
                var group = sortedGroups[i];
                // 组内排序：目标武器最前，红先金后，名称
                var sortedWeapons = group.Weapons.OrderBy(w =>
                {
                    if (targetWeapon != null && w.weapon.Name == targetWeapon.Name) return 0;
                    return 1;
                })
                .ThenBy(w => w.weapon.IsRed ? 0 : 1)
                .ThenBy(w => w.weapon.Name)
                .ToList();

                Console.Write($"【{group.Traits}】 ");
                for (int j = 0; j < sortedWeapons.Count; j++)
                {
                    if (j > 0) Console.Write("、");
                    var (weapon, _) = sortedWeapons[j];
                    if (weapon.IsRed)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (weapon.IsGold)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(weapon.Name);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
}
