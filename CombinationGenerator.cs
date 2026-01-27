namespace EndfieldRecommendation
{
    public static class CombinationGenerator
    {
        // 生成从n个元素中选择k个的所有组合
        public static List<List<T>> GenerateCombinations<T>(List<T> elements, int k)
        {
            var result = new List<List<T>>();
            if (k == 0)
            {
                result.Add(new List<T>());
                return result;
            }

            if (k > elements.Count)
                return result;

            GenerateCombinationsRecursive(elements, k, 0, new List<T>(), result);
            return result;
        }

        private static void GenerateCombinationsRecursive<T>(
            List<T> elements,
            int k,
            int start,
            List<T> current,
            List<List<T>> result)
        {
            if (current.Count == k)
            {
                result.Add(new List<T>(current));
                return;
            }

            for (int i = start; i < elements.Count; i++)
            {
                current.Add(elements[i]);
                GenerateCombinationsRecursive(elements, k, i + 1, current, result);
                current.RemoveAt(current.Count - 1);
            }
        }
    }
}
