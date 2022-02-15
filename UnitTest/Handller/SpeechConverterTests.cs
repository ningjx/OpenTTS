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
            SpeechConf sConfig = new SpeechConf
            {
                SpeechLang = "zh-cn",
                SpeechName = "zh-TW-HsiaoChenNeural",
                SpeechRate = "1.1",
                SpeechStyle = "affectionate",
                SpeechDegree = "1"
            };

            Config config = new Config
            {
                SubscriptionKey = key,
                Region = "southeastasia",
                SavePath = @"D:\",
                SpeechConf = sConfig
            };

            var converter = new SpeechConverter(config);
            TextItem text = new TextItem { FileName = "测试文件.wav", Text = "欢迎使用EdgeTx" };
            var res = converter.CreateAudioFileFromText("ztest", text);

            TextItem text2 = new TextItem { FileName = "测试文件2.wav", Text = "欢迎使用OpenTx" };
            var res2 = converter.CreateAudioFileFromText("ztest", text2);
        }
    }
}