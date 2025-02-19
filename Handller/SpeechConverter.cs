using OpenTTS.Models;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace OpenTTS.Handller
{
    public class SpeechConverter
    {
        private static SpeechGen speech = null;

        static SpeechConverter()
        {
            speech = new SpeechGen(ResourcePool.Config.SubscriptionKey, ResourcePool.Config.Region);
        }

        /// <summary>
        /// 转换语音并保存文件
        /// </summary>
        /// <param name="dicName">要保存到的次级文件夹名</param>
        /// <param name="textItem">要转语音的文本资源</param>
        /// <returns></returns>
        public static Result CreateAudioFileFromText(string dicName, TextItem textItem)
        {
            var savePath = ResourcePool.Config.SavePath.TrimEnd('\\');
            //跳过已经生成的项目
            if (FileHelper.FileExist(savePath, dicName, textItem.FileName).Success)
                return Result.Sucess();

            var audio = CreateAudioFromText(textItem);
            if (audio.Success)
            {
                var fileRes = FileHelper.SaveFile(savePath, dicName, textItem.FileName, audio.Data as byte[]);
                if (fileRes.Success)
                    return Result.Sucess();
                else
                    return Result.Fail(fileRes.Message);
            }
            else
            {
                return Result.Fail(audio.Message);
            }
        }

        public async static Task<Result> CreateAudioFromTextAsync(TextItem textItem)
        {
            var xml = ReplaceParams(Resources.ssml, textItem);
            var audio = await speech.GetAudioFromTextAsync(xml);

            if (audio.Success)
                return Result.Sucess();
            else
                return Result.Fail(audio.Message);
        }

        private static Result CreateAudioFromText(TextItem textItem)
        {
            var xml = ReplaceParams(Resources.ssml, textItem);
            return speech.GetAudioFromText(xml);
        }

        public void ReConnect()
        {
            speech.Dispose();
            speech = new SpeechGen(ResourcePool.Config.SubscriptionKey, ResourcePool.Config.Region);
        }

        private static string ReplaceParams(string xml, TextItem textItem)
        {
            var result = xml;
            if (textItem.SpeechConf == null)
            {
                result = result.Replace("@Param1", GetEnumDescription(ResourcePool.Config.SpeechConf.SpeechLang));
                result = result.Replace("@Param2", ResourcePool.Config.SpeechConf.SpeechCode);
                result = result.Replace("@Param3", ResourcePool.Config.SpeechConf.SpeechRate.ToString("F1"));
                result = result.Replace("@Param4", GetEnumDescription(ResourcePool.Config.SpeechConf.SpeechStyle));
                result = result.Replace("@Param5", ResourcePool.Config.SpeechConf.SpeechDegree.ToString("F1"));
            }
            else
            {
                result = result.Replace("@Param1", GetEnumDescription(textItem.SpeechConf.SpeechLang));
                result = result.Replace("@Param2", textItem.SpeechConf.SpeechCode);
                result = result.Replace("@Param3", textItem.SpeechConf.SpeechRate.ToString());
                result = result.Replace("@Param4", GetEnumDescription(textItem.SpeechConf.SpeechStyle));
                result = result.Replace("@Param5", textItem.SpeechConf.SpeechDegree.ToString());
            }
            result = result.Replace("@Param6", textItem.Text);
            return result;
        }

        private static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}
