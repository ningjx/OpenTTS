﻿using NAudio.CoreAudioApi;
using Newtonsoft.Json;

namespace OpenTTS.Models
{
    /// <summary>
    /// 通过语音合成标记语言 (SSML) 改善合成
    /// 参阅 <see href="https://docs.azure.cn/zh-cn/cognitive-services/speech-service/speech-synthesis-markup?tabs=csharp#adjust-speaking-styles"></see>
    /// </summary>
    public class SpeechConf
    {
        /// <summary>
        /// 语言
        /// </summary>
        [JsonProperty("语言代码")]
        public LanguageEnum SpeechLang { get; set; }
        /// <summary>
        /// 语音代码，也就是讲话的“人”的代码
        /// </summary>
        [JsonProperty("语言风格")] 
        public string SpeechCode { get; set; }
        /// <summary>
        /// 语速 默认1
        /// </summary>
        [JsonProperty("语速")] 
        public double SpeechRate { get; set; }
        /// <summary>
        /// 语气类型 不同语音可以支持的与其类型不尽相同
        /// </summary>
        [JsonProperty("语气")] 
        public SpeechStyleEnum SpeechStyle { get; set; }
        /// <summary>
        /// 语气强度 0.01-2 默认1
        /// </summary>
        [JsonProperty("语气强度")] 
        public double SpeechDegree { get; set; }
        [JsonProperty("角色")]
        public RoleEnum Role { get; set; }
    }
}
