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
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using System.Windows.Shapes;

namespace ValueThing
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        string searchText="";
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

        private Grid createlistview( JSearch jSearchs)
        {
            Grid item = new Grid();
            item.Name = jSearchs.URL;
            item.Height = 150;
            item.Tap += item_Tap;
            //new and set value
            TextBlock title = new TextBlock();
            TextBlock source = new TextBlock();
            TextBlock price = new TextBlock();
            Image image = new Image();
            title.Text = jSearchs.Title;
            source.Text = jSearchs.Source;
            price.Text ="$"+ jSearchs.Price;
            image.Source = new BitmapImage(new Uri(jSearchs.Img, UriKind.RelativeOrAbsolute));
            //set font
            source.FontSize = 15;
            title.FontSize = 30;
            price.FontSize = 50;
            price.Foreground = new SolidColorBrush(Colors.Red);
            source.Foreground = new SolidColorBrush(Colors.Orange);
            title.TextWrapping = TextWrapping.Wrap;//換行         
            //設定邊界長寬
            title.Margin = new Thickness(110, 0, 0, 0);
            source.Margin = new Thickness(110, 110, 0, 0);
            price.Margin = new Thickness(0, 0, 1, 0);
            image.Width = 100;
            image.Height = 100;
            title.Height = 40 * 2;
            title.Width = item.Width;
            price.Width = item.Width;
            //設定對齊
            image.HorizontalAlignment = HorizontalAlignment.Left;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            source.HorizontalAlignment = HorizontalAlignment.Left;
            price.HorizontalAlignment = HorizontalAlignment.Right;
            title.VerticalAlignment = VerticalAlignment.Top;
            image.VerticalAlignment = VerticalAlignment.Top;
           // source.VerticalAlignment = VerticalAlignment.Bottom;
            price.VerticalAlignment = VerticalAlignment.Bottom;
            image.VerticalAlignment = VerticalAlignment.Center;



            Line line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.StrokeThickness = 2;
            Canvas cas = new Canvas();
            cas.Children.Add(line);

            cas.VerticalAlignment = VerticalAlignment.Top;
            //最後add進去
            item.Children.Add(cas);
            item.Children.Add(image);
            item.Children.Add(title);
            item.Children.Add(source);
            item.Children.Add(price);
            
            return item;

        }

        void item_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           string Url=((Grid)sender).Name;
           WebBrowserTask task = new WebBrowserTask();
           task.URL = Url;
           task.Show();
            //throw new NotImplementedException();

        }

        private void Search_click(object sender, RoutedEventArgs e)
        {

            new Searcher(keyWord.Text, SearchData).Search();
            searchText = keyWord.Text;
        }

        private void SearchData(JSearch[] jSearchs)
        {
            Dispatcher.BeginInvoke(() => {
                contentStackPanel.Children.Clear();          

                if (jSearchs.Length == 0)
                    contentStackPanel.Children.Add(fileNotFound());
                for (int i = 0; i < jSearchs.Length; i++)
                {
                    contentStackPanel.Children.Add(createlistview(jSearchs[i]));
                    

                }
                    
              
                
                
            });
        }

        private TextBlock fileNotFound()
        {
            TextBlock notFound =new TextBlock();
            notFound.Text = "'"+searchText+"'can't find any result.";
            notFound.TextAlignment = TextAlignment.Center;
            notFound.Height = 100;
            
            return notFound;
        }

        private Grid createInfoItem(JSearch jSearchs)
        {
            Grid item = new Grid();

            TextBlock title = new TextBlock();
            TextBlock content = new TextBlock();
            TextBlock url = new TextBlock();
            Image img = new Image();
            title.VerticalAlignment = VerticalAlignment.Top;
            content.VerticalAlignment = VerticalAlignment.Center;
            url.VerticalAlignment = VerticalAlignment.Bottom;
            


            return item;

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