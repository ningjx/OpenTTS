using SpeechGenerator.Models;

namespace SpeechGenerator.Handller
{
    public class SpeechConverter
    {
        private SpeechGen speech = null;
        private string path = null;
        private Config config = null;

        public SpeechConverter(Config conf)
        {
            speech = new SpeechGen(conf.SubscriptionKey, conf.Region);
            path = conf.SavePath.TrimEnd('\\');
            config = conf;
        }


        public Result CreateAudioFileFromText(string dicName, TextItem textItem)
        {
            var xml = Resources.ssml;
            xml = xml.Replace("@Param1", config.SpeechLang);
            xml = xml.Replace("@Param2", config.SpeechName);
            xml = xml.Replace("@Param3", config.SpeechRate);
            xml = xml.Replace("@Param4", config.SpeechStyle);
            xml = xml.Replace("@Param5", config.SpeechDegree);
            xml = xml.Replace("@Param6", textItem.Text);

            var audioRes = speech.GetAudioFromText(xml);

            if (audioRes.Success)
            {
                var fileRes = FileHelper.SaveFile(path, dicName, textItem.FileName, audioRes.Data as byte[]);
                if (fileRes.Success)
                    return Result.Sucess();
                else
                    return Result.Fail(fileRes.Message);
            }
            else
            {
                return Result.Fail(audioRes.Message);
            }
        }
    }
}
