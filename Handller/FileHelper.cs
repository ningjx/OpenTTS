using SpeechGenerator.Models;
using System;
using System.IO;

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
    }
}
