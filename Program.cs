namespace EndfieldRecommendation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 读取数据
                var weapons = FileParser.ParseWeapons("武器.txt");
                var dungeons = FileParser.ParseDungeons("副本.txt");

                // 获取用户输入的武器名
                Console.WriteLine("请输入武器名：");
                string? weaponName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(weaponName))
                {
                    Console.WriteLine("错误：武器名不能为空");
                    return;
                }

                // 查找目标武器
                var targetWeapon = weapons.FirstOrDefault(w => w.Name == weaponName);
                if (targetWeapon == null)
                {
                    Console.WriteLine($"错误：未找到武器 '{weaponName}'");
                    return;
                }

                // 生成所有主词条组合（从"主力敏意智"中选择3个）
                var mainTraitOptions = new List<char> { '主', '力', '敏', '意', '智' };
                var mainTraitCombinations = CombinationGenerator.GenerateCombinations(mainTraitOptions, 3);

                // 生成所有推荐方案
                var recommendations = new List<Recommendation>();

                foreach (var dungeon in dungeons)
                {
                    foreach (var mainTraits in mainTraitCombinations)
                    {
                        // 尝试指定每个可掉落的副词条
                        foreach (var subTrait in dungeon.AvailableSubTraits)
                        {
                            var recommendation = CalculateRecommendation(
                                weapons,
                                dungeon,
                                mainTraits,
                                subTrait,
                                true, // isSubTrait
                                targetWeapon);

                            if (recommendation != null)
                            {
                                recommendations.Add(recommendation);
                            }
                        }

                        // 尝试指定每个可掉落的技能词条
                        foreach (var skillTrait in dungeon.AvailableSkillTraits)
                        {
                            var recommendation = CalculateRecommendation(
                                weapons,
                                dungeon,
                                mainTraits,
                                skillTrait,
                                false, // isSubTrait
                                targetWeapon);

                            if (recommendation != null)
                            {
                                recommendations.Add(recommendation);
                            }
                        }
                    }
                }

                // 排序：未完成完美期望降序 → 副本权重升序 → 已完成完美期望降序
                recommendations = recommendations
                    .OrderByDescending(r => r.IncompletePerfectExpectation)
                    .ThenBy(r => r.DungeonWeight)
                    .ThenByDescending(r => r.CompletedPerfectExpectation)
                    .ToList();

                // 输出结果
                Console.WriteLine();
                Console.WriteLine("推荐方案：");
                Console.WriteLine("=".PadRight(80, '='));

                if (recommendations.Count == 0)
                {
                    Console.WriteLine("未找到符合条件的推荐方案（目标武器的完美掉落概率必须大于0）");
                }
                else
                {
                    // 只显示最优方案
                    var rec = recommendations[0];
                    Console.WriteLine();
                    Console.WriteLine($"副本：{rec.DungeonName}");
                    
                    // 输出词条指定方式
                    string mainTraitsStr = string.Join("、", rec.MainTraits);
                    string traitType = rec.IsSubTrait ? "副词条" : "技能词条";
                    Console.WriteLine($"词条指定：主词条 [{mainTraitsStr}]，{traitType} [{rec.SpecifiedTrait}]");

                    // 输出完美掉落（按词条分组合并，保留概率）
                    var perfectIncomplete = rec.WeaponProbabilities
                        .Where(kvp => !kvp.Key.IsCompletedPerfectFarm && kvp.Value.PerfectDropProbability > 0)
                        .Select(kvp => (kvp.Key, kvp.Value.PerfectDropProbability))
                        .ToList();
                    var perfectCompleted = rec.WeaponProbabilities
                        .Where(kvp => kvp.Key.IsCompletedPerfectFarm && kvp.Value.PerfectDropProbability > 0)
                        .Select(kvp => (kvp.Key, kvp.Value.PerfectDropProbability))
                        .ToList();
                    OutputFormatter.WritePerfectDropListGroupedByTraits(perfectIncomplete, "未完成的完美掉落", targetWeapon);
                    OutputFormatter.WritePerfectDropListGroupedByTraits(perfectCompleted, "已完成的完美掉落", targetWeapon);

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误：{ex.Message}");
            }
        }

        static Recommendation? CalculateRecommendation(
            List<Weapon> weapons,
            Dungeon dungeon,
            List<char> mainTraits,
            char specifiedTrait,
            bool isSubTrait,
            Weapon targetWeapon)
        {
            var recommendation = new Recommendation
            {
                DungeonName = dungeon.Name,
                DungeonWeight = dungeon.Weight,
                MainTraits = new List<char>(mainTraits),
                SpecifiedTrait = specifiedTrait,
                IsSubTrait = isSubTrait,
                WeaponProbabilities = new Dictionary<Weapon, DropResult>()
            };

            // 计算每个武器的掉落概率
            foreach (var weapon in weapons)
            {
                var dropResult = ProbabilityCalculator.CalculateDropProbability(
                    weapon,
                    mainTraits,
                    specifiedTrait,
                    isSubTrait,
                    dungeon);

                recommendation.WeaponProbabilities[weapon] = dropResult;
            }

            // 检查目标武器的完美掉落概率是否大于0
            if (!recommendation.WeaponProbabilities.ContainsKey(targetWeapon) ||
                recommendation.WeaponProbabilities[targetWeapon].PerfectDropProbability <= 0)
            {
                return null; // 不符合条件，过滤掉
            }

            recommendation.IncompletePerfectExpectation = recommendation.WeaponProbabilities
                .Where(kvp => !kvp.Key.IsCompletedPerfectFarm)
                .Sum(kvp => kvp.Value.PerfectDropProbability);
            recommendation.CompletedPerfectExpectation = recommendation.WeaponProbabilities
                .Where(kvp => kvp.Key.IsCompletedPerfectFarm)
                .Sum(kvp => kvp.Value.PerfectDropProbability);

            return recommendation;
        }
    }
}
