using Microsoft.CognitiveServices.Speech;
using SpeechGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGenerator.Handller
{
    internal class SpeechGen:IDisposable
    {
        private SpeechSynthesizer speechSynthesizer = null;

        public SpeechGen(SpeechSynthesizer synthesizer)
        {
            speechSynthesizer = synthesizer;
        }

        public SpeechGen(string subscriptionKey, string region)
        {
            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff16Khz16BitMonoPcm);
            speechSynthesizer = new SpeechSynthesizer(config);

        }

        public Result GetAudioFromText(string text)
        {
            var res = speechSynthesizer.SpeakSsmlAsync(text).Result;
            var check = SpeechSynthesisCancellationDetails.FromResult(res);
            if (check.ErrorCode == 0)
            {
                return new Result
                {
                    Success = true,
                    Data = res.AudioData
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
