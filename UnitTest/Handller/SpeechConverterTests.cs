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
            var tes2t = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var test = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var testq = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var key = File.ReadAllText(@"D:\config.txt");
            SpeechConf sConfig = new SpeechConf
            {
                SpeechLang = "zh-cn",
                SpeechName = "zh-CN-XiaoxiaoNeural",
                SpeechRate = 0.5,
                SpeechStyle = "cheerful",
                SpeechDegree = 1
            };

            Config config = new Config
            {
                SubscriptionKey = key,
                Region = "southeastasia",
                SavePath = @"D:\",
                SpeechConf = sConfig
            };

            TextItem text = new TextItem { FileName = "测试文件.wav", Text = "警告！油门摇杆不在最低！" };
            var res = SpeechConverter.Instance.CreateAudioFileFromText("ztest", text);

            //TextItem text2 = new TextItem { FileName = "测试文件2.wav", Text = "欢迎使用OpenTx" };
            //var res2 = converter.CreateAudioFileFromText("ztest", text2);
        }
    }
}