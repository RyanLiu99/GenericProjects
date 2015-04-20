using System.Text;

namespace Generic.Basic.FileSystem.Csv
{
    public class CsvCfg
    {
        /// <summary>
        /// Gets or sets the delimiter used to separate fields.  Default is ',';
        /// </summary>
        public virtual string Delimiter { get; set; }

        /// <summary>
        ///  Gets or sets the encoding used when counting bytes. Default UTF8
        /// </summary>
        public virtual Encoding Encoding { get; set; }

       
        
        /// <summary>
        /// Gets or sets a value indicating if blank lines should be ignored when reading.
        /// True to ignore, otherwise false. Default is true.
        /// </summary>
        public virtual bool IgnoreBlankLines { get; set; }


        public CsvCfg()
        {
            Delimiter = ",";
            Encoding = Encoding.UTF8;
            IgnoreBlankLines = true;

        }
        
    }
}
