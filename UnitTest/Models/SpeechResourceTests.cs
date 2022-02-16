using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace SpeechGenerator.Models.Tests
{
    [TestClass()]
    public class SpeechResourceTests
    {
        private readonly Regex PathRegex = new Regex(@"(?<=\\).*(?=\.txt$)");

        [TestMethod()]
        public void LoadSpeechResourcesTest()
        {
            string test = @"D:\新建文本文档.txt";
            var w = PathRegex.Match(test).Value;
            Console.WriteLine( PathRegex.Match(test).Value);
            //var res = SpeechResource.LoadSpeechResources();
        }
    }
}