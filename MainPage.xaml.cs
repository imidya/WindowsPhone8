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
            if (dataUpdateCount >= 2)
            {
                CompareResult.Visibility = System.Windows.Visibility.Visible;
                SetBusy(false);
            }
        }

        private void UpdateProduction2Rate(JRate jRate)
        {
            Dispatcher.BeginInvoke(() =>
            {
                Production2Rate.Text = jRate.Rate.ToString();
                if( jRate.Url != null )
                    Production2Image.Source = new BitmapImage(new Uri(jRate.Url, UriKind.Absolute));
                else
                    Production2Image.Source = new BitmapImage(new Uri(@"assets\DeleteRed.png", UriKind.Relative));
                UpdateDone();
            });
        }

        private void UpdateProduction1Rate(JRate jRate)
        {
            Dispatcher.BeginInvoke(() =>
            {
                Production1Rate.Text = jRate.Rate.ToString();
                if (jRate.Url != null)
                    Production1Image.Source = new BitmapImage(new Uri(jRate.Url, UriKind.Absolute));
                else
                    Production1Image.Source = new BitmapImage(new Uri(@"assets\DeleteRed.png", UriKind.Relative));
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
            title.FontWeight = FontWeights.Bold;
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
         
            price.VerticalAlignment = VerticalAlignment.Bottom;
            image.VerticalAlignment = VerticalAlignment.Center;

            item.Margin = new Thickness(0, 20, 0, 0);
            
            //最後add進去
            
            item.Children.Add(image);
            item.Children.Add(title);
            item.Children.Add(source);
            item.Children.Add(price);
            item.Children.Add(drawLine());
            
            return item;

        }

        void item_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           string Url=((Grid)sender).Name;
           WebBrowserTask task = new WebBrowserTask();
           task.URL = Url;
           task.Show();
        }

        private void Search_click(object sender, RoutedEventArgs e)
        {
            new Searcher(keyWord.Text, SearchData).Search();
            new Poster(keyWord.Text, PostData).Get();
            searchText = keyWord.Text;
        }

        private void PostData(JPosts jPosts)
        {
             Dispatcher.BeginInvoke(() => {
                 webStackPanel.Children.Clear();
                 blogStackPanel.Children.Clear();
                 if (jPosts.Webs.Length != 0)
                     web_noinfo.Visibility = Visibility.Collapsed;
                 if (jPosts.Blogs.Length != 0)
                     blog_noinfo.Visibility = Visibility.Collapsed;
                 for (int i = 0; i < jPosts.Webs.Length; i++)                 
                    webStackPanel.Children.Add(createInfoWebItem(jPosts.Webs[i]));
                 
                     
                 for (int i = 0; i < jPosts.Blogs.Length; i++)
                    blogStackPanel.Children.Add(createInfoBlogItem(jPosts.Blogs[i])); 

             });
        }

        private void SearchData(JSearch[] jSearchs)
        {
            Dispatcher.BeginInvoke(() => {
                contentStackPanel.Children.Clear();          

                if (jSearchs.Length == 0)
                    contentStackPanel.Children.Add(fileNotFound());
                for (int i = 0; i < jSearchs.Length; i++)      
                    contentStackPanel.Children.Add(createlistview(jSearchs[i]));

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

        private Grid createInfoWebItem(JWeb jweb)
        {
            Grid item = new Grid();
            StackPanel infoStack = new StackPanel();
            item.Name = jweb.Url;
            item.Tap+=item_Tap;
            TextBlock title = new TextBlock();
            TextBlock content = new TextBlock();
            TextBlock url = new TextBlock();
            Image img = new Image();
            title.Text = jweb.Title;
            content.Text = jweb.Content;
            url.Text = jweb.Url;


            Uri uriR = new Uri("/Assets/right_arrow.png", UriKind.Relative);
            BitmapImage imgSourceR = new BitmapImage(uriR);
            img.Source = imgSourceR;

            title.FontWeight = FontWeights.Bold;
            title.Foreground = new SolidColorBrush( Color.FromArgb(255,0,102,204));
            url.Foreground = new SolidColorBrush(Colors.Orange);
            title.FontSize = 40;
            url.FontSize = 15;
            title.TextWrapping = TextWrapping.Wrap;//換行 
            title.Height = 60 * 2;
            if (title.ActualWidth<=430)
                title.Height = 60 ;
            img.Width = 50;
            img.Height = 50;
            
            content.TextWrapping = TextWrapping.Wrap;//換行  
 
            url.HorizontalAlignment = HorizontalAlignment.Right;
            infoStack.Children.Add(title);
            infoStack.Children.Add(content);
            infoStack.Children.Add(url);
            item.Children.Add(infoStack);
            item.Children.Add(img);
            Canvas test = drawLine();
            item.Children.Add(test);

            infoStack.Margin = new Thickness(0, 0, 50, 0);
            img.HorizontalAlignment = HorizontalAlignment.Right;
            item.Margin = new Thickness(0, 20, 0, 0);
            return item;

        }

        private Grid createInfoBlogItem(JBlog jblog)
        {
            Grid item = new Grid();
            StackPanel infoStack = new StackPanel();
            item.Name = jblog.PostUrl;
            item.Tap += item_Tap;
            TextBlock title = new TextBlock();
            TextBlock content = new TextBlock();
            TextBlock url = new TextBlock();
            Image img = new Image();
            title.Text = jblog.Title;
            content.Text = jblog.Content;
            url.Text = jblog.PostUrl;


            Uri uriR = new Uri("/Assets/right_arrow.png", UriKind.Relative);
            BitmapImage imgSourceR = new BitmapImage(uriR);
            img.Source = imgSourceR;

            title.FontWeight = FontWeights.Bold;
            title.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 102, 204));
            url.Foreground = new SolidColorBrush(Colors.Orange);
            title.FontSize = 40;
            url.FontSize = 15;
            title.TextWrapping = TextWrapping.Wrap;//換行 
            title.Height = 60 * 2;
            if (title.ActualWidth <= 430)
                title.Height = 60;
            img.Width = 50;
            img.Height = 50;

            content.TextWrapping = TextWrapping.Wrap;//換行  

            url.HorizontalAlignment = HorizontalAlignment.Right;
            infoStack.Children.Add(title);
            infoStack.Children.Add(content);
            infoStack.Children.Add(url);
            item.Children.Add(infoStack);
            item.Children.Add(img);
            item.Children.Add(drawLine());

            infoStack.Margin = new Thickness(0, 0, 50, 0);
            img.HorizontalAlignment = HorizontalAlignment.Right;
            item.Margin = new Thickness(0, 20, 0, 0);
            return item;
            
        }

        private Canvas drawLine()
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(Colors.DarkGray);
            line.StrokeThickness = 2;
            Canvas cas = new Canvas();
            cas.Children.Add(line);
     

            Point point1 = new Point();
            point1.X = 0.0;
            point1.Y = 0.0;

            Point point2 = new Point();
            point2.X = 500.0;
            point2.Y = 0.0;

            line.X1 = point1.X;
            line.Y1 = point1.Y;
            line.X2 = point2.X;
            line.Y2 = point2.Y;

            cas.VerticalAlignment = VerticalAlignment.Bottom;
            return cas;
    
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