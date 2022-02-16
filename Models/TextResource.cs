using System.Collections.Generic;
using System.ComponentModel;

namespace SpeechGenerator.Models
{
    public class TextResource : List<TextItem>

    {
        public string DicName { get; set; }

    }

    public class TextItem : INotifyPropertyChanged
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

        private bool _isProcessed;


        public bool IsProcessed
        {
            get => _isProcessed;
            set
            {
                _isProcessed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsProcessed"));
            }
        }

        public TextItem(string filename, string text)
        {
            FileName = filename;
            Text = text;
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
