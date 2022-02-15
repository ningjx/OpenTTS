using SpeechGenerator.Handller;
using SpeechGenerator.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            speechSelect.ItemsSource = ResourcePool.Instance.SpeechResource.Select(t => t.Name).ToArray();
            keyinput.Text = string.IsNullOrEmpty(ResourcePool.Instance.Config.SubscriptionKey) ? keyinput.Text : ResourcePool.Instance.Config.SubscriptionKey;
            reginput.Text = string.IsNullOrEmpty(ResourcePool.Instance.Config.Region) ? reginput.Text : ResourcePool.Instance.Config.Region;

            if (string.IsNullOrEmpty(ResourcePool.Instance.Config.SubscriptionKey))
            {
                configSpeech.Visibility = Visibility.Hidden;
                configKey.Visibility = Visibility.Visible;
            }

            speechSelect.SelectedValue = ResourcePool.Instance.SpeechResource.First(t => t.Code == ResourcePool.Instance.Config.SpeechConf.SpeechName).Name;
            styleSelect.SelectedValue = ResourcePool.Instance.Config.SpeechConf.SpeechStyle;
            degree.Value = ResourcePool.Instance.Config.SpeechConf.SpeechDegree;
            rate.Value = ResourcePool.Instance.Config.SpeechConf.SpeechRate;

            flodertext.Text = string.IsNullOrEmpty(ResourcePool.Instance.Config.SavePath) ? flodertext.Text : ResourcePool.Instance.Config.SavePath;
            filetext.Text = string.IsNullOrEmpty(ResourcePool.Instance.Config.FilePath) ? filetext.Text : ResourcePool.Instance.Config.FilePath;
        }

        private void styleSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = sender as ComboBox;
            ResourcePool.Instance.Config.SpeechConf.SpeechStyle = (string)combox.SelectedValue;
        }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextItem text = new TextItem("", speechText.Text);
            var res = SpeechConverter.Instance.CreateAudioFromText(text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Config.SaveConfig(ResourcePool.Instance.Config);
        }

        private void keyinput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.Text == "输入秘钥")
                return;
            ResourcePool.Instance.Config.SubscriptionKey = tb.Text;
        }

        private void reginput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.Text == "输入区域标识符")
                return;
            ResourcePool.Instance.Config.Region = tb.Text;
        }

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

        }

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

        private void degree_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            if (slider.Name == "degree")
            {
                degreelable.Content = $"语气强度{slider.Value}";
                if (slider.Value > 0)
                    ResourcePool.Instance.Config.SpeechConf.SpeechDegree = slider.Value;
            }
            else if (slider.Name == "rate")
            {
                ratelable.Content = $"语速{slider.Value}";
                if (slider.Value > 0)
                    ResourcePool.Instance.Config.SpeechConf.SpeechRate = slider.Value;
            }
        }

        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ResourcePool.Instance.Config.SavePath = dialog.SelectedPath.TrimEnd('\\');
                flodertext.Text = dialog.SelectedPath.TrimEnd('\\');
            }
            else
            {
                //点击了取消
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ResourcePool.Instance.Config.FilePath = dialog.FileName;
                filetext.Text = dialog.FileName;
                var readfile = FileHelper.ReadFile(dialog.FileName);

                if (!readfile.Success)
                    MessageBox.Show(readfile.Message);
                else
                {
                    ResourcePool.Instance.TextResource.DicName = dialog.SafeFileName.Split('.')[0];
                    var data = (string[])readfile.Data;

                    data.ToList().ForEach(x =>
                    {
                        var item = x.Split(' ');
                        ResourcePool.Instance.TextResource.Add(new TextItem(item[0], item[1]));
                    });
                    datagrid.ItemsSource = ResourcePool.Instance.TextResource;
                }
            }
            else
            {
                //点击了取消
            }
        }

        private void textbox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var box = sender as TextBox;
            string message = "";
            switch (box.Name)
            {
                case "keyinput":
                    message = "输入秘钥";
                    break;
                case "reginput":
                    message = "输入区域标识符";
                    break;
                case "flodertext":
                    message = "保存路径";
                    break;
                case "filetext":
                    message = "资源文件";
                    break;
                default: break;
            }
            //box.ToolTip = message;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new ProcessAudio(ConvertAudio));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        public void ConvertAudio()
        {
            foreach (var item in ResourcePool.Instance.TextResource)
            {
                Thread.Sleep(1000);
                var res = SpeechConverter.Instance.CreateAudioFileFromText(ResourcePool.Instance.TextResource.DicName, item);
                if (!res.Success)
                {
                    MessageBox.Show(res.Message);

                    break;

                }
            }
        }

        public delegate void ProcessAudio();
    }
}
