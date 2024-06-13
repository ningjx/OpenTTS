using Microsoft.CognitiveServices.Speech;
using SpeechGenerator.Models;
using System;
using System.Threading.Tasks;

namespace SpeechGenerator.Handller
{
    internal class SpeechGen : IDisposable
    {
        /// <summary>
        /// 语音转换实例
        /// </summary>
        private SpeechSynthesizer speechSynthesizer = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionKey">秘钥</param>
        /// <param name="region">区域代码</param>
        public SpeechGen(string subscriptionKey, string region)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff16Khz16BitMonoPcm);
            speechSynthesizer = new SpeechSynthesizer(config);
        }

        /// <summary>
        /// 通过XML文本，转换语音
        /// 注意XML文本包含SSML语言，不是单纯的要转换的文字
        /// </summary>
        /// <param name="text">XML文件</param>
        /// <returns></returns>
        public async Task<Result> GetAudioFromTextAsync(string text)
        {
            var speech =await speechSynthesizer.SpeakSsmlAsync(text);
            var check = SpeechSynthesisCancellationDetails.FromResult(speech);
            if (check.ErrorCode == 0)
            {
                return new Result
                {
                    Success = true,
                    Data = speech.AudioData
                };
            }
            else
            {
                return new Result
                {
                    Success = false,
                    Message = $"转换失败，原因：{check.Reason}\r\n详情{check.ErrorDetails}"
                };
            }
        }

        public  Result GetAudioFromText(string text)
        {
            var speech = speechSynthesizer.SpeakSsmlAsync(text).Result;
            var check = SpeechSynthesisCancellationDetails.FromResult(speech);
            if (check.ErrorCode == 0)
            {
                return new Result
                {
                    Success = true,
                    Data = speech.AudioData
                };
            }
            else
            {
                return new Result
                {
                    Success = false,
                    Message = $"转换失败，原因：{check.Reason}\r\n详情{check.ErrorDetails}"
                };
            }
        }

        public void Dispose()
        {
            speechSynthesizer.Dispose();
        }
    }
}
