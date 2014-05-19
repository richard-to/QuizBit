using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit
{
    class QuizletAccessToken
    {
        public string AccessToken { get; set; }
        public string UserID { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(AccessToken) && !String.IsNullOrEmpty(UserID);
        }
    }
}
