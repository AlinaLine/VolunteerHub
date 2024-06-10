using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages
{
    public partial class ReviewsPage : Page
    {
        public ObservableCollection<Reviews> Reviews { get; set; }
        private List<Reviews> allReviews; 
        private List<Reviews> paginatedReviews;

        private const int ItemsPerPage = 10;
        private int currentPage = 1;

        public ReviewsPage()
        {
            InitializeComponent();
            Reviews = new ObservableCollection<Reviews>();
            allReviews = new List<Reviews>();
            paginatedReviews = new List<Reviews>();
            LoadReviews();
            this.DataContext = this;
        }

        private async void LoadReviews()
        {
            using (var context = new dbVolunteerHubEntities())
            {
                var reviews = await context.Reviews.Include("Events").Include("Users").ToListAsync();

                foreach (var review in reviews)
                {
                    allReviews.Add(review);
                }
            }
            UpdateListView();
        }

        private void UpdateListView()
        {
            int startIndex = (currentPage - 1) * ItemsPerPage;
            int endIndex = Math.Min(startIndex + ItemsPerPage, allReviews.Count);

            paginatedReviews = allReviews.GetRange(startIndex, endIndex - startIndex);
            ReviewsListView.ItemsSource = paginatedReviews;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdateListView();
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage * ItemsPerPage < allReviews.Count)
            {
                currentPage++;
                UpdateListView();
            }
        }
    }
}
