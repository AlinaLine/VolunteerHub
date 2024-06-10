using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VolunteerHub.Model;
using VolunteerHub.Views.Pages.AdminPages;

namespace VolunteerHub.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для EventsCalendarPage.xaml
    /// </summary>
    public partial class EventsCalendarPage : Page
    {
        public Users User { get; set; }
        private dbVolunteerHubEntities _context;
        private const int ItemsPerPage = 10; 
        private int currentPage = 1;
        private List<Events> allEvents; 
        private List<Events> paginatedEvents; 

        public EventsCalendarPage(Users user = null)
        {
            InitializeComponent();
            LoadTestData();
            User = user;
        }
        private void LoadTestData()
        {
            using (var db = new dbVolunteerHubEntities())
            {
                allEvents = db.Events.ToList(); 

                foreach (var ev in allEvents)
                {
                    if (ev.PicEvent != null && ev.PicEvent.Length > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Event ID: {ev.EventID}, Name: {ev.Name}, Picture Size: {ev.PicEvent.Length} bytes");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Event ID: {ev.EventID}, Name: {ev.Name}, No Picture");
                    }
                }

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

        private void EventsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (User == null)
            {
                MessageBox.Show("Извините, но чтобы открыть подробности мероприятий, вам необходимо зарегистрироваться как пользователь.", "Вы не зарегистрированы.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Events selectionEvent = EventsListView.SelectedItem as Events;
            if (selectionEvent != null && User != null)
            {
                if (User.Roles != null && User.Roles.RoleName == "Администратор")
                {
                    ManageEventPage manageEventPage = new ManageEventPage(User, selectionEvent);
                    manageEventPage.DataContext = selectionEvent;
                    this.NavigationService.Navigate(manageEventPage);
                }
                else
                {
                    EventDetailsPage eventDetailsPage = new EventDetailsPage(User, selectionEvent);
                    eventDetailsPage.DataContext = selectionEvent;
                    this.NavigationService.Navigate(eventDetailsPage);
                }
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTestData();
        }
    }
}
