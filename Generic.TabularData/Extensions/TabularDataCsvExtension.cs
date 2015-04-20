
using System.IO;
using System.Linq;
using System.Text;

namespace Generic.TabularData.Extensions
{
    public static class TabularDataCsvExtension
    {
        public static Stream ToCsvStream(this TabularTable data, Encoding encoding = null, string delimiter = ",")
        {
            
            if (encoding == null) encoding = Encoding.Unicode;
            
            MemoryStream stream = new MemoryStream();
            var writer = new StreamWriter(stream, encoding);
            //write header
            writer.WriteLine(string.Join(delimiter, data.cols.Select(c => c.lable)));
            //write rows
            foreach (var row in data.rows)
            {
                writer.WriteLine(string.Join(delimiter, row) );
            }
            writer.Flush();
            
            //imporve later
            stream.Seek(0, SeekOrigin.Begin);            
            return stream;
        }
    }
}
