﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace H2Dict.Helper
{
    public class TypeDict
    {
        private static TypeDict _typeDict = new TypeDict();
        private int _ind = 0;

        public List<string> TypeDictList = new List<string>();

        private TypeDict()
        {
            LoadTypeDict();
        }

        private async void  LoadTypeDict()
        {
            string root =
                Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            string path = root + @"\Data";
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
            IReadOnlyList<StorageFolder> lstFolder = await folder.GetFoldersAsync();
            foreach (var nameFolder in lstFolder)
            {
                TypeDictList.Add(nameFolder.DisplayName);
            }
        }

        public void SetTypeDict(string nameDict)
        {
            _typeDict._ind = _typeDict.TypeDictList.FindIndex(x => x.Equals(nameDict));
            if (_typeDict._ind == -1)
                _typeDict._ind = 0;
        }

        public string GetTypeDict()
        {
            return _typeDict.TypeDictList[_typeDict._ind];
        }

        public void Refresh()
        {
            _typeDict.LoadTypeDict();
        }

        public static TypeDict GetInstance()
        {
            if (_typeDict == null)
                _typeDict = new TypeDict();
            return _typeDict;
        }
    }
}
