using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit
{
    public interface IDataProvider
    {
        string Fetch(string key);
        T Fetch<T>(string key);
        bool Contains(string key);
        void Store(string key, string value);
        void Store<T>(string key, T value);
        void Remove(string key);
    }
}
