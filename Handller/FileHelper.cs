using SpeechGenerator.Models;
using System;
using System.IO;
using System.Linq;

namespace SpeechGenerator.Handller
{
    internal static class FileHelper
    {
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

            try
            {
                File.WriteAllBytes($"{path}\\{dicName}\\{fileName}", bytes);
                return Result.Sucess();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

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
