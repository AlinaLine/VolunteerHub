using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using VolunteerHub.Model;
using VolunteerHub.ViewModel;

namespace VolunteerHub.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для EventDetailsPage.xaml
    /// </summary>
    public partial class EventDetailsPage : Page
    {
        public Events Event { get; set; }
        public Users User { get; set; }
        public EventDetailsPage(Users users, Events events)
        {
            InitializeComponent();
            Event = events;
            User = users;
            DataContext = Event;
            UpdateRegistrationStatus();
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        public void RegisterUserForEvent(int userId, int eventId, DateTime timePreference)
        {
            try
            {
                using (var db = new dbVolunteerHubEntities())
                {
                    var registration = new EventRegistrations
                    {
                        EventID = eventId,
                        UserID = userId,
                        TimePreference = timePreference,
                        Status = "Registered"
                    };

                    db.EventRegistrations.Add(registration);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при регистрации на мероприятие: " + ex.Message);
                MessageBox.Show("Ошибка при регистрации на мероприятие: " + ex.Message, "Системная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        private async void buttonFolowEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new dbVolunteerHubEntities())
                {
                    int currentUserId = User.UserID;
                    int currentEventId = Event.EventID;
                    DateTime preferredTime = DateTime.Now;

                    RegisterUserForEvent(currentUserId, currentEventId, preferredTime);
                    MessageBox.Show("Вы успешно зарегистрированы на мероприятие!", "Операция прошла успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateRegistrationStatus();

                    try
                    {
                        await NotifyViewModel.NotifyEventRegistration(User.Email, Event);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при отправке уведомления: {ex.Message}");
                        MessageBox.Show("Произошла ошибка при отправке уведомления по электронной почте.", "Ошибка уведомления", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при регистрации на мероприятие: {ex.Message}");
                MessageBox.Show(ex.Message, "Произошла системная ошибка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool IsUserRegistered(int userId, int eventId)
        {

            using (var db = new dbVolunteerHubEntities())
            {
                return db.EventRegistrations.Any(reg => reg.UserID == userId && reg.EventID == eventId);
            }

        }

        private void UpdateRegistrationStatus()
        {
            int currentUserId = User.UserID;  
            int currentEventId = Event.EventID;

            if (IsUserRegistered(currentUserId, currentEventId))
            {
                RegistrationStatusTextBlock.Visibility = Visibility.Visible;
                RegistrationStatusTextBlock.Foreground = new SolidColorBrush(Colors.Green);
                buttonFolowEvent.Visibility = Visibility.Collapsed;
                UnregisterButton.Visibility = Visibility.Visible;
            }
            else
            {
                RegistrationStatusTextBlock.Visibility = Visibility.Collapsed;
                buttonFolowEvent.Visibility = Visibility.Visible;
                UnregisterButton.Visibility = Visibility.Collapsed;
            }
        }
        private async void UnregisterButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new dbVolunteerHubEntities())
            {
                var registration = db.EventRegistrations.FirstOrDefault(reg => reg.UserID == User.UserID && reg.EventID == Event.EventID);
                if (registration != null)
                {
                    db.EventRegistrations.Remove(registration);
                    db.SaveChanges();
                    MessageBox.Show("Вы успешно отписались от мероприятия.", "Операция прошла успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateRegistrationStatus();
                    await NotifyViewModel.NotifyEventUnregistration(User.Email, Event);
                }
            }
        }
    }
}
