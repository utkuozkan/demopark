using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ConsoleApp
{
    class Program
    {
        private static Dictionary<string, List<int>> wordsAndIndexes = null;
        static void Main(string[] args)
        {
            string folder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + @"\";
            string full_file_path = folder + ConfigurationManager.AppSettings["txt_file_name"];

            DownloadBook(full_file_path);

            var allLines = File.ReadAllLines(full_file_path).ToList();
            //find index to read txt from specific line
            //I started to read txt from ETYMOLOGY to the last paragraph of Epilogue
            int first_index = allLines.FindLastIndex(x => x.EndsWith("ETYMOLOGY."));
            var last_index = allLines.LastIndexOf("children, only found another orphan.") + 1;
            allLines = allLines.Skip(first_index).Take(last_index - first_index).ToList();

            wordsAndIndexes = new Dictionary<string, List<int>>();

            foreach (var item in allLines)
            {
                if (string.IsNullOrEmpty(item)) continue;
                CalculateEachWord(item);
            }
            WriteXml(folder + ConfigurationManager.AppSettings["xml_file_name"]);
        }
        /// <summary>
        /// Writes the XML.
        /// </summary>
        private static void WriteXml(string filePath)
        {
            WordSettings<string, string> dict = new WordSettings<string, string>();
            foreach (KeyValuePair<string, List<int>> item in wordsAndIndexes)
                dict.Add(item.Key, item.Value.Count.ToString());

            XmlSerializer serializer = new XmlSerializer(typeof(WordSettings<string, string>));
            TextWriter textWriter = new StreamWriter(filePath);
            serializer.Serialize(textWriter, dict);
            textWriter.Close();
        }

        /// <summary>
        /// Calculates the each word.
        /// </summary>
        /// <param name="myString">My string.</param>
        private static void CalculateEachWord(string myString)
        {
            myString = CheckIllegalChars(myString).ToLowerInvariant();
            var words = myString.Split(' ');

            int index = 0;
            for (index = 0; index < words.Length; index++)
            {
                var word = (string)words.GetValue(index);

                if (string.IsNullOrEmpty(word)) continue;

                if (!wordsAndIndexes.ContainsKey(word))
                {
                    wordsAndIndexes.Add(word, new List<int>());
                }
                wordsAndIndexes[word].Add(index);
            }
        }
        /// <summary>
        /// Checks the illegal chars.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string CheckIllegalChars(string input)
        {
            // make regex of illegal characters list
            string illegalChars = "[" + Regex.Escape(new string(new char[] {
                '.', '?', ';', ',', '(', ')', '_', '—', '"', '‘', '“', '’', '!' ,':'})) + "]";
            // replace characters by space
            input = Regex.Replace(input, illegalChars, " ");
            // replace double spaces by a single space
            input = Regex.Replace(input, "\\s\\s+", " ");
            return input;
        }


        /// <summary>
        /// Downloads the book.
        /// </summary>
        private static void DownloadBook(string filePath)
        {
            if (File.Exists(filePath)) return;

            WebClient client = new WebClient();
            string dnlad = client.DownloadString(ConfigurationManager.AppSettings["url"]);
            File.WriteAllText(filePath, dnlad);
        }
    }
}
