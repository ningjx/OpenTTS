using System.Collections.Generic;
using System.ComponentModel;

namespace SpeechGenerator.Models
{
    public class TextResource : List<TextItem>
    {
        /// <summary>
        /// 转换的语音保存到的新文件夹名
        /// </summary>
        public string DicName { get; set; }
    }

    public class TextItem : INotifyPropertyChanged
    {
        private bool _isProcessed;

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
        /// <summary>
        /// 是否已经处理
        /// </summary>
        public bool IsProcessed
        {
            get => _isProcessed;
            set
            {
                _isProcessed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsProcessed"));
            }
        }

        /// <summary>
        /// 文本资源初始化
        /// </summary>
        /// <param name="filename">语音文件的文件名</param>
        /// <param name="text">文本</param>
        public TextItem(string filename, string text)
        {
            FileName = filename;
            Text = text;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
