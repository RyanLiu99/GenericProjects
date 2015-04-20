using CsvHelper;
using CsvHelper.Configuration;
using Generic.Basic.Validations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Generic.Basic.FileSystem.Csv
{
    public static class CsvUtility
    {
        [Obsolete("suggest to use ReadCsvStream, so not coupled to local file system")]
        public static List<List<string>> ReadCsvFiles(string filePathName, out bool hasMoreLines, int? take = null, int? skip = 0, CsvCfg csvCfg = null)
        {
            
            if (take.HasValue)
            {
                var result = ReadCsvFiles(filePathName, take +1, skip, csvCfg).ToList();
                hasMoreLines = (result != null || result.Count > take.Value);
                if(hasMoreLines)
                    result.RemoveAt(result.Count -1);
                return result;
            }
            else
            {
                hasMoreLines = false;
                return ReadCsvFiles(filePathName, take, skip, csvCfg).ToList();
            }
         
        }
      

        [Obsolete("suggest to use ReadCsvStream, so not coupled to local file system")]
        public static IEnumerable<List<string>> ReadCsvFiles(string filePathName, int? take = null, int? skip = 0, CsvCfg csvCfg = null)
        {           
            using (var stream = File.OpenRead(filePathName))   // File.OpenText(filePathName))
            {

                return ReadCsvStream(stream, take, skip, csvCfg);
            }
        }

        public static List<List<string>> ReadCsvStream(Stream csvSteam, out bool hasMoreLines, int? take = null, int? skip = 0, CsvCfg csvCfg = null)
        {

            if (take.HasValue)
            {
                var result = ReadCsvStream(csvSteam, take + 1, skip, csvCfg).ToList();
                hasMoreLines = (result != null || result.Count > take.Value);
                if (hasMoreLines)
                    result.RemoveAt(result.Count - 1);
                return result;
            }
            else
            {
                hasMoreLines = false;
                return ReadCsvStream(csvSteam, take, skip, csvCfg).ToList();
            }

        }

        public static IEnumerable<List<string>> ReadCsvStream(Stream csvSteam, int? take = null, int? skip = 0, CsvCfg csvCfg = null)
        {
            CheckArgument.NotNull(csvSteam, "csvSteam");

            if (csvCfg == null)
                csvCfg = new CsvCfg();

            using (var textReader = new StreamReader(csvSteam))
            {
                CsvConfiguration cfg = new CsvConfiguration()
                {
                    Encoding = csvCfg.Encoding,
                    Delimiter = csvCfg.Delimiter,
                    IgnoreBlankLines = csvCfg.IgnoreBlankLines
                };

                using (var parser = new CsvParser(textReader, cfg))
                {

                    while (skip-- > 0)
                    {
                        if (parser.Read() == null)
                        {
                            yield break;
                        }

                    }

                    int count = 0;
                    while (count++ < take.GetValueOrDefault(int.MaxValue))
                    {
                        var row = parser.Read();
                        if (row == null)
                        {
                            yield break;
                        }

                        yield return row.ToList();
                    }

                }
            }
        }
    }
}
