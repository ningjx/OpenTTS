using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGenerator.Models
{
    /// <summary>
    /// 通过语音合成标记语言 (SSML) 改善合成
    /// 参阅 <see href="https://docs.azure.cn/zh-cn/cognitive-services/speech-service/speech-synthesis-markup?tabs=csharp#adjust-speaking-styles"></see>
    /// </summary>
    public class SpeechConf
    {
        /// <summary>
        /// 语音语言 zh-cn zh-hk zh-tw，实际全部使用zh-cn即可
        /// </summary>
        public string SpeechLang { get; set; }
        /// <summary>
        /// 语音名字，也就是讲话的“人”
        /// </summary>
        public string SpeechName { get; set; }
        /// <summary>
        /// 语速 默认1
        /// </summary>
        public string SpeechRate { get; set; }
        /// <summary>
        /// 语气类型 不同语音可以支持的与其类型不尽相同
        /// </summary>
        public string SpeechStyle { get; set; }
        /// <summary>
        /// 语气强度 0.01-2 默认1
        /// </summary>
        public string SpeechDegree { get; set; }
    }
}
