using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MyAnimeWorld.Common.Utilities.Censor
{
    public static class WordsFilter
    {
        public static string CensorComment(string content)
        {
            using (var reader = new StreamReader(@"~/../../MyAnimeWorld.Common/Utilities/Censor/full-list-of-bad-words_text-file_2018_07_30.csv"))
            {
                var words = content.Split();

                while (!reader.EndOfStream)
                {
                    var badWord = reader.ReadLine();
                    var newWord = string.Join("", 
                        badWord[0].ToString(),
                        new string('*',badWord.Length - 2),
                        badWord[badWord.Length - 1].ToString());

                    content = content.Replace(badWord, newWord,true,CultureInfo.InvariantCulture);
                }
            }

            return content;
        }
    }
}
