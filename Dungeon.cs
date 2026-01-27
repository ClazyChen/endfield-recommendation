namespace EndfieldRecommendation
{
    public class Dungeon
    {
        public string Name { get; set; } = string.Empty;
        public List<char> AvailableSubTraits { get; set; } = new();
        public List<char> AvailableSkillTraits { get; set; } = new();
    }
}
