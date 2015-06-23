using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace H2Dict.Helper
{
    public class DataHelperFavoriteWords
    {
        private const string FileName = "FavoriteWords.txt";
        private const string TypeDict = "EnVi/";
        private static DataHelperFavoriteWords _dataHelper = new DataHelperFavoriteWords();
        private List<string> _lstWords = new List<string>();

        public async static Task<List<string>> LoadListWords()
        {
            return await _dataHelper.LoadListWords("EnVi/");
        }

        private async Task<List<string>> LoadListWords(string typeDict)
        {
            if (_lstWords.Count != 0)
                return _lstWords;

            string result = null;
            string path = @"ms-appx:///Data/" + typeDict + FileName;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));

            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                result = await sRead.ReadToEndAsync();

            string[] lines = result.Split(new char[2] { '\r', '\n' });

            foreach (string line in lines)
            {
                if (line != "")
                    _lstWords.Add(line);
            }

            return _lstWords;
        }

        public static async Task SaveListWords(List<string> lstWords)
        {
            string value = "";

            foreach (var words in lstWords)
            {
                value = value + words + "\r\n";
            }
            await _dataHelper.SaveListWords(value);

        }

        private async Task SaveListWords(string value)
        {
            string path = @"ms-appx:///Data/" + TypeDict + FileName;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));

            using (StreamWriter sWrite = new StreamWriter(await file.OpenStreamForWriteAsync()))
            {
                await sWrite.WriteAsync(value);
                _lstWords.Clear();
            }

        }
    }
}
