using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTS.Models
{
    public class CsvRow
    {
        public string Translation { get; set; }
        public string Path { get; set; }
        public string Filename { get; set; }
    }
    //public sealed class CsvRowMap : ClassMap<CsvRow>
    //{
    //    public CsvRowMap()
    //    {
    //        Map(m => m.Translation).Name("Translation");
    //        Map(m => m.Path).Name("Path");
    //        Map(m => m.Filename).Name("Filename");
    //    }
    //}
}
