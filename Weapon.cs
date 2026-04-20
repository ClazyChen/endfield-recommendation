namespace EndfieldRecommendation
{
    public class Weapon
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty; // "红" or "金"
        public char MainTrait { get; set; } // 主词条
        public char SubTrait { get; set; } // 副词条
        public char SkillTrait { get; set; } // 技能词条

        /// <summary>武器.txt 中以 # 开头：已达成完美掉落，否则为未完成。</summary>
        public bool IsCompletedPerfectFarm { get; set; }

        public bool IsRed => Color == "红";
        public bool IsGold => Color == "金";
    }
}
