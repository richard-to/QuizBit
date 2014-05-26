using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBit
{
    class IsolatedStorageDataProvider : IDataProvider
    {
        private IsolatedStorageSettings _appSettings;

        public IsolatedStorageDataProvider()
        {
            _appSettings = IsolatedStorageSettings.ApplicationSettings;
        }

        public string Fetch(string key)
        {
            if (_appSettings.Contains(key))
            {
                return (string)_appSettings[key];
            }
            else
            {
                return null;
            }
        }

        public T Fetch<T>(string key)
        {
            if (_appSettings.Contains(key))
            {
                string value = (string)_appSettings[key];
                return JsonConvert.DeserializeObject<T>(value);
            }
            else
            {
                return default(T);
            }
        }

        public bool Contains(string key)
        {
            return _appSettings.Contains(key);
        }

        public void Store(string key, string value)
        {
            if (_appSettings.Contains(key))
            {
                _appSettings[key] = value;
            }
            else
            {
                _appSettings.Add(key, value);
            }
            _appSettings.Save();
        }

        public void Store<T>(string key, T value)
        {
            string serializedValue = JsonConvert.SerializeObject(value);
            if (_appSettings.Contains(key))
            {
                _appSettings[key] = serializedValue;
            }
            else
            {
                _appSettings.Add(key, serializedValue);
            }
            _appSettings.Save();
        }

        public void Remove(string key)
        {
            if (_appSettings.Contains(key))
            {
                _appSettings.Remove(key);
                _appSettings.Save();
            }
        }
    }
}
