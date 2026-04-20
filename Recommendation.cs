namespace EndfieldRecommendation
{
    public class Recommendation
    {
        public string DungeonName { get; set; } = string.Empty;
        public List<char> MainTraits { get; set; } = new(); // 指定的3个主词条
        public char SpecifiedTrait { get; set; } // 指定的副词条或技能词条
        public bool IsSubTrait { get; set; } // true=副词条, false=技能词条
        public Dictionary<Weapon, DropResult> WeaponProbabilities { get; set; } = new();
        public int DungeonWeight { get; set; }
        /// <summary>未完成完美掉落武器的 P(完美) 之和。</summary>
        public double IncompletePerfectExpectation { get; set; }
        /// <summary>已完成完美掉落武器的 P(完美) 之和。</summary>
        public double CompletedPerfectExpectation { get; set; }
    }
}
