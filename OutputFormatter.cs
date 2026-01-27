namespace EndfieldRecommendation
{
    public static class OutputFormatter
    {
        public static void WriteWeaponWithProbability(Weapon weapon, double probability)
        {
            Console.Write("【");
            
            // 设置颜色
            if (weapon.IsRed)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (weapon.IsGold)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.Write(weapon.Name);
            Console.ResetColor();
            
            Console.Write($"（{probability:P1}）");
            Console.Write("】");
        }

        public static void WriteWeaponList(List<(Weapon weapon, double probability)> weapons, string title, Weapon? targetWeapon = null)
        {
            if (weapons.Count == 0)
            {
                Console.WriteLine($"{title}：无");
                return;
            }

            // 排序：目标武器在最前，然后按颜色（红先金后），最后按概率降序
            var sortedWeapons = weapons.OrderBy(w => 
            {
                // 目标武器排在最前（返回0）
                if (targetWeapon != null && w.weapon.Name == targetWeapon.Name)
                    return 0;
                // 其他武器返回1，排在后面
                return 1;
            })
            .ThenBy(w => 
            {
                // 对于非目标武器，红色返回0（排在前面），金色返回1（排在后面）
                // 目标武器已经排在最前，这里返回0保持位置
                if (targetWeapon != null && w.weapon.Name == targetWeapon.Name)
                    return 0;
                return w.weapon.IsRed ? 0 : 1;
            })
            .ThenByDescending(w => w.probability)
            .ToList();

            Console.Write($"{title}：");
            for (int i = 0; i < sortedWeapons.Count; i++)
            {
                if (i > 0)
                    Console.Write("、");
                
                WriteWeaponWithProbability(sortedWeapons[i].weapon, sortedWeapons[i].probability);
            }
            Console.WriteLine();
        }
    }
}
