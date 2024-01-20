using Castle.Components.DictionaryAdapter.Xml;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTaskParsingFiles;
using TechTaskParsingFiles.Controllers;
using TechTaskParsingFiles.Models;
using TechTaskParsingFiles.Services;

namespace TechTaskParsingFilesTest.ServicesTest
{
    public class JsonServiceTest
    {
        //Перевірка на збереження даних відправленого JSON в бд та зрівняння з вхідними даними
        [Fact]
        public async void JsonService_JsonTreeUpload_ShoudSaveAndBeEqualToTheDataThatWasSentJSON()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

            using (var context = new DBContext(options))
            {
                DBModels modelsforcheck = new DBModels();

                foreach (var fileContent in modelsforcheck.JSONSModels) {
                    var fileMock = new Mock<IFormFile>();
                    fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes(fileContent)));
                    fileMock.Setup(f => f.FileName).Returns("test.json");
                    fileMock.Setup(f => f.Length).Returns(fileContent.Length);

                    var jsonTreeService = new JsonService(context);

                    var controller = new HomeController(context, jsonTreeService);

                    // Act
                    var result = await controller.JsonTree(fileMock.Object);

                    // Assert
                    Assert.True(context.jsons.ToList().Count > 0);

                    var check = context.jsons.Last();
                    check.Data.Should().BeEquivalentTo(fileContent);
                }
            }
        }

        //Перевірка на збереження даних відправленого TXT в бд та зрівняння з вхідними даними
        [Fact]
        public async void JsonService_JsonTreeUpload_ShoudSaveAndBeEqualToTheDataThatWasSentTXT()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

            using (var context = new DBContext(options))
            {
                DBModels modelsforcheck = new DBModels();
                for (int i=0;i!= modelsforcheck.TXTCheckModels.Count(); i++)
                {
                    

                    var fileMock = new Mock<IFormFile>();
                    fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes(modelsforcheck.TXTAddModels[i])));
                    fileMock.Setup(f => f.FileName).Returns("test.txt");
                    fileMock.Setup(f => f.Length).Returns(modelsforcheck.TXTAddModels[i].Length);

                    var jsonTreeService = new JsonService(context);

                    var controller = new HomeController(context, jsonTreeService);

                    // Act
                    var result = await controller.JsonTree(fileMock.Object);

                    // Assert
                    Assert.True(context.jsons.ToList().Count > 0);

                    var check = context.jsons.Last();
                    check.Data.Should().BeEquivalentTo(modelsforcheck.TXTCheckModels[i]);
                }
               
            }
        }
    }
}
