using Newtonsoft.Json;
using System;
using System.IO;
using SpeechGenerator.Models;
using SpeechGenerator.Handller;

namespace SpeechGenerator
{
    public class Config
    {
        private static readonly string CnfPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SpeechGenerator";
        public string SubscriptionKey { get; set; } = "";
        public string Region { get; set; } = "";
        public string SavePath { get; set; } = "";
        public string FilePath { get; set; } = "";

        public SpeechConf SpeechConf { get; set; }

        public static Config LoadConfig()
        {
            try
            {
                if (File.Exists($"{CnfPath}\\config.json"))
                {
                    var text = File.ReadAllText($"{CnfPath}\\config.json");
                    return JsonConvert.DeserializeObject<Config>(text);
                }
                else
                {
                    return GetDefault();
                }
            }
            catch
            {
                return GetDefault();
            }
        }

        public static void SaveConfig(Config config)
        {
            FileHelper.SaveFile(CnfPath, "config.json", JsonConvert.SerializeObject(config));
        }

        private static Config GetDefault()
        {
            SpeechConf sConfig = new SpeechConf
            {
                SpeechLang = "zh-cn",
                SpeechName = "zh-CN-XiaoxiaoNeural",
                SpeechRate = 1,
                SpeechStyle = "cheerful",
                SpeechDegree = 1
            };

            Config config = new Config
            {
                SubscriptionKey = "",
                Region = "",
                SavePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                SpeechConf = sConfig
            };
            return config;
        }
    }
}
