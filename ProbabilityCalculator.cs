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
            double mainNotMatchProb = 1.0 - mainMatchProb;

            // 计算副词条和技能词条的概率
            double subMatchProb, subNotMatchProb;
            double skillMatchProb, skillNotMatchProb;

            if (isSubTrait)
            {
                // 指定了副词条
                subMatchProb = (specifiedTrait == weapon.SubTrait) ? 1.0 : 0.0;
                subNotMatchProb = 1.0 - subMatchProb;

                // 技能词条从所有可掉落的技能词条中随机
                skillMatchProb = dungeon.AvailableSkillTraits.Contains(weapon.SkillTrait) 
                    ? 1.0 / dungeon.AvailableSkillTraits.Count 
                    : 0.0;
                skillNotMatchProb = 1.0 - skillMatchProb;
            }
            else
            {
                // 指定了技能词条
                skillMatchProb = (specifiedTrait == weapon.SkillTrait) ? 1.0 : 0.0;
                skillNotMatchProb = 1.0 - skillMatchProb;

                // 副词条从所有可掉落的副词条中随机
                subMatchProb = dungeon.AvailableSubTraits.Contains(weapon.SubTrait) 
                    ? 1.0 / dungeon.AvailableSubTraits.Count 
                    : 0.0;
                subNotMatchProb = 1.0 - subMatchProb;
            }

            // 完美掉落：三个词条都匹配
            result.PerfectDropProbability = mainMatchProb * subMatchProb * skillMatchProb;

            // 良好掉落：恰好两个词条匹配
            // 主+副匹配，技能不匹配
            double mainSubMatch = mainMatchProb * subMatchProb * skillNotMatchProb;
            // 主+技能匹配，副不匹配
            double mainSkillMatch = mainMatchProb * subNotMatchProb * skillMatchProb;
            // 副+技能匹配，主不匹配
            double subSkillMatch = mainNotMatchProb * subMatchProb * skillMatchProb;
            
            result.GoodDropProbability = mainSubMatch + mainSkillMatch + subSkillMatch;

            // 一般掉落：恰好一个词条匹配
            // 仅主匹配
            double onlyMain = mainMatchProb * subNotMatchProb * skillNotMatchProb;
            // 仅副匹配
            double onlySub = mainNotMatchProb * subMatchProb * skillNotMatchProb;
            // 仅技能匹配
            double onlySkill = mainNotMatchProb * subNotMatchProb * skillMatchProb;
            
            result.NormalDropProbability = onlyMain + onlySub + onlySkill;

            return result;
        }
    }
}
