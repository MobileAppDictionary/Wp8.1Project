using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using H2Dict.Model;
using H2Dict.Helper;

namespace H2Dict.ViewModel
{
    public class Dict
    {
        private ListWords _lstWord;

        public Dict()
        {
            _lstWord = new ListWords();
        }

        public async Task LoadListWords()
        {
            _lstWord = await DataHelper.LoadListWords();
        }

        public async Task<string> Search(string key)
        {
            string res = null;

            int ind = _lstWord.LstKey.FindIndex(x => x.Equals(key));
            if (ind > 0)
            {
                int offset = GetDemicalValue(_lstWord.LstOffset[ind]);
                int length = GetDemicalValue(_lstWord.LstLength[ind]);
            }
            

            return res;
        }

        public List<string> GetSuggestion(string key)
        {
            List<string> lstString = new List<string>();
            int len = key.Length;

            if (len < 3)
                return lstString;

            foreach (string word in _lstWord.LstKey)
            {
                if (word.Length >= len && word.Substring(0, len).Equals(key))
                {
                    int ind = _lstWord.LstKey.FindIndex(x => x.Equals(word));
                    for (int i = 0; i < 4; i++)
                    {
                        lstString.Add(_lstWord.LstKey[ind + i]);
                    }
                }
            }

            return lstString;
        }

        private int GetDemicalValue(string str)
        {
            String base64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            int decValue = 0;
            int len = str.Length;
            for (int i = 0; i < len; i++)
            {
                int pos = base64.IndexOf(str[i]);
                decValue += (int)Math.Pow(64, len - i - 1) * pos;
            }
            return decValue;
        }

        private async Task<string> GetMeaning (int offset, int length)
        {
            string res = null;

            return res;
        }
    }
}
