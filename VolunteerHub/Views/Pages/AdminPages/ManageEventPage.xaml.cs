using Microsoft.Win32;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для ManageEventPage.xaml
    /// </summary>
    public partial class ManageEventPage : Page
    {
        private dbVolunteerHubEntities _context;
        private Events _currentEvent;

        public ManageEventPage(Users user, Events selectedEvent = null)
        {
            InitializeComponent();
            _context = new dbVolunteerHubEntities();
            DeleteEvent.Visibility = selectedEvent != null ? Visibility.Visible : Visibility.Collapsed;
            LoadOrganizers();
            _currentEvent = selectedEvent ?? new Events();
            LoadEventDetails();
        }
        private void LoadOrganizers()
        {
            cbOrganizer.ItemsSource = _context.Users.ToList();
        }

        private void LoadEventDetails()
        {
            if (_currentEvent != null)
            {
                txbTitle.Text = _currentEvent.Name;
                txbDescription.Text = _currentEvent.Description;
                EventDate.SelectedDate = _currentEvent.EventDate;
                txbLocation.Text = _currentEvent.Location;
                cbOrganizer.SelectedItem = _context.Users.FirstOrDefault(u => u.UserID == _currentEvent.OrganizerID);

                if (_currentEvent.PicEvent != null)
                {
                    using (var stream = new System.IO.MemoryStream(_currentEvent.PicEvent))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = stream;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        EventPicture.Source = image;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedOrganizer = cbOrganizer.SelectedItem as Users;
                if (selectedOrganizer == null)
                {
                    MessageBox.Show("Пожалуйста, выберите организатора.");
                    return;
                }

                UpdateEventProperties(selectedOrganizer);

                if (_currentEvent.EventID == 0) 
                {
                    _context.Events.Add(_currentEvent);
                }
                else
                {
                    _context.Entry(_currentEvent).State = System.Data.Entity.EntityState.Modified;
                }

                _context.SaveChanges();

                MessageBox.Show("Мероприятие успешно сохранено.", "Уведомление системы.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении мероприятия: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateEventProperties(Users selectedOrganizer)
        {
            _currentEvent.Name = txbTitle.Text;
            _currentEvent.Description = txbDescription.Text;
            _currentEvent.EventDate = EventDate.SelectedDate;
            _currentEvent.Location = txbLocation.Text;
            _currentEvent.OrganizerID = selectedOrganizer.UserID;

            if (EventPicture.Source != null)
            {
                var bitmap = EventPicture.Source as BitmapImage;
                if (bitmap != null)
                {
                    _currentEvent.PicEvent = ConvertBitmapImageToByteArray(bitmap);
                }
            }
        }

        private byte[] ConvertBitmapImageToByteArray(BitmapImage bitmap)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        private void UploadPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                EventPicture.Source = bitmap;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private Visibility GetVisibility(Events events)
        {
            if (events != null)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        private void DeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить это мероприятие?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (_currentEvent != null && _currentEvent.EventID != 0)
                    {
                        var eventToDelete = _context.Events.Find(_currentEvent.EventID);
                        if (eventToDelete != null)
                        {
                            var eventRegistrationsToDelete = _context.EventRegistrations
                                .Where(er => er.EventID == _currentEvent.EventID)
                                .ToList();
                            _context.EventRegistrations.RemoveRange(eventRegistrationsToDelete);

                            var reviewsToDelete = _context.Reviews
                                .Where(r => r.EventID == _currentEvent.EventID)
                                .ToList();
                            _context.Reviews.RemoveRange(reviewsToDelete);

                            _context.Events.Remove(eventToDelete);
                            _context.SaveChanges();

                            MessageBox.Show("Мероприятие успешно удалено.", "Удаление завершено", MessageBoxButton.OK, MessageBoxImage.Information);
                            NavigationService.GoBack();
                        }
                        else
                        {
                            MessageBox.Show("Мероприятие не найдено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Мероприятие не найдено или не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при удалении мероприятия: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
