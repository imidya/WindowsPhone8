using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ValueThing.Resources;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;

namespace ValueThing
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private int dataUpdateCount;

        private void BattleClick(object sender, RoutedEventArgs e)
        {
            dataUpdateCount = 0;
            SetBusy();
            CompareResult.Visibility = System.Windows.Visibility.Collapsed;

            new Rater(Production1.Text, UpdateProduction1Rate).Rate();
            new Rater(Production2.Text, UpdateProduction2Rate).Rate();
            new Searcher(Production1.Text, UpdateProduction1Image).Search();
            new Searcher(Production2.Text, UpdateProduction2Image).Search();
        }

        private void SetBusy(bool busy = true)
        {
            if (busy)
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.IsIndeterminate = true;
                SystemTray.ProgressIndicator.IsVisible = true;
            }
            else 
            {
                SystemTray.ProgressIndicator.IsVisible = false;
            }
        }

        private void UpdateDone()
        {
            dataUpdateCount++;
            if (dataUpdateCount >= 4)
            {
                CompareResult.Visibility = System.Windows.Visibility.Visible;
                SetBusy(false);
            }
        }

        private void UpdateProduction2Image(JSearch[] jSearchs)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if( jSearchs.Length > 0 )
                    Production2Image.Source = new BitmapImage(new Uri(jSearchs[0].Img, UriKind.Absolute));
                else
                    Production2Image.Source = new BitmapImage(new Uri(@"assets\DeleteRed.png", UriKind.Relative));
                UpdateDone();
            });
        }

        private void UpdateProduction1Image(JSearch[] jSearchs)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (jSearchs.Length > 0)
                    Production1Image.Source = new BitmapImage(new Uri(jSearchs[0].Img, UriKind.Absolute));
                else
                    Production1Image.Source = new BitmapImage(new Uri(@"assets\DeleteRed.png", UriKind.Relative));
                UpdateDone();
            });
        }

        private void UpdateProduction2Rate(JRate jRate)
        {
            Dispatcher.BeginInvoke(() =>
            {
                Production2Rate.Text = jRate.Rate.ToString();
                UpdateDone();
            });
        }

        private void UpdateProduction1Rate(JRate jRate)
        {
            Dispatcher.BeginInvoke(() =>
            {
                Production1Rate.Text = jRate.Rate.ToString();
                UpdateDone();
            });
        }

        private void Update(JSearch[] jSearchs)
        {
            Dispatcher.BeginInvoke(() =>
            {
                String showMessage ="";
                for (int i = 0; i < jSearchs.Length; i++)
                {
                    showMessage += i + "\n";
                    showMessage += "url: " + jSearchs[i].URL + "\n";
                    showMessage += "source: " + jSearchs[i].Source + "\n";
                    showMessage += "price: " + jSearchs[i].Price + "\n";
                    showMessage += "img: " + jSearchs[i].Img + "\n";
                    showMessage += "title: " + jSearchs[i].Title + "\n";
                }
                MessageBox.Show(showMessage);
            });
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}