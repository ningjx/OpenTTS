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

        private SpeechConverter(Config conf)
        {
            speech = new SpeechGen(conf.SubscriptionKey, conf.Region);
            path = conf.SavePath.TrimEnd('\\');
        }

        /// <summary>
        /// 转换语音并保存文件
        /// </summary>
        /// <param name="dicName">要保存到的次级文件夹名</param>
        /// <param name="textItem">要转语音的文本资源</param>
        /// <returns></returns>
        public Result CreateAudioFileFromText(string dicName, TextItem textItem)
        {
            //跳过已经生成的项目
            if (FileHelper.FileExist(path, dicName, textItem.FileName).Success)
                return Result.Sucess();

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

        public void ReConnect()
        {
            speech.Dispose();
            speech = new SpeechGen(ResourcePool.Instance.Config.SubscriptionKey, ResourcePool.Instance.Config.Region);
        }

        private string ReplaceParams(string xml, TextItem textItem)
        {
            var result = xml;
            if (textItem.SpeechConf == null)
            {
                result = result.Replace("@Param1", ResourcePool.Instance.Config.SpeechConf.SpeechLang);
                result = result.Replace("@Param2", ResourcePool.Instance.Config.SpeechConf.SpeechName);
                result = result.Replace("@Param3", ResourcePool.Instance.Config.SpeechConf.SpeechRate.ToString("F1"));
                result = result.Replace("@Param4", ResourcePool.Instance.Config.SpeechConf.SpeechStyle);
                result = result.Replace("@Param5", ResourcePool.Instance.Config.SpeechConf.SpeechDegree.ToString("F1"));
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
