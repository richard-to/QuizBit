using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit.Quizlet
{
    class User
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("account_type")]
        public string AccountType { get; set; }

        [JsonProperty("sign_up_date")]
        public int SignUpDate { get; set; }

        [JsonProperty("profile_image")]
        public string ProfileImage { get; set; }

        [JsonProperty("sets")]
        public List<FlashcardSet> Sets { get; set; }
    }
}
