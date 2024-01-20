using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTaskParsingFilesTest.ServicesTest
{
    public class DBModels
    {
        //Моделі для записання дaних які надсилаються в JSON
        public List<string> JSONSModels { get; set; }


        //Моделі для перевірки дaних які надсилаються в TXT
        public List<string> TXTCheckModels { get; set; }


        //Моделі для записання дaних які надсилаються в TXT
        public List<string> TXTAddModels { get; set; }

        public DBModels()
        {
            JSONSModels = new List<string>();
            TXTCheckModels = new List<string>();
            TXTAddModels = new List<string>();

            JSONSModels.Add(@"{
  ""keyA"": {
    ""keyB"": {
      ""keyC"": ""value1"",
      ""keyD"": ""value2""
    },
    ""keyD"": ""value5""
  },
  ""keyE"": ""value3"",
  ""keyC"": {
    ""keyD"": ""value4""
  }
}");
            JSONSModels.Add(@"{
  ""keyA"": {
    ""keyB"": {
      ""keyC"": ""value1"",
      ""keyD"": {
        ""keyL"": ""valuesm""
      }
    },
    ""keyD"": ""value5""
  },
  ""keyE"": ""value3"",
  ""keyC"": {
    ""keyD"": ""value4""
  }
}");

            TXTCheckModels.Add(@"{
  ""keyA"": {
    ""keyB"": {
      ""keyC"": ""value1"",
      ""keyD"": ""value2""
    },
    ""keyD"": ""value5""
  },
  ""keyE"": ""value3"",
  ""keyC"": {
    ""keyD"": ""value4""
  }
}");
            TXTAddModels.Add("keyA:keyB:keyC:value1\nkeyA:keyB:keyD:value2\nkeyE:value3\nkeyC:keyD:value4\nkeyA:keyD:value5");
        }
    }
}
