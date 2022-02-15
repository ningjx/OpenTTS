using System.Collections.Generic;

namespace SpeechGenerator.Models
{
    public class TextResource : List<TextItem>
    {
        public string DicName { get; set; }
    }

    public class TextItem
    {
        /// <summary>
        /// 该条语音保存的文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 语音文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 可以为每条语音配置单独的语气
        /// </summary>
        public SpeechConf SpeechConf { get; set; }
    }
}
