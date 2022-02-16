using SpeechGenerator.Handller;
using SpeechGenerator.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SpeechGenerator
{
    public class ResourcePool
    {
        private static ResourcePool pool = null;
        private Task ConvertTask = null;
        private bool CancleReq = false;
        private readonly Regex PathRegex = new Regex(@"(?<=^[A-Z]:\\).*(?=\.txt$)");
        private System.Timers.Timer Timer = new System.Timers.Timer(1000);
        private int Count = 10;//请求受限等待时间
        private int Backwards;

        public static ResourcePool Instance
        {
            get
            {
                if (pool == null)
                    pool = new ResourcePool();
                return pool;
            }
        }

        public SpeechResource SpeechResource = null;
        public Config Config = null;
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

        public void StartTask()
        {
            if ((ConvertTask != null && ConvertTask.Status != TaskStatus.Running) || ConvertTask == null)
            {
                ConvertTask = new Task(ConvertingTask);
                ConvertTask.Start();
            }
        }

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
        public event PollDelegate TextRowChanged;
        public event PollDelegate TitleChange;
        public event PollDelegate Finish;
        //public event PollFailDelegate FailEvent;
        //public event PollFailDelegate LimitEvent;
    }
}
