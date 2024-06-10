using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace VolunteerHub.Views.Pages.TestingPages
{
    /// <summary>
    /// Логика взаимодействия для StartOfTesting.xaml
    /// </summary>
    public partial class StartOfTesting : Page
    {
        private Users _user;
        public StartOfTesting(Users user = null)
        {
            InitializeComponent();
            _user = user;
        }

        private void StartTesting_Click(object sender, RoutedEventArgs e)
        {
            if (_user != null)
            {
                NavigationService.Navigate(new QuestionPage(_user.UserID));
            }
            else
            {
                NavigationService.Navigate(new QuestionPage());

            }
        }
    }
}
