using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Common
{
    public class BadWordsList
    {
        public int deviations { get; set; }
        public int end { get; set; }
        public int info { get; set; }
        public string original { get; set; }
        public int replacedLen { get; set; }
        public int start { get; set; }
        public string word { get; set; }
    }

    public class BadWord
    {
        public List<BadWordsList> bad_words_list { get; set; }
        public int bad_words_total { get; set; }
        public string censored_content { get; set; }
        public string content { get; set; }
    }
}
