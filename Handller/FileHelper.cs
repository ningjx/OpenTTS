using NAudio.Wave;
using SpeechGenerator.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpeechGenerator.Handller
{
    internal static class FileHelper
    {
        /// <summary>
        /// 从文件路径中提取不含后缀的文件名
        /// </summary>
        public static readonly Regex PathRegex = new Regex(@"(?<=\\).*(?=\.txt)");
        private static readonly Regex LineRegex = new Regex(@"(.+\..+?)( +)(.+)");

        public static Result SaveFile(string path, string dicName, string fileName, byte[] bytes)
        {
            path = path.TrimEnd('\\');
            //if (!Directory.Exists(path))
            //{
            //    return Result<string>.Fail(null, $"保存路径{path}不存在");
            //}

            var dic = new DirectoryInfo($"{path}\\{dicName}");
            if (!dic.Exists)
                dic.Create();

            using (var reader = new CAudioFileReader($"{path}\\{dicName}\\{fileName}", bytes))
            {
                try
                {
                    if (ResourcePool.Config.Volume != 1f)
                        reader.Volume = ResourcePool.Config.Volume;

                    WaveFileWriter.CreateWaveFile16($"{path}\\{dicName}\\{fileName}", reader);
                    return Result.Sucess();
                }
                catch (Exception ex)
                {
                    return Result.Fail(ex.Message);
                }
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="filename">文件名</param>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static Result SaveFile(string path, string filename, string text)
        {
            path = path.TrimEnd('\\');
            //if (!Directory.Exists(path))
            //{
            //    return Result<string>.Fail(null, $"保存路径{path}不存在");
            //}

            var dic = new DirectoryInfo(path);
            if (!dic.Exists)
                dic.Create();

            try
            {
                File.WriteAllText($"{path}\\{filename}", text);
                return Result.Sucess();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static Result ReadFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Result.Fail("文件路径为空");
            if (!File.Exists(path))
                return Result.Fail("文件不存在");

            try
            {
                var texts = File.ReadAllLines(path);
                return Result.Sucess("", texts);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 将要翻译的文本，读取到<see cref="ResourcePool"/>中，共享资源都在该类
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static Result ReadFileToResource(string path)
        {
            try
            {
                var readfile = ReadFile(path);
                var createDice = PathRegex.Match(path).Value.Split('\\').LastOrDefault() ;

                ResourcePool.TextResource.DicName = createDice;
                if (!readfile.Success)
                {
                    return readfile;
                }
                else
                {
                    ResourcePool.TextResource.Clear();
                    var textLines = (string[])readfile.Data;
                    foreach (var line in textLines)
                    {
                        if (LineRegex.IsMatch(line))
                        {
                            var items = LineRegex.Matches(line)[0].Groups;
                            if (!string.IsNullOrWhiteSpace(items[3].Value))
                                ResourcePool.TextResource.Add(new TextItem(items[1].Value, items[3].Value));
                        }
                    }
                }
                return Result.Sucess();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public static Result FileExist(string path, string dicName, string fileName)
        {
            var exist = File.Exists($"{path}\\{dicName}\\{fileName}");
            return new Result() { Success = exist };
        }
    }
}
