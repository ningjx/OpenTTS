using System.Collections.Generic;

namespace SpeechGenerator.Models
{
    public class TextResource : List<TextItem>
    {
        public string DicName { get; set; }
    }

    public class TextItem
    {
        public string FileName { get; set; }
        public string Text { get; set; }
    }
}
