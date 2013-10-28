using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Concordance
{
    public class Concordance
    {
        private const int DEFAULT_LINES_PER_PAGE = 40;
        SortedDictionary<char, SortedDictionary<string, WordInfo>> concordance;
        string[] Text;
        private int linesPerPage = DEFAULT_LINES_PER_PAGE;
        

        public static int DefaultLinesPerPage
        {
            get { return DEFAULT_LINES_PER_PAGE; }
        }
        

        public int LinesPerPage
        {
            get { return linesPerPage; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Value must be greater than 0");
                linesPerPage = value;
            }
        }


        public Concordance(string path, Encoding encoding)
        {
            if (encoding == null) encoding = Encoding.Default;
            try
            {
                Text = File.ReadAllLines(path, encoding);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Concordance(string[] text)
        {
            Text = text;
        }

        public void GenerateConcordance()
        {
            if (Text == null) return;
            concordance = new SortedDictionary<char, SortedDictionary<string, WordInfo>>();

            int page = 0;
            Regex splitR = new Regex(@"\W+", RegexOptions.IgnoreCase);

            for (int i = 0; i < Text.Length; i++)
            {
                page = (i % linesPerPage == 0) ? page + 1 : page;

                string[] words = Regex.Split(Text[i], splitR.ToString());
                ToLowerCase(words);
                FillConcordance(page, words);
            }
        }

        private void ToLowerCase(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].ToLower();
            }
        }

        private void FillConcordance(int page, string[] words)
        {
            foreach (var word in words)
            {
                if (string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word)) continue;
                if (!concordance.ContainsKey(word[0]))
                    concordance[word[0]] = new SortedDictionary<string, WordInfo>();

                if (!concordance[word[0]].ContainsKey(word))
                    concordance[word[0]][word] = new WordInfo(word) { Count = 0 };

                concordance[word[0]][word].AddPage(page);
                concordance[word[0]][word].Count++;
            }
        }

        public void PrintToFile(string path, string fileName)
        {
            using (StreamWriter wr = new StreamWriter(Path.Combine(path, fileName), false, Encoding.Default))
            {
                foreach (var letter in concordance.Keys)
                {
                    wr.WriteLine("===============  " + char.ToUpper(letter) + "  ===============");
                    foreach (var wordInfo in concordance[letter].Values)
                    {
                        wr.WriteLine(wordInfo.ToString());
                    }
                }
            }
        }

        public void PrintToFile(string fileName)
        {
            PrintToFile(Environment.CurrentDirectory, fileName);
        }
    }
}
