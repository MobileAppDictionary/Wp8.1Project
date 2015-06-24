﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace H2Dict.Helper
{
    public class DataHelperFavoriteWords
    {
        private const string FileName = "FavoriteWords.txt";
        private string _typeDict = App.TypeDictIns.GetTypeDict();
        private static DataHelperFavoriteWords _dataHelper = new DataHelperFavoriteWords();
        private List<string> _lstWords = new List<string>();

        public async static Task<List<string>> LoadListWords()
        {
            return await _dataHelper.LoadListWordsAsync();
        }

        private async Task<List<string>> LoadListWordsAsync()
        {
            if (_lstWords.Count != 0)
                return _lstWords;

            string result = null;
//            string path = @"ms-appx:///Data/" + typeDict + FileName;
//            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
            StorageFolder local = ApplicationData.Current.LocalFolder;
            StorageFolder fold;
            if (await FolderExists(local, _typeDict))
            {
                fold = await local.GetFolderAsync(_typeDict);
            }
            else
            {
                fold = await local.CreateFolderAsync(_typeDict);
            }

            if (!await FileExists(fold, FileName))
                fold.CreateFileAsync(FileName);

            using (StreamReader sRead = new StreamReader(await fold.OpenStreamForReadAsync(FileName)))
                result = await sRead.ReadToEndAsync();

            string[] lines = result.Split(new char[2] { '\r', '\n' });

            foreach (string line in lines)
            {
                if (line != "")
                    _lstWords.Add(line);
            }

            return _lstWords;
        }

        private async Task<bool> FolderExists(StorageFolder folder, string name)
        {
            try
            {
                StorageFolder file = await folder.GetFolderAsync(name);
            }
            catch { return false; }
            return true;
        }

        private async Task<bool> FileExists(StorageFolder folder, string name)
        {
            try
            {
                StorageFile file = await folder.GetFileAsync(name);
            }
            catch { return false; }
            return true;
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
            //string path = @"ms-appx:///Data/" + TypeDict + FileName;
            StorageFolder local = ApplicationData.Current.LocalFolder;
            StorageFolder fold;
            if (await FolderExists(local, _typeDict))
            {
                fold = await local.GetFolderAsync(_typeDict);
            }
            else
            {
                fold = await local.CreateFolderAsync(_typeDict);
            }

            if (!await FileExists(fold, FileName))
                fold.CreateFileAsync(FileName);
            //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
            StorageFile file = await fold.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
//            IRandomAccessStreamReference thumbail = RandomAccessStreamReference.CreateFromUri(new Uri(path));
//            StorageFile fileTemp =
//                await StorageFile.ReplaceWithStreamedFileFromUriAsync(file, new Uri(@"ms-appx:///Data/" + TypeDict), thumbail);
            //StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(TypeDict + FileName, CreationCollisionOption.ReplaceExisting);

            using (StreamWriter sWrite = new StreamWriter(await file.OpenStreamForWriteAsync()))
            {
                sWrite.Flush();
                await sWrite.WriteAsync(value);
                await sWrite.FlushAsync();
                sWrite.Dispose();
            }
            //_lstWords.Clear();
            //_lstWords = await LoadListWords();
        }
    }
}
