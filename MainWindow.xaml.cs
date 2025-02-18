using OpenTTS.Handller;
using OpenTTS.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OpenTTS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //从配置文件中，加载窗口位置
            Top = ResourcePool.Config.Top;
            Left = ResourcePool.Config.Left;

            //从配置中加载保存的秘钥和区域代码
            keyinput.Text = string.IsNullOrEmpty(ResourcePool.Config.SubscriptionKey) ? keyinput.Text : ResourcePool.Config.SubscriptionKey;
            reginput.Text = string.IsNullOrEmpty(ResourcePool.Config.Region) ? reginput.Text : ResourcePool.Config.Region;

            //根据条件切换启动时展示的页面
            if (Helper.CheckUpdate())
                update.Visibility = Visibility.Visible;
            else if (string.IsNullOrEmpty(ResourcePool.Config.SubscriptionKey))
                configKey.Visibility = Visibility.Visible;
            else
                configSpeech.Visibility = Visibility.Visible;
            
            //绑定语言选项
            language.ItemsSource = Enum.GetValues(typeof(LanguageEnum));

            //从配置中加载保存文件夹和资源文件路径
            flodertext.Text = string.IsNullOrEmpty(ResourcePool.Config.SavePath) ? flodertext.Text : ResourcePool.Config.SavePath;
            filetext.Text = string.IsNullOrEmpty(ResourcePool.Config.FilePath) ? filetext.Text : ResourcePool.Config.FilePath;

            //订阅转换任务的事件
            ResourcePool.TextRowChanged += Instance_TextRowChanged;
            ResourcePool.TitleChange += Instance_TitleChange;
        }

        private void Instance_TitleChange(object sender, int index)
        {
            Dispatcher.Invoke(new WindowDelegate(() =>
            {
                this.Title = (string)sender;
            }
            ));
        }

        private void Instance_TextRowChanged(object sender, int index)
        {
            datagrid.Dispatcher.Invoke(new WindowDelegate(() =>
            {
                datagrid.ScrollIntoView(sender);
            }
            ));
        }

        /// <summary>
        /// 选择语言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void language_Select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = sender as ComboBox;
            var selectLan = combox.SelectedIndex;
            speechSelect.ItemsSource = ResourcePool.SpeechResource.Where(l => (int)l.Language == selectLan);
            speechSelect.SelectedIndex = 0;
        }

        /// <summary>
        /// 选择讲话人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void person_Select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = sender as ComboBox;
            var selectItem = (Voice)combox.SelectedValue;

            if (selectItem == null)
                return;

            var voice = ResourcePool.SpeechResource.FirstOrDefault(t => t.Name == selectItem.Name);
            ResourcePool.Config.SpeechConf.SpeechCode = voice.Code;
            var styles = ResourcePool.SpeechResource.Where(l => l.Code == ResourcePool.Config.SpeechConf.SpeechCode).FirstOrDefault()?.Styles;
            styleSelect.ItemsSource = styles;
            styleSelect.SelectedIndex = 0;
        }

        /// <summary>
        /// 选择语气
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void styleSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = sender as ComboBox;
            var style = combox.SelectedValue as SpeechStyle;
            if (style != null)
                ResourcePool.Config.SpeechConf.SpeechStyle = style.Style;
        }

        /// <summary>
        /// 试听按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_ShiTing(object sender, RoutedEventArgs e)
        {
            TextItem text = new TextItem("", speechText.Text);
            audition.IsEnabled = false;
            var result = await ResourcePool.AuditionAsync(text);
            audition.IsEnabled = true;
            if (!result.Success)
                MessageBox.Show(result.Message);
        }

        /// <summary>
        /// 退出程序时保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ResourcePool.Config.Top = Top;
            ResourcePool.Config.Left = Left;
            Config.SaveConfig(ResourcePool.Config);
        }

        /// <summary>
        /// 输入秘钥
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keyinput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.Text == "输入秘钥")
                return;
            ResourcePool.Config.SubscriptionKey = tb.Text;
        }

        /// <summary>
        /// 输入区域标识符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reginput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.Text == "输入区域标识符")
                return;
            ResourcePool.Config.Region = tb.Text;
        }

        /// <summary>
        /// 下一步按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStep(object sender, RoutedEventArgs e)
        {
            var bu = sender as Button;
            if (bu.Name == "keynetxbutton")
            {
                configKey.Visibility = Visibility.Hidden;
                configSpeech.Visibility = Visibility.Visible;
            }
            else if (bu.Name == "spnextbutton")
            {
                configSpeech.Visibility = Visibility.Hidden;
                convertgrid.Visibility = Visibility.Visible;
            }
            else if (bu.Name == "skipupdate")
            {
                update.Visibility = Visibility.Hidden;
                configKey.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 上一步按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastStep(object sender, RoutedEventArgs e)
        {
            var bu = sender as Button;
            if (bu.Name == "splastbutton")
            {
                configSpeech.Visibility = Visibility.Hidden;
                configKey.Visibility = Visibility.Visible;
            }
            if (bu.Name == "connextstep")
            {
                configSpeech.Visibility = Visibility.Visible;
                convertgrid.Visibility = Visibility.Hidden;
            }

        }

        /// <summary>
        /// 滑条事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            if (slider.Name == "degree")
            {
                degreelable.Content = $"语气强度{slider.Value:F1}";
                if (slider.Value > 0)
                    ResourcePool.Config.SpeechConf.SpeechDegree = slider.Value;
            }
            else if (slider.Name == "rate")
            {
                ratelable.Content = $"语速{slider.Value:F1}";
                //if (slider.Value > 0)
                ResourcePool.Config.SpeechConf.SpeechRate = slider.Value;
            }
        }

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_OpenFolder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ResourcePool.Config.SavePath = dialog.SelectedPath.TrimEnd('\\');
                flodertext.Text = dialog.SelectedPath.TrimEnd('\\');
            }
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_OpenFile(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "支持的文件 (*.txt, *.csv)|*.txt;*.csv";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ResourcePool.Config.FilePath = dialog.FileName;
                filetext.Text = dialog.FileName;
                ResourcePool.TextResource.DicName = dialog.SafeFileName.Split('.')[0];
                var res = FileHelper.ReadFileToResource(dialog.FileName);
                if (!res.Success)
                {
                    MessageBox.Show(res.Message);
                }
                else
                {
                    datagrid.ItemsSource = null;
                    datagrid.ItemsSource = ResourcePool.TextResource;
                }
            }
        }

        /// <summary>
        /// 开始转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_StartTask(object sender, RoutedEventArgs e)
        {
            start.IsEnabled = false;
            var res = await ResourcePool.StartConvertingAsync();
            start.IsEnabled = true;
            if (!res.Success)
                MessageBox.Show(res.Message);
        }

        /// <summary>
        /// 停止转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_AbordTask(object sender, RoutedEventArgs e)
        {
            ResourcePool.AbordTask();
        }

        /// <summary>
        /// 加载转换页面时，按照已保存的文件路径刷新表格数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertgrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(filetext.Text))
            {
                var res = FileHelper.ReadFileToResource(filetext.Text);
                if (res.Success)
                    datagrid.ItemsSource = ResourcePool.TextResource;
            }
        }

        private delegate void WindowDelegate();

        /// <summary>
        /// 更新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://gitee.com/n-i-n-g/OpenTTS/releases");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //加载配置文件上一次保存的语音配置
            var conf = Config.LoadConfig().SpeechConf;

            //设置语言
            language.SelectedIndex = (int)conf.SpeechLang;
            //设置讲话人
            speechSelect.SelectedIndex = ResourcePool.SpeechResource.Where(l => l.Language == conf.SpeechLang).Select(l => l.Code).ToList().IndexOf(conf.SpeechCode);
            //设置语气
            styleSelect.SelectedIndex = (int)conf.SpeechStyle;
            //设置语气强度
            degree.Value = conf.SpeechDegree;
            //设置语速
            rate.Value = conf.SpeechRate;
        }
    }
}
