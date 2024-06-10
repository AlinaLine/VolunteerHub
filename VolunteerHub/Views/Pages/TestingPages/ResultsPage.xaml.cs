using System.Windows;
using System.Windows.Controls;

namespace VolunteerHub.Views.Pages.TestingPages
{
    public partial class ResultsPage : Page
    {
        public ResultsPage(string resultType, string resultDescription)
        {
            InitializeComponent();
            DisplayVolunteerTypeResult(resultType, resultDescription);
        }

        private void DisplayVolunteerTypeResult(string resultType, string resultDescription)
        {
            var resultTypeTextBlock = new TextBlock
            {
                Text = resultType,
                FontSize = 36,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 20, 0, 10)
            };

            var resultDescriptionTextBlock = new TextBlock
            {
                Text = resultDescription,
                FontSize = 24,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 10, 0, 20)
            };

            ResultsStackPanel.Children.Add(resultTypeTextBlock);
            ResultsStackPanel.Children.Add(resultDescriptionTextBlock);
        }

        private void TryAgain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartOfTesting());
        }
    }
}
