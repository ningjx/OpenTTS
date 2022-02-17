using Newtonsoft.Json;
using SpeechGenerator.Handller;
using SpeechGenerator.Models;
using System;
using System.IO;

namespace SpeechGenerator
{
    public class Config
    {
        //private static readonly string CnfPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SpeechGenerator";
        private static readonly string CnfPath = $"{System.IO.Directory.GetCurrentDirectory()}";

        [JsonProperty("服务秘钥")]
        public string SubscriptionKey { get; set; } = "";

        [JsonProperty("微软服务区域代码")]
        public string Region { get; set; } = "";

        [JsonProperty("文件保存路径")]
        public string SavePath { get; set; } = "";

        [JsonProperty("文本读取路径")]
        public string FilePath { get; set; } = "";

        [JsonProperty("窗口顶部距离")]
        public double Top = 500;
        [JsonProperty("窗口左侧距离")]
        public double Left = 1000;

        /// <summary>
        /// 调节文件音频电平的倍数，1为不调节
        /// </summary>
        [JsonProperty("音量放大倍数")]
        public float Volume = 4f;

        [JsonProperty("语音配置")]
        public SpeechConf SpeechConf { get; set; }

        /// <summary>
        /// 从本机加载配置
        /// </summary>
        /// <returns></returns>
        public static Config LoadConfig()
        {
            try
            {
                if (File.Exists($"{CnfPath}\\Config.json"))
                {
                    var text = File.ReadAllText($"{CnfPath}\\Config.json");
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

        /// <summary>
        /// 保存配置到本地
        /// </summary>
        /// <param name="config"></param>
        public static void SaveConfig(Config config)
        {
            FileHelper.SaveFile(CnfPath, "Config.json", JsonConvert.SerializeObject(config));
        }

        /// <summary>
        /// 生成默认配置
        /// </summary>
        /// <returns></returns>
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
