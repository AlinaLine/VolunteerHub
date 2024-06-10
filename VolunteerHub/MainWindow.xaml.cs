using System.Windows;
using VolunteerHub.Views.Pages;
using VolunteerHub.Views.Windows;
using VolunteerHub.Model;
using System.Windows.Navigation;
using VolunteerHub.Views.Pages.AdminPages;
using VolunteerHub.Views.Pages.TestingPages;
using VolunteerHub.Properties;

namespace VolunteerHub
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Users User { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void SignOut()
        {
            Settings.Default.UserId = -1;
            Settings.Default.Save();

            User = new Users();
            ProfilePanel.Visibility = Visibility.Collapsed;
            UserControlPanel.Visibility = Visibility.Collapsed;
            ActionPanel.Visibility = Visibility.Visible;
            VolunteerControlPanel.Visibility = Visibility.Collapsed;
            AdminControlPanel.Visibility = Visibility.Collapsed;

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public MainWindow(Users user) : this()
        {
            User = user ?? new Users();

            if (user != null)
            {

                ProfilePanel.Visibility = Visibility.Visible;
                UserControlPanel.Visibility = Visibility.Visible;
                ActionPanel.Visibility = Visibility.Collapsed;

                if (user.RoleID == 1)
                {
                    VolunteerControlPanel.Visibility = Visibility.Collapsed;
                    AdminControlPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPage());
        }
        private void buttonSignIn_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow signInWindow = new SignInWindow();
            signInWindow.Show();
            this.Close();
        }

        private void buttonRegistration_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();
            this.Close();
        }

        private void buttonMainPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPage());
        }

        private void buttonEventsCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (User != null)
            {
                MainFrame.Navigate(new EventsCalendarPage(User));
            }
            else
            {
                MainFrame.Navigate(new EventsCalendarPage());
            }
        }

        private void buttonProfile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProfileUserPage(User));
        }

        private void CurrentUserEvent_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CurrentUserEventsPage(User));
        }

        private void ReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReviewsPage());
        }

        private void ProposalButton_Click(object sender, RoutedEventArgs e)
        {
            if (User != null)
            {
                MainFrame.Navigate(new ProposeEventPage(User));
            }
            else
            {
                MessageBox.Show("Чтобы предложить мероприятие, вам необходимо войти в аккаунт. Если у вас нет аккаунта, то вы можете зарегистрироватсья",
                    "Внимание, вы не вошли в систему.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void buttonManageEvent_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EventsCalendarPage(User));
        }

        private void buttonManageUser_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserListPage());
        }

        private void buttonManageNews_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new NewsPage());
        }

        private void Testing_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StartOfTesting(User));
        }

        private void ShowResult_Click(object sender, RoutedEventArgs e)
        {
            if (User != null)
                MainFrame.Navigate(new ShowResultsPage(User.UserID));
            else
            {
                MessageBox.Show("Чтобы просмотреть результаты теста, вам необходимо войти в аккаунт. Если у вас нет аккаунта, то вы можете зарегистрироватсья",
                   "Внимание, вы не вошли в систему.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        private void buttonAddEvent_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ManageEventPage(User));
        }

        private void buttonSignOut_Click(object sender, RoutedEventArgs e)
        {

            SignOut();
        }
    }
}
