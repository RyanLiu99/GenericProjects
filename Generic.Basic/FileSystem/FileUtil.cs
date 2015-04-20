
using System;

namespace Generic.Basic.FileSystem
{
    public static class FileUtil
    {
        public static string AddTimeStampToFileName(string fileName)
        {
            var timeStamp = DateTime.UtcNow.ToString("-yyyyMMddHHmmssffff");
            var posOfDot = fileName.LastIndexOf(".");

            if (posOfDot != -1)
            {
                var name = fileName.Substring(0, posOfDot);
                var extWithDot = fileName.Substring(posOfDot);

                return  name + timeStamp + extWithDot;
            }
            else //no ext
            {
                return fileName + timeStamp;
            }
        }
    }
}
