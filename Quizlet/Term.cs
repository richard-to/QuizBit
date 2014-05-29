using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit.Quizlet
{
    class Term
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("term")]
        public string Value { get; set; }

        [JsonProperty("definition")]
        public string Definition { get; set; }
    }
}
