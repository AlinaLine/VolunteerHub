using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для UserListPage.xaml
    /// </summary>
    public partial class UserListPage : Page
    {
        public UserListPage()
        {
            InitializeComponent();

            LoadUsers();
        }

        private void LoadUsers()
        {
            using (var context = new dbVolunteerHubEntities())
            {
                var usersList = context.Users.ToList();
                UsersDataGrid.ItemsSource = usersList;
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UsersDataGrid.SelectedItem is Users selectedUser)
                {
                    MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя {selectedUser.Username}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (var context = new dbVolunteerHubEntities())
                        {
                            var userToDelete = context.Users.FirstOrDefault(u => u.UserID == selectedUser.UserID);
                            if (userToDelete != null)
                            {
                                context.Users.Remove(userToDelete);
                                context.SaveChanges();
                            }
                        }
                        LoadUsers();
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите пользователя для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении пользователя. Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UsersDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = (Users)UsersDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                this.NavigationService.Navigate(new ManageUserPage(selectedItem));
            }
        }
    }
}
