using System;
using System.Data.Entity.Infrastructure;
using System.Windows;
using System.Windows.Controls;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProposeEventPage.xaml
    /// </summary>
    public partial class ProposeEventPage : Page
    {
        private Users _user;
        public ProposeEventPage(Users user)
        {
            InitializeComponent();
            _user = user;
            LoadUserData();
        }
        private void LoadUserData()
        {
            NameTextBox.Text = _user.Firstname;
            LastnameTextBox.Text = _user.Lastname;
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ThemeTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                    !DateEvent.SelectedDate.HasValue ||
                    string.IsNullOrWhiteSpace(AdressTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newEventApplication = new EventApplications
                {
                    UserID = _user.UserID,
                    TitleEvent = ThemeTextBox.Text,
                    EventProposal = DescriptionTextBox.Text,
                    ApplicationDate = DateEvent.SelectedDate.Value,
                    AddressEvent = AdressTextBox.Text,
                    Status = "Proposed"
                };

                using (var context = new dbVolunteerHubEntities())
                {
                    context.EventApplications.Add(newEventApplication);
                    await context.SaveChangesAsync();
                }

                NameTextBox.Clear();
                LastnameTextBox.Clear();
                ThemeTextBox.Clear();
                DescriptionTextBox.Clear();
                DateEvent.SelectedDate = null;
                AdressTextBox.Clear();

                MessageBox.Show("Мероприятие успешно предложено.", "Системное уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show("Произошла ошибка при сохранении данных. Пожалуйста, попробуйте снова.", "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла неожиданная ошибка. Пожалуйста, попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
