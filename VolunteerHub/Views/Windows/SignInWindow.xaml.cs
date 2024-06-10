using System;
using System.Linq;
using System.Windows;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    { 
        public SignInWindow()
        {
            InitializeComponent();
        }
        private void SaveUserCredentials(int userId)
        {
            Properties.Settings.Default.UserId = userId;
            Properties.Settings.Default.Save();
        }

        private void buttonSignIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new dbVolunteerHubEntities())
                {
                    var currentUser = db.Users.Include("Roles")
                        .FirstOrDefault(item => item.Username == textBoxUsername.Text && item.PasswordHash == passwordBoxPassword.Password);

                    if (currentUser == null)
                    {
                        MessageBox.Show("Извините, но введённый пароль или имя пользователя некорректные.", "Недействительный имя пользователя или пароль.",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        SaveUserCredentials(currentUser.UserID);

                        var isAdmin = currentUser.Roles != null && currentUser.Roles.RoleName == "Администратор";
                        if (isAdmin)
                        {
                            MessageBox.Show("Добро пожаловать, администратор!", "Административный доступ", MessageBoxButton.OK, MessageBoxImage.Information);

                            MainWindow mainWindow = new MainWindow(currentUser);
                            mainWindow.Show();

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Добро пожаловать!", "Пользовательский доступ", MessageBoxButton.OK, MessageBoxImage.Information);
                            MainWindow mainWindow = new MainWindow(currentUser);
                            mainWindow.Show();

                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла системная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();
            this.Close();
        }
    }
}
