﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using H2Dict.Helper;
using H2Dict.ViewModel;
using View.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace H2Dict
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TranslatedWords : Page
    {
        private Dict _dict = new Dict();

        public Dict Dict
        {
            get { return _dict; }
            set { _dict = value; }
        }

        private readonly NavigationHelper _navigationHelper;

        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public class Words
        {
            public string name { get; set; }
        }
        public static ObservableCollection<Words> LstTempTranslatedWords = new ObservableCollection<Words>();

        private static List<string> _lstAllTranslatedWords = new List<string>();

        public TranslatedWords()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this._navigationHelper = new NavigationHelper(this);
            this.NavigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.NavigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private bool incall;
        private bool endoflist;
        private int offset = 0;
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            _lstAllTranslatedWords = await Dict.LoadTranslatedWords();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this._navigationHelper.OnNavigatedTo(e);
        }

        // method to pull out a ScrollViewer
        public static ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer) return depObj as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }

        private void TranslatedWordsList_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer viewer = GetScrollViewer(this.TranslatedWordsList);
            viewer.ViewChanged += MainPage_ViewChanged;

        }

        private void MainPage_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer view = (ScrollViewer)sender;
            double progress = view.VerticalOffset / view.ScrollableHeight;
            System.Diagnostics.Debug.WriteLine(progress);
            if (progress > 0.7 & !incall && !endoflist)
            {
                incall = true;
                progressring.IsActive = true;
                FetchCountries(++offset);
            }

        }

        private void FetchCountries(int offset)
        {
            
            int start = offset * 20;
            for (int i = start; i < start + 20; i++)
            {
                if (i < _lstAllTranslatedWords.Count)
                    LstTempTranslatedWords.Add(new Words { name = _lstAllTranslatedWords[i] });
                else
                {
                    endoflist = true;
                    break;
                }
            }
            progressring.IsActive = false;
            incall = false;
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            TranslatedWordsList.ItemsSource = LstTempTranslatedWords;
            FetchCountries(0);

        }
    }
}
