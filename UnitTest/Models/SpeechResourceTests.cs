using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

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
            string test = @"C:\Users\aa\Desktop\SpeechGenerator\EdgeTx语音模板\新建文本文档.txt";
            var w = PathRegex.Match(test).Value;
            Console.WriteLine( PathRegex.Match(test).Value);
            //var res = SpeechResource.LoadSpeechResources();
        }
    }
}