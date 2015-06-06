using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using H2Dict.Model;
using View.Common;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace H2Dict
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ListWords _lstWords = new ListWords();
        public ListWords LstWords
        {
            get { return _lstWords; }
            set { _lstWords = value; }
        } 


        private readonly NavigationHelper navigationHelper;

        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        public List<string> Suggestions { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.NavigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.NavigationHelper.SaveState += this.NavigationHelper_SaveState;
            Suggestions = new List<string>();
            txtSearch.ItemsSource = Suggestions;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            LstWords = await DataHelper.LoadListWords();
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
            this.navigationHelper.OnNavigatedTo(e);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                Suggestions.Clear();
                Suggestions.Add(txtSearch.Text + "1");
                Suggestions.Add(txtSearch.Text + "2");
                Suggestions.Add(txtSearch.Text + "3");
                Suggestions.Add(txtSearch.Text + "4");
                txtSearch.ItemsSource = Suggestions;
                //string str = txtSearch.Text;
                //int len = str.Length;
                
                //foreach(string word in LstWords.LstKey)
                //{
                //    if(word.Length >= len && word.Substring(0,len).Equals(str))
                //    {
                //        int ind = LstWords.LstKey.FindIndex(x => x.Equals(word));
                //        for(int i = 0; i < 10; i++)
                //        {
                //            Suggestions.Clear();
                //            Suggestions.Add(LstWords.LstKey[ind + i]);
                //        }
                //    }
                //}
            }
        }

        private void txtSearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            txtSearch.Text = "Choosen";
        }
    }
}
