namespace EndfieldRecommendation
{
    public static class ProbabilityCalculator
    {
        public static DropResult CalculateDropProbability(
            Weapon weapon,
            List<char> specifiedMainTraits,
            char specifiedTrait,
            bool isSubTrait,
            Dungeon dungeon)
        {
            var result = new DropResult();

            // 计算主词条匹配概率
            double mainMatchProb = specifiedMainTraits.Contains(weapon.MainTrait) ? 1.0 / 3.0 : 0.0;

            double subMatchProb;
            double skillMatchProb;

            if (isSubTrait)
            {
                // 指定了副词条
                subMatchProb = (specifiedTrait == weapon.SubTrait) ? 1.0 : 0.0;

                // 技能词条从所有可掉落的技能词条中随机
                skillMatchProb = dungeon.AvailableSkillTraits.Contains(weapon.SkillTrait) 
                    ? 1.0 / dungeon.AvailableSkillTraits.Count 
                    : 0.0;
            }
            else
            {
                // 指定了技能词条
                skillMatchProb = (specifiedTrait == weapon.SkillTrait) ? 1.0 : 0.0;

                // 副词条从所有可掉落的副词条中随机
                subMatchProb = dungeon.AvailableSubTraits.Contains(weapon.SubTrait) 
                    ? 1.0 / dungeon.AvailableSubTraits.Count 
                    : 0.0;
            }

            // 完美掉落：三个词条都匹配
            result.PerfectDropProbability = mainMatchProb * subMatchProb * skillMatchProb;

            return result;
        }
    }
}
