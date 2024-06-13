using SpeechGenerator.Handller;
using SpeechGenerator.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace SpeechGenerator
{
    /// <summary>
    /// 程序运行资源池，所有共享的配置、资源和线程，都放在了此处
    /// </summary>
    public class ResourcePool
    {
        /// <summary>
        /// 用来转换语音的线程
        /// </summary>
        private static CancellationTokenSource cts;
        private static int retryTimenow = 0;
        /// <summary>
        /// 计时器
        /// </summary>
        private static Timer RetryTimer = new Timer(1000);
        private static int Backwards;

        /// <summary>
        /// 加载预置的支持的语言格式
        /// </summary>
        public static SpeechResource SpeechResource = null;
        /// <summary>
        /// 程序配置
        /// </summary>
        public static Config Config = null;
        /// <summary>
        /// 文本资源
        /// </summary>
        public static TextResource TextResource = new TextResource();

        static ResourcePool()
        {
            SpeechResource = SpeechResource.LoadSpeechResources();
            Config = Config.LoadConfig();

            RetryTimer.AutoReset = true;
            RetryTimer.Elapsed += (sender, e) => { TitleChange?.Invoke($"连接受限，{--Backwards}秒后尝试第({retryTimenow}/{Config.RetryTime})次重试", 0); };

            Backwards = Config.RetryInterval / 1000;
        }

        /// <summary>
        /// 终止转换
        /// </summary>
        public static void AbordTask()
        {
            cts.Cancel();
            TitleChange?.Invoke("OpenTTS", 0);
            TimerReset();
        }

        /// <summary>
        /// 转换单个资源，用于试听按钮
        /// </summary>
        /// <param name="item"></param>
        public static async Task<Result> AuditionAsync(TextItem item)
        {
            return await SpeechConverter.CreateAudioFromTextAsync(item);
        }

        public static async Task<Result> StartConvertingAsync()
        {
            var result = new Result();
            cts = new CancellationTokenSource();
            await Task.Run(() => { result = ConvertTextToSpeachAsync(cts); });
            return result;
        }


        private static Result ConvertTextToSpeachAsync(CancellationTokenSource cts)
        {
            try
            {
                foreach (var item in TextResource)
                {
                    retryTimenow = 0;
                    cts.Token.ThrowIfCancellationRequested();

                    TitleChange?.Invoke($"({TextResource.IndexOf(item)}/{TextResource.Count})转换中...", TextResource.IndexOf(item));

                    if (item.IsProcessed)
                        continue;

                    var covRes = SpeechConverter.CreateAudioFileFromText(TextResource.DicName, item);

                    if (!covRes.Success)
                    {
                        //失败，进入重试
                        for (int i = 0; i < Config.RetryTime; i++)
                        {
                            retryTimenow++;
                            cts.Token.ThrowIfCancellationRequested();
                            RetryTimer.Start();
                            Thread.Sleep(Config.RetryInterval);
                            TimerReset();
                            covRes = SpeechConverter.CreateAudioFileFromText(TextResource.DicName, item);
                        }

                        if (!covRes.Success)
                            return new Result { Success = false, Message = $"重试{Config.RetryTime}次依然无法生成该条语音，\r\n建议检查格式，\r\n并重启程序再试" };
                    }
                    TextRowChanged?.Invoke(item, TextResource.IndexOf(item));
                    item.IsProcessed = true;
                }
            }
            catch (OperationCanceledException)
            {
                return Result.Fail("任务已停止");
            }
            finally
            {
                TitleChange?.Invoke("OpenTTS", TextResource.Count);
            }

            return Result.Sucess("转换完成");
        }

        private static void TimerReset()
        {
            RetryTimer.Stop();
            Backwards = Config.RetryInterval / 1000;
        }

        public delegate void PollDelegate(object sender, int index);
        public delegate void PollFailDelegate(object sender, string message);
        /// <summary>
        /// 每个文本资源处理完成后触发
        /// </summary>
        public static event PollDelegate TextRowChanged;
        /// <summary>
        /// 通过该事件修改窗口的标题
        /// </summary>
        public static event PollDelegate TitleChange;
    }
}
