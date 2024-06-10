using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            LoadNews();
        }

        private void LoadNews()
        {
            using (var context = new dbVolunteerHubEntities())
            {
                var newsList = context.News.ToList();
                NewsListView.ItemsSource = newsList;
            }
        }

        private void NewsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NewsListView.SelectedItem is News selectedNews)
            {
                var newsDisplayPage = new NewsDisplayPage(selectedNews.NewsID);
                NavigationService.Navigate(newsDisplayPage);
            }
        }
    }
}
