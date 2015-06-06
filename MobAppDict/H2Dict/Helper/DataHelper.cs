﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

using H2Dict.Model;

namespace H2Dict.Helper
{
    public class DataHelper
    {
        private static DataHelper _dataHelper = new DataHelper();
        private ListWords _lstWords = new ListWords();
        public static async Task<ListWords> LoadListWords()
        {
            return await _dataHelper.LoadListWords("");
        }

        private async Task<ListWords> LoadListWords(string filename)
        {
            if (_lstWords.LstKey.Count != 0)
                return _lstWords;

            var fold = Windows.Storage.ApplicationData.Current.LocalFolder;
            
            string result = null;

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Data/EnVi/anhviet109K.index.txt"));
            
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                result = await sRead.ReadToEndAsync();
            
            string[] lines = result.Split(new char[1] { '\n' });

            foreach(string line in lines)
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
