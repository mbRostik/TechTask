namespace TechTaskParsingFiles.Models
{
    public static class JsonHelper
    {
        public static string FormatJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
