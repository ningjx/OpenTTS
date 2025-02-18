using NAudio.Wave;
using OpenTTS.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;

namespace OpenTTS.Handller
{
    internal static class FileHelper
    {
        /// <summary>
        /// 从文件路径中提取不含后缀的文件名
        /// </summary>
        public static readonly Regex PathRegex = new Regex(@"(?<=\\).*(?=\.+)");
        private static readonly Regex LineRegex = new Regex(@"(.+\..+?)( +)(.+)");

        public static Result SaveFile(string path, string dicName, string fileName, byte[] bytes)
        {
            path = path.TrimEnd('\\');
            //if (!Directory.Exists(path))
            //{
            //    return Result<string>.Fail(null, $"保存路径{path}不存在");
            //}
            //如果保存文件名不是.wav后缀，就改成wav后缀
            fileName.TrimStart('\\');
            if (!fileName.EndsWith(".wav"))
                fileName += ".wav";
            
            //如果文件名上包含目录，那么创建目录
            var dic = new DirectoryInfo(Path.GetDirectoryName($"{path}\\{dicName}\\{fileName}"));
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
        public static Result ReadTxtFile(string path)
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
                //读取svg
                if (path.EndsWith(".csv"))
                    return ReadCsvToResource(path);

                if (path.EndsWith(".txt"))
                    return ReadTxtToResource(path);

                return Result.Fail();
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

        public static Result ReadCsvToResource(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Result.Fail("文件路径为空");
            if (!File.Exists(path))
                return Result.Fail("文件不存在");

            //将提供的待转换文件的文件名，作为转换后文件存放的文件夹名字
            var createDice = PathRegex.Match(path).Value.Split('\\').LastOrDefault();

            ResourcePool.TextResource.DicName = createDice;
            ResourcePool.TextResource.Clear();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true, // 文件包含标题行
                MissingFieldFound = null, // 忽略缺失字段
                PrepareHeaderForMatch = args => args.Header.ToLower() // 列名不区分大小写
            };

            var csvRecords = new List<CsvRow>();
            using (var reader = new StreamReader(path))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        var record = csv.GetRecord<CsvRow>();
                        csvRecords.Add(record);
                    }
                }
            }

            foreach (var csvLine in csvRecords)
            {
                if (string.IsNullOrEmpty(csvLine.Filename) || string.IsNullOrEmpty(csvLine.Translation))
                    continue;

                string fileName = string.Empty;
                if (!string.IsNullOrEmpty(csvLine.Path))
                    fileName = csvLine.Path + "\\";

                ResourcePool.TextResource.Add(new TextItem(fileName + csvLine.Filename, csvLine.Translation));
            }

            return Result.Sucess();
        }
        public static Result ReadTxtToResource(string path)
        {
            var readfileRes = ReadTxtFile(path);
            if (!readfileRes.Success)
                return readfileRes;

            //将提供的待转换文件的文件名，作为转换后文件存放的文件夹名字
            var createDice = PathRegex.Match(path).Value.Split('\\').LastOrDefault();

            ResourcePool.TextResource.DicName = createDice;
            ResourcePool.TextResource.Clear();

            var textLines = (string[])readfileRes.Data;
            foreach (var line in textLines)
            {
                if (LineRegex.IsMatch(line))
                {
                    var items = LineRegex.Matches(line)[0].Groups;
                    if (!string.IsNullOrWhiteSpace(items[3].Value))
                        ResourcePool.TextResource.Add(new TextItem(items[1].Value, items[3].Value));
                }
            }

            return Result.Sucess();
        }
    }
}
