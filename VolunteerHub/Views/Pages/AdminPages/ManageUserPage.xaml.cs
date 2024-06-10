using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ManageUserPage.xaml
    /// </summary>
    public partial class ManageUserPage : Page
    {
        private Users _user;
        public ManageUserPage(Users user)
        {
            InitializeComponent();
            _user = user;
            LoadUserData();
        }

        private void LoadUserData()
        {
            txbFirstname.Text = _user.Firstname;
            txbLastname.Text = _user.Lastname;
            LoadCombobox(cmbGender, () => cmbGender.SelectedValue = _user.GenderID);
            txbUserSkills.Text = _user.Skills;
            txbPreferences.Text = _user.Preferences;
            txbEmail.Text = _user.Email;
            txbPhone.Text = _user.ContactInfo;
            txbUsername.Text = _user.Username;
            txbPassword.Text = _user.PasswordHash;

        }

        private void LoadCombobox(ComboBox comboBox, Action afterLoad)
        {
            using (var db = new dbVolunteerHubEntities())
            {
                var genders = db.Genders.ToList();
                comboBox.ItemsSource = genders;
                comboBox.DisplayMemberPath = "GenderName";
                comboBox.SelectedValuePath = "GenderID";
                afterLoad?.Invoke();
            }
        }
        private void ToggleControlEnabled()
        {
            txbFirstname.IsEnabled = !txbFirstname.IsEnabled;
            txbLastname.IsEnabled = !txbLastname.IsEnabled;
            txbUserSkills.IsEnabled = !txbUserSkills.IsEnabled;
            txbPreferences.IsEnabled = !txbPreferences.IsEnabled;
            txbUsername.IsEnabled = !txbUsername.IsEnabled;
            txbPassword.IsEnabled = !txbPassword.IsEnabled;
            txbEmail.IsEnabled = !txbEmail.IsEnabled;
            txbPhone.IsEnabled = !txbPhone.IsEnabled;
            cmbGender.IsEnabled = !cmbGender.IsEnabled;
            btnSave.IsEnabled = !btnSave.IsEnabled;
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            ToggleControlEnabled();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new dbVolunteerHubEntities())
                {
                    db.Users.Attach(_user);
                    db.Entry(_user).State = System.Data.Entity.EntityState.Modified;

                    _user.Firstname = txbFirstname.Text;
                    _user.Lastname = txbLastname.Text;
                    _user.Username = txbUsername.Text;
                    _user.PasswordHash = txbPassword.Text;
                    _user.Email = txbEmail.Text;
                    _user.Skills = txbUserSkills.Text;
                    _user.Preferences = txbPreferences.Text;
                    _user.ContactInfo = txbPhone.Text;

                    if (cmbGender.SelectedItem != null)
                    {
                        _user.GenderID = ((Genders)cmbGender.SelectedItem).GenderID;
                    }

                    db.SaveChanges();

                    LoadUserData();

                    MessageBox.Show("Данные успешно сохранены.", "Операция завершена", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении данных. Пожалуйста, проверьте введённые данные и попробуйте снова. Текст ошибки: " + ex.Message,
                    "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txbPhone_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;
            string text = textBox.Text + e.Text;

            if (text.Length == 1)
            {
                textBox.Text = "+7 (";
                textBox.SelectionStart = textBox.Text.Length;
            }
            else if (text.Length == 8)
            {
                textBox.Text += ") ";
                textBox.SelectionStart = textBox.Text.Length;
            }
            else if (text.Length == 13 || text.Length == 16)
            {
                textBox.Text += "-";
                textBox.SelectionStart = textBox.Text.Length;
            }

            if (textBox.Text.Length >= 18)
            {
                e.Handled = true;
            }
        }
    }
}
