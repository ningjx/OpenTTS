using NAudio.Wave;
using SpeechGenerator.Models;
using System;
using System.IO;
using System.Linq;

namespace SpeechGenerator.Handller
{
    internal static class FileHelper
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="dicName">次级文件夹</param>
        /// <param name="fileName">文件名</param>
        /// <param name="bytes">字节流</param>
        /// <returns></returns>
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
                    if (ResourcePool.Instance.Config.Volume != 1f)
                        reader.Volume = ResourcePool.Instance.Config.Volume;

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

                if (!readfile.Success)
                {
                    return readfile;
                }
                else
                {
                    var data = (string[])readfile.Data;

                    data.ToList().ForEach(x =>
                    {
                        var item = x.Split(' ');
                        if (item.Length > 1)
                            ResourcePool.Instance.TextResource.Add(new TextItem(item[0], item[1]));
                    });
                }
                return Result.Sucess();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
