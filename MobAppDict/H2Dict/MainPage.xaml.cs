﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
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
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace H2Dict
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
    //    private ListWords _lstWords = new ListWords();
    //    public ListWords LstWords
    //    {
    //        get { return _lstWords; }
    //        set { _lstWords = value; }
    //    }

        private Dict _dict;

        public Dict Dict
        {
          get { return _dict; }
          set { _dict = value; }
        }

        private readonly NavigationHelper navigationHelper;

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        public MainPage()
        {
            this.InitializeComponent();

            _dict = new Dict();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.NavigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.NavigationHelper.SaveState += this.NavigationHelper_SaveState;

        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            await Dict.LoadListWords();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            
            if(Dict.LstWord.LstKey.Count == 0)
                this.navigationHelper.OnNavigatedTo(e);

            if (App.ChangeDict)
                Dict.LoadListWords();

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //remove the handler before you leave!
            //this.navigationHelper.OnNavigatedFrom(e);
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Application.Current.Exit();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string str = txtSearch.Text;
            string res = await Dict.Search(str);

            txtDisplay.Text = res;
            // Lưu lược sử.
            if(res != "N/A")
                Dict.UpdateTranslatedWords(str);
        }

        private void txtSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = getSuggestionWord(sender.Text);
            }
        }

        private void txtSearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = txtSearch.Text;
        }

        // Suggestion word for AutoSuggestion
        private List<string> getSuggestionWord(string key)
        {
            return Dict.GetSuggestion(key);
        }

        private void AppBarButtonHistory_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (TranslatedWords));
        }

        private void AppBarButtonFavorite_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (FavoriteWords));
        }

        private void AppBarButtonSettings_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (Settings));
        }

        private async void ButtonAddFav_OnClick(object sender, RoutedEventArgs e)
        {
            
            
            if (txtDisplay.Text.Equals("N/A"))
            {
                string nofi = "Add favorite word fail!!!";
                MessageDialog md = new MessageDialog(nofi);
                await md.ShowAsync();
            }
            else
            {
                string nofi = "Add favorite word success!!!";
                MessageDialog md = new MessageDialog(nofi);
                await md.ShowAsync();

                Dict.UpdateFavoriteWords(txtSearch.Text);
            }
           

        }

        private void GridDisplay_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            // If you need the clicked element:
            // Item whichOne = senderElement.DataContext as Item;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }
    }
}
