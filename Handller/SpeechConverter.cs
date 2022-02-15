using SpeechGenerator.Models;

namespace SpeechGenerator.Handller
{
    public class SpeechConverter
    {
        private static SpeechConverter speechConverter = null;

        public static SpeechConverter Instance
        {
            get
            {
                if (speechConverter == null)
                    speechConverter = new SpeechConverter(ResourcePool.Instance.Config);
                return speechConverter;
            }
        }

        private SpeechGen speech = null;
        private string path = null;
        private Config config = null;

        private SpeechConverter(Config conf)
        {
            speech = new SpeechGen(conf.SubscriptionKey, conf.Region);
            path = conf.SavePath.TrimEnd('\\');
            config = conf;
        }


        public Result CreateAudioFileFromText(string dicName, TextItem textItem)
        {
            var xml = ReplaceParams(Resources.ssml, textItem);
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

        public Result CreateAudioFromText(TextItem textItem)
        {
            var xml = ReplaceParams(Resources.ssml, textItem);
            var audioRes = speech.GetAudioFromText(xml);
            if (audioRes.Success)
                return Result.Sucess();
            else
                return Result.Fail(audioRes.Message);
        }


        private string ReplaceParams(string xml, TextItem textItem)
        {
            var result = xml;
            if (textItem.SpeechConf == null)
            {
                result = result.Replace("@Param1", config.SpeechConf.SpeechLang);
                result = result.Replace("@Param2", config.SpeechConf.SpeechName);
                result = result.Replace("@Param3", config.SpeechConf.SpeechRate.ToString());
                result = result.Replace("@Param4", config.SpeechConf.SpeechStyle);
                result = result.Replace("@Param5", config.SpeechConf.SpeechDegree.ToString());
            }
            else
            {
                result = result.Replace("@Param1", textItem.SpeechConf.SpeechLang);
                result = result.Replace("@Param2", textItem.SpeechConf.SpeechName);
                result = result.Replace("@Param3", textItem.SpeechConf.SpeechRate.ToString());
                result = result.Replace("@Param4", textItem.SpeechConf.SpeechStyle);
                result = result.Replace("@Param5", textItem.SpeechConf.SpeechDegree.ToString());
            }
            result = result.Replace("@Param6", textItem.Text);
            return result;
        }
    }
}
