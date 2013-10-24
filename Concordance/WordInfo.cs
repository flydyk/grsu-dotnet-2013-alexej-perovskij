using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concordance
{
    public class WordInfo : IComparable<WordInfo>
    {
        public string Word { get; private set; }
        public long Count { get; set; }
        SortedSet<int> pages;

        public WordInfo(string word)
        {
            Word = word;
            pages = new SortedSet<int>();
        }

        public void AddPage(int page)
        {
            pages.Add(page);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(string.Format("{0,-30}", Word));
            str.Append(Count + ":");

            foreach (var page in pages)
            {
                str.Append(" " + page);
            }

            return str.ToString();
        }
        
        public int CompareTo(WordInfo other)
        {
            return this.Word.CompareTo(other.Word);
        }
    }
}
