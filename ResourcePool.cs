using SpeechGenerator.Handller;
using SpeechGenerator.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SpeechGenerator
{
    /// <summary>
    /// 程序运行资源池，所有共享的配置、资源和线程，都放在了此处
    /// </summary>
    public class ResourcePool
    {
        private static ResourcePool pool = null;

        /// <summary>
        /// 用来转换语音的线程
        /// </summary>
        private Task ConvertTask = null;
        private bool CancleReq = false;

        /// <summary>
        /// 从文件路径中提取不含后缀的文件名
        /// </summary>
        private readonly Regex PathRegex = new Regex(@"(?<=^[A-Z]:\\).*(?=\.txt$)");

        /// <summary>
        /// 计时器
        /// </summary>
        private System.Timers.Timer Timer = new System.Timers.Timer(1000);
        private int Count = 10;//可以配置请求受限等待的时间
        private int Backwards;

        /// <summary>
        /// 单例
        /// </summary>
        public static ResourcePool Instance
        {
            get
            {
                if (pool == null)
                    pool = new ResourcePool();
                return pool;
            }
        }

        /// <summary>
        /// 加载预置的支持的语言格式
        /// </summary>
        public SpeechResource SpeechResource = null;
        /// <summary>
        /// 程序配置
        /// </summary>
        public Config Config = null;
        /// <summary>
        /// 文本资源
        /// </summary>
        public TextResource TextResource = new TextResource();

        private ResourcePool()
        {
            SpeechResource = SpeechResource.LoadSpeechResources();
            Config = Config.LoadConfig();

            Timer.AutoReset = true;
            Timer.Elapsed += Timer_Elapsed;

            Backwards = Count;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TitleChange?.Invoke($"连接受限，{--Backwards}秒后自动继续", 0);
        }

        /// <summary>
        /// 开始转换
        /// </summary>
        public void StartTask()
        {
            if ((ConvertTask != null && ConvertTask.Status != TaskStatus.Running) || ConvertTask == null)
            {
                ConvertTask = new Task(ConvertingTask);
                ConvertTask.Start();
            }
        }

        /// <summary>
        /// 终止转换
        /// </summary>
        public void AbordTask()
        {
            if (ConvertTask != null && ConvertTask.Status == TaskStatus.Running)
            {
                CancleReq = true;
                TitleChange?.Invoke("OpenTTS", 0);
                TimerReset();
                //Thread.Sleep(2000);
                //ConvertTask.Dispose();
            }
        }

        /// <summary>
        /// 转换单个资源，用于试听按钮
        /// </summary>
        /// <param name="item"></param>
        public void StartTask(TextItem item)
        {
            Task.Run(() => { SpeechConverter.Instance.CreateAudioFromText(item); });
        }

        private void ConvertingTask()
        {
            TextResource.DicName = PathRegex.Match(Config.FilePath).Value?.Split('\\').ToList().LastOrDefault();
            foreach (var item in TextResource)
            {
                if (CancleReq)
                {
                    CancleReq = false;
                    return;
                }

                if (item.IsProcessed)
                    continue;

                TitleChange?.Invoke("转换中...", TextResource.IndexOf(item));
                Thread.Sleep(100);
                ConvertNRetry(item);
            }
            TitleChange?.Invoke("OpenTTS", TextResource.Count);
        }

        private Result ConvertNRetry(TextItem item)
        {
            var newRes = SpeechConverter.Instance.CreateAudioFileFromText(TextResource.DicName, item);
            if (!newRes.Success)
            {
                Timer.Start();
                Thread.Sleep(TimeSpan.FromSeconds(Count));
                TimerReset();
                return ConvertNRetry(item);
            }
            else
            {
                TextRowChanged?.Invoke(item, TextResource.IndexOf(item));
                item.IsProcessed = true;
                return newRes;
            }
        }

        private void TimerReset()
        {
            Timer.Stop();
            Backwards = Count;
        }

        public delegate void PollDelegate(object sender, int index);
        public delegate void PollFailDelegate(object sender, string message);
        /// <summary>
        /// 每个文本资源处理完成后触发
        /// </summary>
        public event PollDelegate TextRowChanged;
        /// <summary>
        /// 通过该事件修改窗口的标题
        /// </summary>
        public event PollDelegate TitleChange;
    }
}
