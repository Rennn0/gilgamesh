namespace EducativeIo.Projects.Netflix
{
    public partial class Netflix
    {
        public class MovieCombinations
        {
            private readonly Dictionary<string, string[]> _movies;
            private readonly List<string> _combinations;
            public MovieCombinations()
            {
                _combinations = new List<string>();
                _movies = new Dictionary<string, string[]>
                {
                    { "Family", new string[3]{"Frozen","Kung fu Panda", "Ice Age"} },
                    { "Action", new string[3]{"Iron Man","Wonder Woman","Avengers"}},
                    { "Fantasy", new string[3]{"Jumangi", "Lion King", "Tarzan"}},
                    { "Comedy", new string[4]{"Coco", "The Croods", "Vivi","Pets"}},
                    { "Horror", new string[4]{"Oculus", "Sinister","Insidious","Annebelle"}},
                };
            }

            public List<string> Combinations(string[] categories)
            {
                if (categories.Length == 0) return [];
                Combinations(0, new List<string>(), ref categories);
                return _combinations;
            }

            private void Combinations(int index, List<string> path, ref string[] categories)
            {
                if (path.Count == categories.Count())
                {
                    _combinations.Add(string.Join("", path));
                    return;
                }

                string[] possibleCombinations = _movies[categories[index]];
                for (int i = 0; i < possibleCombinations.Length; i++)
                {
                    path.Add($"{possibleCombinations[i]};");
                    Combinations(index + 1, path, ref categories);
                    if (path.Count > 0)
                    {
                        path.RemoveAt(path.Count - 1);
                    }
                }
            }
        }
    }
}