using Newtonsoft.Json;
using OpenTTS.Handller;
using OpenTTS.Models;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace OpenTTS
{
    public class Config
    {
        //private static readonly string CnfPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\OpenTTS";
        private static readonly string CnfPath = $"{System.IO.Directory.GetCurrentDirectory()}";

        [JsonProperty("服务秘钥")]
        public string SubscriptionKey { get; set; } = "";

        [JsonProperty("微软服务区域代码")]
        public string Region { get; set; } = "";

        [JsonProperty("文件保存路径")]
        public string SavePath { get; set; } = "";

        [JsonProperty("文本读取路径")]
        public string FilePath { get; set; } = "";

        [JsonProperty("窗口顶部距离像素")]
        public double Top = 500;
        [JsonProperty("窗口左侧距离像素")]
        public double Left = 1000;

        [JsonProperty("重试间隔时间毫秒")]
        public int RetryInterval = 5000;
        [JsonProperty("最大重试次数")]
        public int RetryTime = 3;

        /// <summary>
        /// 调节文件音频电平的倍数，1为不调节
        /// </summary>
        [JsonProperty("音量放大倍数")]
        public float Volume = 1f;

        [JsonProperty("语音配置")]
        public SpeechConf SpeechConf { get; set; }

        /// <summary>
        /// 从本机加载配置
        /// </summary>
        /// <returns></returns>
        public static Config LoadConfig()
        {

            if (File.Exists($"{CnfPath}\\Config.json"))
            {
                var text = File.ReadAllText($"{CnfPath}\\Config.json");
                try
                {
                    return JsonConvert.DeserializeObject<Config>(text);
                }
                catch
                {
                    //如果更新导致原来的配置文件不可用，那尽量从元累的配置中读出秘钥和区域，避免需要重新配置
                    string serviceKey = ExtractByRegex(text, @"""服务秘钥""\s*:\s*""([^""]+)""");
                    string regionCode = ExtractByRegex(text, @"""微软服务区域代码""\s*:\s*""([^""]+)""");
                    var defaultConfig = GetDefault();
                    defaultConfig.SubscriptionKey = serviceKey;
                    defaultConfig.Region = regionCode;
                    return defaultConfig;
                }
            }
            else
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
                SpeechLang = LanguageEnum.普通话,
                SpeechCode = "zh-CN-XiaoxiaoNeural",
                SpeechRate = 1,
                SpeechStyle = SpeechStyleEnum.欢快,
                SpeechDegree = 1,
                Role = RoleEnum.Unspecified
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

        private static string ExtractByRegex(string input, string pattern)
        {
            Match match = Regex.Match(input, pattern, RegexOptions.Multiline);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }
    }
}
