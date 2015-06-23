using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace H2Dict.Helper
{
    public class DataHelperTranslatedWords
    {
        private const string FileName = "TranslatedWords.txt";
        private const string TypeDict = "EnVi/";
        private static DataHelperTranslatedWords _dataHelper = new DataHelperTranslatedWords();
        private List<string> _lstWords = new List<string>();

        public async static Task<List<string>> LoadListWords()
        {
            return await _dataHelper.LoadListWords("EnVi/");
        }

        private async Task<List<string>> LoadListWords(string typeDict)
        {
            string result = null;
            string path = @"ms-appx:///Data/" + typeDict + FileName;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));

            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                result = await sRead.ReadToEndAsync();

            string[] lines = result.Split(new char[1] { '\n' });

            foreach (string line in lines)
            {
                _lstWords.Add(line);
            }

            return _lstWords;
        }
    }
}
