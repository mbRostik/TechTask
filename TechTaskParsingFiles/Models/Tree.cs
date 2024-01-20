namespace TechTaskParsingFiles.Models
{
    public class Tree
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public List<Tree> Children { get; set; }
    }
}
