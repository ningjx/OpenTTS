using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechGenerator.Handller;
using SpeechGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGenerator.Handller.Tests
{
    [TestClass()]
    public class SpeechConverterTests
    {
        [TestMethod()]
        public void CreateAudioFileFromTextTest()
        {
            var key = File.ReadAllText(@"D:\config.txt");
            Config config = new Config
            {
                SubscriptionKey = key,
                Region = "southeastasia",
                SavePath = @"D:\",

                SpeechLang = "zh-cn",
                SpeechName = "zh-cn-XiaoxiaoNeural",
                SpeechRate = "+10%",
                SpeechStyle = "affectionate",
                SpeechDegree = "1"
            };

            var converter = new SpeechConverter(config);
            TextItem text = new TextItem { FileName = "测试文件.wav", Text = "欢迎使用EdgeTx" };
            var res = converter.CreateAudioFileFromText("ztest", text);
        }

        [TestMethod()]
        public void CreateAudioFileFromTextTest1()
        {
            Assert.Fail();
        }
    }
}