namespace EndfieldRecommendation
{
    public class Dungeon
    {
        public string Name { get; set; } = string.Empty;
        public List<char> AvailableSubTraits { get; set; } = new();
        public List<char> AvailableSkillTraits { get; set; } = new();
        /// <summary>副本.txt 最后一列：排序时权重升序（数值越小越优先）。</summary>
        public int Weight { get; set; }
    }
}
