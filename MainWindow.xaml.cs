using SpeechGenerator.Handller;
using SpeechGenerator.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SpeechGenerator
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
            Top = ResourcePool.Instance.Config.Top;
            Left = ResourcePool.Instance.Config.Left;

            //绑定支持的语言风格
            speechSelect.ItemsSource = ResourcePool.Instance.SpeechResource.Select(t => t.Name).ToArray();

            //从配置中加载保存的秘钥和区域代码
            keyinput.Text = string.IsNullOrEmpty(ResourcePool.Instance.Config.SubscriptionKey) ? keyinput.Text : ResourcePool.Instance.Config.SubscriptionKey;
            reginput.Text = string.IsNullOrEmpty(ResourcePool.Instance.Config.Region) ? reginput.Text : ResourcePool.Instance.Config.Region;

            //根据条件切换启动时展示的页面
            if (Helper.CheckUpdate())
                update.Visibility = Visibility.Visible;
            else if (string.IsNullOrEmpty(ResourcePool.Instance.Config.SubscriptionKey))
                configKey.Visibility = Visibility.Visible;
            else
                configSpeech.Visibility = Visibility.Visible;

            //从配置中加载选择的语言风格、语气、语气强度和语速
            speechSelect.SelectedValue = ResourcePool.Instance.SpeechResource.First(t => t.Code == ResourcePool.Instance.Config.SpeechConf.SpeechName).Name;
            styleSelect.SelectedValue = ResourcePool.Instance.Config.SpeechConf.SpeechStyle;
            degree.Value = ResourcePool.Instance.Config.SpeechConf.SpeechDegree;
            rate.Value = ResourcePool.Instance.Config.SpeechConf.SpeechRate;

            //从配置中加载保存文件夹和资源文件路径
            flodertext.Text = string.IsNullOrEmpty(ResourcePool.Instance.Config.SavePath) ? flodertext.Text : ResourcePool.Instance.Config.SavePath;
            filetext.Text = string.IsNullOrEmpty(ResourcePool.Instance.Config.FilePath) ? filetext.Text : ResourcePool.Instance.Config.FilePath;

            //订阅转换任务的事件
            ResourcePool.Instance.TextRowChanged += Instance_TextRowChanged;
            ResourcePool.Instance.TitleChange += Instance_TitleChange;
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
        /// 选择语气
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void styleSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = sender as ComboBox;
            ResourcePool.Instance.Config.SpeechConf.SpeechStyle = (string)combox.SelectedValue;
        }

        /// <summary>
        /// 选择语言风格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void speechSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = sender as ComboBox;
            var selectValue = (string)combox.SelectedValue;

            if (string.IsNullOrEmpty(selectValue))
                return;

            var voice = ResourcePool.Instance.SpeechResource.FirstOrDefault(t => t.Name == (string)combox.SelectedValue);
            ResourcePool.Instance.Config.SpeechConf.SpeechName = voice.Code;

            styleSelect.ItemsSource = voice.Styles?.Select(t => t.Style).ToArray();
        }

        /// <summary>
        /// 试听按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_ShiTing(object sender, RoutedEventArgs e)
        {
            TextItem text = new TextItem("", speechText.Text);
            ResourcePool.Instance.StartTask(text);
        }

        /// <summary>
        /// 退出程序时保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ResourcePool.Instance.Config.Top = Top;
            ResourcePool.Instance.Config.Left = Left;
            Config.SaveConfig(ResourcePool.Instance.Config);
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
            ResourcePool.Instance.Config.SubscriptionKey = tb.Text;
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
            ResourcePool.Instance.Config.Region = tb.Text;
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
                    ResourcePool.Instance.Config.SpeechConf.SpeechDegree = slider.Value;
            }
            else if (slider.Name == "rate")
            {
                ratelable.Content = $"语速{slider.Value:F1}";
                //if (slider.Value > 0)
                ResourcePool.Instance.Config.SpeechConf.SpeechRate = slider.Value;
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
                ResourcePool.Instance.Config.SavePath = dialog.SelectedPath.TrimEnd('\\');
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

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ResourcePool.Instance.Config.FilePath = dialog.FileName;
                filetext.Text = dialog.FileName;
                ResourcePool.Instance.TextResource.DicName = dialog.SafeFileName.Split('.')[0];
                var res = FileHelper.ReadFileToResource(dialog.FileName);
                if (!res.Success)
                {
                    MessageBox.Show(res.Message);
                }
                else
                {
                    datagrid.ItemsSource = ResourcePool.Instance.TextResource;
                }
            }
        }

        /// <summary>
        /// 开始转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_StartTask(object sender, RoutedEventArgs e)
        {
            ResourcePool.Instance.StartTask();
        }

        /// <summary>
        /// 停止转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_AbordTask(object sender, RoutedEventArgs e)
        {
            ResourcePool.Instance.AbordTask();
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
                    datagrid.ItemsSource = ResourcePool.Instance.TextResource;
            }
        }

        private delegate void WindowDelegate();

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void styleSelect_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var com = sender as ComboBox;
            var selectStyle = Convert.ToString(com.SelectedValue);
            var selectSpeech = Convert.ToString(speechSelect.SelectedValue);
            if (!string.IsNullOrEmpty(selectStyle) && !string.IsNullOrEmpty(selectSpeech))
            {
                com.ToolTip = ResourcePool.Instance.SpeechResource.FirstOrDefault(t => t.Name == selectSpeech)?.Styles.FirstOrDefault(t => t.Style == selectStyle)?.Description;
            }
        }

        /// <summary>
        /// 更新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://gitee.com/n-i-n-g/OpenTTS/releases");
        }
    }
}
