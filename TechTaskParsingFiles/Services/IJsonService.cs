using Newtonsoft.Json.Linq;
using TechTaskParsingFiles.Models;

namespace TechTaskParsingFiles.Services
{
    public interface IJsonService
    {
        object GetPropertyValue(dynamic obj, string path);

        Dictionary<string, object> ConvertToNestedDictionary(string input);

        List<Tree> GetJsonTree(JObject jsonObject);

        Task<List<Tree>> JsonTreeUpload(IFormFile file);

        Task<List<Tree>> JsonTreeById(int id);

        Task<List<Tree>> MyUrl(string path);
    }
}
