using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit.Quizlet
{
    class FlashcardSet
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("term_count")]
        public string TermCount { get; set; }

        [JsonProperty("create_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("modified_date")]
        public int ModifiedDate { get; set; }
    }
}
