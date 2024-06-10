using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using VolunteerHub.Model;
using VolunteerHub.ViewModel;

namespace VolunteerHub.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstname = txbFirstname.Text;
                string lastname = txbLastname.Text;
                string username = txbUsername.Text;
                string password = psbPasswordBox.Password;
                string email = txbEmail.Text;
                int? roleId = 2;

                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(firstname) ||
                    string.IsNullOrWhiteSpace(lastname) ||
                    !roleId.HasValue)
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!EmailValidator.IsValidEmail(email))
                {
                    MessageBox.Show("Введите корректный адрес электронной почты.", "Недействительный адрес электронной почты", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var db = new dbVolunteerHubEntities())
                {
                    if (db.Users.Any(u => u.Username == username))
                    {
                        MessageBox.Show("Это имя пользователя уже занято. Пожалуйста, выберите другое.", "Неуникальное имя пользователя", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (!EmailValidator.CheckUniqueEmail(email))
                    {
                        MessageBox.Show("Этот адрес электронной почты уже зарегистрирован.", "Неуникальный адрес электронной почты", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Users user = new Users
                    {
                        Firstname = firstname,
                        Lastname = lastname,
                        Username = username,
                        PasswordHash = password,
                        Email = email,
                        Skills = txbSkills.Text,
                        Preferences = txbPreferences.Text,
                        ContactInfo = txbContactInfo.Text,
                        RoleID = roleId.Value
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                    MessageBox.Show("Регистрация успешна!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла системная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SignInWindow signInWindow = new SignInWindow();
            signInWindow.Show();
            this.Close();
        }

        private void txbUsername_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, @"\p{IsCyrillic}");
        }
    }
}
