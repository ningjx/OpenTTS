using SpeechGenerator.Handller;
using SpeechGenerator.Models;
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
            speechSelect.ItemsSource = ResourcePool.Instance.SpeechResource.Select(t => t.Name).ToArray();
            keyinput.Text = ResourcePool.Instance.Config.SubscriptionKey;
            reginput.Text = ResourcePool.Instance.Config.Region;

            if (string.IsNullOrEmpty(ResourcePool.Instance.Config.SubscriptionKey))
            {
                configSpeech.Visibility = Visibility.Hidden;
                configKey.Visibility = Visibility.Visible;
            }

            speechSelect.SelectedValue = ResourcePool.Instance.SpeechResource.First(t => t.Code == ResourcePool.Instance.Config.SpeechConf.SpeechName).Name;
            styleSelect.SelectedValue = ResourcePool.Instance.Config.SpeechConf.SpeechStyle;
            degree.Value = ResourcePool.Instance.Config.SpeechConf.SpeechDegree;
            rate.Value = ResourcePool.Instance.Config.SpeechConf.SpeechRate;
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
            TextItem text = new TextItem { FileName = "", Text = speechText.Text };
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
    }
}
