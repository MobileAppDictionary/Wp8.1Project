using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H2Dict.Helper
{
    public class DataHelperTranslatedWords
    {
        private const string fileName = "TranslatedWords.txt";

        private static DataHelper _dataHelper = new DataHelper();
        private ListWords _lstWords = new ListWords();

        private async Task<ListWords> LoadListWords(string filename)
        {
            if (_lstWords.LstKey.Count != 0)
                return _lstWords;

            var fold = Windows.Storage.ApplicationData.Current.LocalFolder;

            string result = null;
            string path = @"ms-appx:///Data/" + typeDict + fileInd;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));

            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                result = await sRead.ReadToEndAsync();

            string[] lines = result.Split(new char[1] { '\n' });

            foreach (string line in lines)
            {
                string[] strs = line.Split(new char[1] { '\t' });

                _lstWords.LstKey.Add(strs[0]);
                _lstWords.LstOffset.Add(strs[1]);
                _lstWords.LstLength.Add(strs[2]);
            }

            return _lstWords;
        }
    }
}
