using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechTaskParsingFiles.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using TechTaskParsingFiles.Services;
namespace TechTaskParsingFiles.Controllers
{
    public class HomeController : Controller
    {
        private DBContext context { get; set; }
        private IJsonService service { get; set; }
        public HomeController(DBContext context, IJsonService service)
        {
            this.context = context;
            this.service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var myDataItems = context.jsons.ToList();
            return View(myDataItems);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        //action для загрузки даних та їх відображення
        public async Task<IActionResult> JsonTree(IFormFile file)
        {
            var result = await service.JsonTreeUpload(file);

            if (result == null)
            {
                return StatusCode(500, "Smth went wrong");
            }
            return View(result);
        }


        [HttpGet("JsonTree")]
        //action для відображення даних по вибраному айді
        public async Task<IActionResult> JsonTree(int id)
        {
            var result = await service.JsonTreeById(id);
            if (result == null)
            {
                return StatusCode(500, "Smth went wrong");
            }

            return View(result);
        }
        
        [HttpGet("JsonShow")]
        //action для відображення окремого вузла
        public IActionResult JsonShow(string id, string children)
        {
            
            var selectedNode = new Tree
            {
                Key = id,
                Children = JsonConvert.DeserializeObject<List<Tree>>(children)
            };

            return View(selectedNode);
        }

        [HttpGet("Check/{*path}")]
        //action для відображення окремого вузла або кінцевого елементу через URL 
        public async Task<IActionResult> MyUrl(string path)
        {
            var result = await service.MyUrl(path);

            if (result == null)
            {
                return StatusCode(500, "Smth went wrong");
            }

            return View(result);
        }

    }
}