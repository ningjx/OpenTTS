using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using static SpeechGenerator.Models.Voice;

namespace SpeechGenerator.Models.Tests
{
    [TestClass()]
    public class SpeechResourceTests
    {
        //^[A-Z]:\\(.+?\\)
        //private readonly Regex PathRegex = new Regex(@"(?<=.*\\).*(?=\.txt$)");
        private readonly Regex PathRegex = new Regex(@"(?<=^[A-Z]:\\).*(?=\.txt$)");

        [TestMethod()]
        public void LoadSpeechResourcesTest()
        {
            var a = new a();
            a.b = GenderEnum.Male;
            string test = JsonConvert.SerializeObject(a);
            Console.WriteLine(test);
        }
        class a
        {
            public GenderEnum b { get; set; }
        }
    }
}