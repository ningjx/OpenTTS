namespace SpeechGenerator.Models
{
    public class Config
    {
        public string SubscriptionKey { get; set; }
        public string Region { get; set; }
        public string SavePath { get; set; }

        public SpeechConf SpeechConf { get; set; }
    }
}
