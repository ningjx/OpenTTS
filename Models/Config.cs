namespace SpeechGenerator.Models
{
    public class Config
    {
        public string SubscriptionKey { get; set; }
        public string Region { get; set; }
        public string SavePath { get; set; }

        public string SpeechLang { get; set; }
        public string SpeechName { get; set; }
        public string SpeechRate { get; set; }
        /// <summary>
        /// 语气类型 不同语音可以支持的与其类型不尽相同
        /// 参阅 <see href="https://docs.azure.cn/zh-cn/cognitive-services/speech-service/speech-synthesis-markup?tabs=csharp"></see>
        /// </summary>
        public string SpeechStyle { get; set; }
        /// <summary>
        /// 语气强度 0.01-2 默认1
        /// </summary>
        public string SpeechDegree { get; set; }
    }
}
