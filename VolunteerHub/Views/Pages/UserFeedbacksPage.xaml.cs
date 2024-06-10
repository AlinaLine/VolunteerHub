using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserFeedbacksPage.xaml
    /// </summary>
    public partial class UserFeedbacksPage : Page
    {
        private Events _events {  get; set; }
        private Users _user { get; set; }

        public UserFeedbacksPage(Events events, Users user)
        {
            InitializeComponent();
            _events = events;
            _user = user;
        }

        private void btnLeaveFeedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txbFeedbacksText.Text) || string.IsNullOrEmpty(txbFinalGrade.Text))
                {
                    MessageBox.Show("Недопускаются пустые значения для отзыва, пожалуйста, заполните поле отзыва и поле оценки", "Внимание, недопустимое значение",
                       MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using(var db = new dbVolunteerHubEntities())
                {
                    Reviews reviews = new Reviews
                    {
                        UserID = _user.UserID,
                        EventID = _events.EventID,
                        Rating = int.Parse(txbFinalGrade.Text),
                        Comment = txbFeedbacksText.Text
                    };

                    db.Reviews.Add(reviews);
                    db.SaveChanges();
                    MessageBox.Show("Спасибо вам за отвзыв, ваш отзыв сохранён в системе.", "Ваш отзыв отправлен!",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла системная ошибка, подробности здесь: \n" + ex.Message, "Системная ошибка.",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadDataCurrentEvent()
        {
            txbCurrentEvent.Text = _events.Name;
            txbFirstname.Text = _user.Firstname;
            txbLastname.Text = _user.Lastname;
            txbDateNow.Text = DateTime.Now.ToString();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataCurrentEvent();
        }

        private void txbFinalGrade_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !IsValidFullInput(fullText);
        }

        private bool IsValidFullInput(string fullText)
        {
            return Regex.IsMatch(fullText, "^[0-5]$");
        }

    }
}
