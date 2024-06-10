using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VolunteerHub.Model;
using VolunteerHub.Views.Windows;
using VolunteerHub.Properties;

namespace VolunteerHub
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (Settings.Default.UserId == -1)
            {
            }
            else
            {
                using (var db = new dbVolunteerHubEntities())
                {
                    var currentUser = db.Users.Include("Roles")
                        .FirstOrDefault(item => item.UserID == Settings.Default.UserId);

                    if (currentUser != null)
                    {
                        MainWindow userWindow = new MainWindow(currentUser);
                        userWindow.Show();
                        return;
                    }
                }
            }


            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

    }
}
