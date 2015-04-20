using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Generic.Basic.String
{
    public static class StringUtil
    {

        public static string FixHtmlString(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            var cleanStr = RemoveHtml(inputString);

            cleanStr = RemoveSpecialChars(cleanStr, false);
            return cleanStr;
        }

        public static string RemoveSpecialChars(this string inputString, bool removeSpaceToo = true)
        {
            string replaceChar = removeSpaceToo ? string.Empty : " ";
            //replace tab 
            inputString = inputString.Replace("\t", replaceChar);
            inputString = inputString.Replace("&", replaceChar);            
            inputString = inputString.Replace("\r", replaceChar);
            inputString = inputString.Replace("\n", replaceChar);
            inputString = inputString.Replace("\\", replaceChar);
            inputString = inputString.Replace("/", replaceChar);
            inputString = inputString.Replace("+", replaceChar);

            if (removeSpaceToo) 
                inputString = inputString.Replace(" ", string.Empty);
            
            inputString = inputString.TrimToSingSpace();

            return inputString;
        }

        // A string method that removes html tags
        // with a regex pattern
        public static String RemoveHtml(String message)
        {
            // The pattern for a html tag
            String htmlTagPattern = "<(.|\n)+?>";
            // Create a regex object with the pattern 
            Regex objRegExp = new Regex(htmlTagPattern);
            // Replace html tag by an empty string
            message = objRegExp.Replace(message, " ");
            message = message.Replace("&nbsp;", " ");
            // Return the message without html tags
            return message.TrimToSingSpace();
        }

        public static string TrimToSingSpace(this string input)
        {
            if (input == null) return null;

            var pattern = @"\s+";
            Regex objRegExp = new Regex(pattern);
            var trimmed = objRegExp.Replace(input, " ");
            return trimmed;
        }

    }
}
