using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace SpeechGenerator
{
    public static class Helper
    {
        /// <summary>
        /// 从Gitee检查最新文件中的版本号，以实现检查更新
        /// </summary>
        /// <returns></returns>
        public static bool CheckUpdate()
        {
            try
            {
                var client = new HttpClient();
                using (var request = new HttpRequestMessage(HttpMethod.Get, "https://gitee.com/n-i-n-g/SpeechGenerator/blob/master/Properties/AssemblyInfo.cs"))
                {
                    var response = client.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var dataString = response.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrEmpty(dataString))
                        {
                            var re = new Regex(@"(?<=AssemblyVersion\("")\d\.\d\.\d\.\d(?<!\""\))");
                            var version = re.Match(dataString).Value;
                            if (!string.IsNullOrEmpty(version))
                            {
                                var items = version.Split('.');
                                var curVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                                var newVer = new Version(Convert.ToInt32(items[0]), Convert.ToInt32(items[1]), Convert.ToInt32(items[2]), Convert.ToInt32(items[3]));
                                return newVer > curVer;
                            }
                        }
                    }
                    return false;
                }
            }
            catch { return false; }
        }
    }
}
