using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechTaskParsingFiles.Models;

namespace TechTaskParsingFiles.Services
{
    public class JsonService:IJsonService
    {
        private DBContext context { get; set; }

        public JsonService(DBContext context)
        {
            this.context = context;
        }

        
        public object GetPropertyValue(dynamic obj, string path)
        {
            JObject jObject = JObject.FromObject(obj);

            foreach (string propertyName in path.Split('/'))
            {
                JToken token = jObject[propertyName];

                if (token == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found in the JSON object.");
                }
                try
                {
                    obj = token.ToObject<dynamic>();
                    jObject = JObject.FromObject(obj);
                }
                catch
                {
                    return obj as string;
                }
            }

            return obj;
        }

        public Dictionary<string, object> ConvertToNestedDictionary(string input)
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var line in lines)
            {
                var tokens = line.Split(':');
                var currentDict = result;

                for (int i = 0; i < tokens.Length - 2; i++)
                {
                    if (!currentDict.ContainsKey(tokens[i]))
                    {
                        currentDict[tokens[i]] = new Dictionary<string, object>();
                    }

                    currentDict = (Dictionary<string, object>)currentDict[tokens[i]];
                }

                currentDict[tokens[tokens.Length - 2]] = tokens[tokens.Length - 1].Trim();
            }

            return result;
        }

        public List<Tree> GetJsonTree(JObject jsonObject)
        {
            List<Tree> Model = new List<Tree>();

            foreach (var property in jsonObject.Properties())
            {
                var node = new Tree
                {
                    Key = property.Name,
                    Value = null,
                    Children = null
                };

                if (property.Value.Type == JTokenType.Object)
                {
                    node.Children = GetJsonTree((JObject)property.Value);
                }
                else
                {
                    node.Value = property.Value.ToString();
                }
                Model.Add(node);
            }

            return Model;
        }
    
        public async Task<List<Tree>> JsonTreeUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var allowedExtensions = new[] { ".json", ".txt" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return null;
            }

            if (fileExtension == ".txt")
            {
                try
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        var txtString = reader.ReadToEnd();
                        Dictionary<string, object> result = ConvertToNestedDictionary(txtString);

                        var jsonString = JsonHelper.FormatJson(result);


                        var data = JsonConvert.DeserializeObject<dynamic>(jsonString);
                        var jsonObject = JObject.Parse(data.ToString());

                        List<Tree> JsonTree = GetJsonTree(jsonObject);

                        JsonModel temp = new JsonModel
                        {
                            Data = jsonString
                        };
                        context.jsons.Add(temp);
                        await context.SaveChangesAsync();
                        return JsonTree;
                    }
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                try
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        var jsonString = reader.ReadToEnd();
                        var data = JsonConvert.DeserializeObject<dynamic>(jsonString);
                        var jsonObject = JObject.Parse(data.ToString());

                        List<Tree> JsonTree = GetJsonTree(jsonObject);

                        JsonModel temp = new JsonModel
                        {
                            Data = jsonString
                        };
                        context.jsons.Add(temp);
                        await context.SaveChangesAsync();
                        return JsonTree;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public async Task<List<Tree>> JsonTreeById(int id)
        {
            var myData = context.jsons.Find(id);

            if (myData == null)
            {
                return null;
            }
            try
            {

                var jsonString = myData.Data;
                var data = JsonConvert.DeserializeObject<dynamic>(jsonString);
                var jsonObject = JObject.Parse(data.ToString());

                List<Tree> JsonTree = GetJsonTree(jsonObject);

                return JsonTree;

            }

            catch
            {
                return null;
            }
        }
        public async Task<List<Tree>> MyUrl(string path)
        {
            if (path == null)
            {
                return null;
            }
            JsonModel myData = new JsonModel();
            try
            {
                myData = await context.jsons.FindAsync(int.Parse(path[0].ToString()));

                if (myData == null)
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

            try
            {
                string stringWithoutFirstChar = path.Remove(0, 2);
                dynamic currentObject = JsonConvert.DeserializeObject<dynamic>(myData.Data);

                object result = GetPropertyValue(currentObject, stringWithoutFirstChar);

                if (result.GetType() == typeof(string))
                {
                    List<Tree> temp = new List<Tree> { new Tree { Value = (string)result, Children = null, Key = null } };
                    return temp;
                }

                var jsonObject = JObject.Parse((dynamic)result.ToString());

                List<Tree> JsonTree = GetJsonTree(jsonObject);

                return JsonTree;
            }
            catch
            {
                return null;
            }
        }
    }
}
