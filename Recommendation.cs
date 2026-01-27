namespace EndfieldRecommendation
{
    public class Recommendation
    {
        public string DungeonName { get; set; } = string.Empty;
        public List<char> MainTraits { get; set; } = new(); // 指定的3个主词条
        public char SpecifiedTrait { get; set; } // 指定的副词条或技能词条
        public bool IsSubTrait { get; set; } // true=副词条, false=技能词条
        public Dictionary<Weapon, DropResult> WeaponProbabilities { get; set; } = new();
        public double PerfectExpectation { get; set; }
        public double GoodExpectation { get; set; }
        public double NormalExpectation { get; set; }
    }
}
