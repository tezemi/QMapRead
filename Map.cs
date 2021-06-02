using System.Collections.Generic;

namespace QMapRead
{
    public class Map
    {
        public string FilePath { get; set; }
        public readonly List<Entity> Entities = new List<Entity>();

        public Map(string filePath)
        {
            FilePath = filePath;
        }
    }
}