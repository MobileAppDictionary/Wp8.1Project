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
        private List<string> _lstTranslatedWords; 
        public Dict()
        {
            _lstWord = new ListWords();
            _lstTranslatedWords = new List<string>();
        }

        public ListWords LstWord
        {
            get { return _lstWord; }
        }

        public List<string> LstTranslatedWords
        {
            get { return _lstTranslatedWords; }
        }

        public async Task LoadListWords()
        {
            _lstWord = await DataHelper.LoadListWords();
        }

        public async Task<string> Search(string key)
        {
            string res = null;

            int ind = _lstWord.LstKey.FindIndex(x => x.Equals(key));
            if (ind >= 0)
            {
                int offset = GetDemicalValue(_lstWord.LstOffset[ind]);
                int length = GetDemicalValue(_lstWord.LstLength[ind]);
                res = await GetMeaning(offset, length);
                //res = offset + length + "";
                res = _lstWord.LstOffset[ind] + " " + _lstWord.LstLength[ind] + " " + res;
            }

            if (res == null)
                res = "N/A";

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
            return await DataHelper.GetMeaning(offset, length);
        }

        public async Task<List<string>>  LoadTranslatedWords()
        {
            if (_lstTranslatedWords.Count != 0)
                return _lstTranslatedWords;

            _lstTranslatedWords = await DataHelperTranslatedWords.LoadListWords();
            return _lstTranslatedWords;
        }
    }
}
