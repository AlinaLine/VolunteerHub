using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для CurrentUserEventsPage.xaml
    /// </summary>
    public partial class CurrentUserEventsPage : Page
    {
        private Users _user { get; set; }
        private const int ItemsPerPage = 10; 
        private int currentPage = 1;
        private List<Events> allEvents; 
        private List<Events> paginatedEvents; 

        public CurrentUserEventsPage(Users user)
        {
            InitializeComponent();
            _user = user;
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
            if (currentPage * ItemsPerPage < allEvents.Count)
            {
                currentPage++;
                UpdateListView();
            }
        }
        private void UpdateListView()
        {
            int startIndex = (currentPage - 1) * ItemsPerPage;
            int endIndex = startIndex + ItemsPerPage;
            endIndex = endIndex > allEvents.Count ? allEvents.Count : endIndex;

            paginatedEvents = allEvents.GetRange(startIndex, endIndex - startIndex);
            EventsListView.ItemsSource = paginatedEvents;
        }
        private void btnFeedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                var eventItem = button.CommandParameter as Events;
                if (eventItem != null)
                {
                    var feedbackPage = new UserFeedbacksPage(eventItem, _user);
                    this.NavigationService.Navigate(feedbackPage);
                }
                else
                {
                    Console.WriteLine("eventItem is null");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла системная ошибка, подробности здесь: \n" + ex.Message, "Системная ошибка.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        public List<Events> GetEventsForUser(int userId)
        {
            using (var context = new dbVolunteerHubEntities())
            {
                var userEvents = context.EventRegistrations
                                        .Where(reg => reg.UserID == userId)
                                        .Select(reg => reg.Events)
                                        .ToList();

                return userEvents;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            EventsListView.ItemsSource = GetEventsForUser(_user.UserID);
        }
    }
}
