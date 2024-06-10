using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages.TestingPages
{
    public partial class ShowResultsPage : Page
    {
        private dbVolunteerHubEntities _context;
        private int _userId;

        public ShowResultsPage(int userId)
        {
            InitializeComponent();
            _context = new dbVolunteerHubEntities();
            _userId = userId;
            LoadResults();
        }

        private void LoadResults()
        {
            var results = _context.TestResults
                .Where(r => r.UserID == _userId)
                .OrderByDescending(r => r.TestDate)
                .FirstOrDefault();

            if (results != null)
            {
                var resultTypeTextBlock = new TextBlock
                {
                    Text = $"Ваш тип волонтерства: {results.ResultType}",
                    FontSize = 36,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 10)
                };

                var resultDescriptionTextBlock = new TextBlock
                {
                    Text = GetResultDescription(results.ResultType),
                    FontSize = 24,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 10, 0, 20)
                };

                ResultsStackPanel.Children.Add(resultTypeTextBlock);
                ResultsStackPanel.Children.Add(resultDescriptionTextBlock);

                var resultAnswers = _context.TestResultAnswers
                    .Where(ra => ra.ResultID == results.ResultID)
                    .ToList();

                foreach (var resultAnswer in resultAnswers)
                {
                    var question = _context.TestQuestions.FirstOrDefault(q => q.QuestionID == resultAnswer.QuestionID);
                    var answer = _context.TestAnswers.FirstOrDefault(a => a.AnswerID == resultAnswer.AnswerID);

                    var questionTextBlock = new TextBlock
                    {
                        Text = question.QuestionText,
                        FontSize = 24,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 10, 0, 0)
                    };

                    var answerTextBlock = new TextBlock
                    {
                        Text = $"Ответ: {answer.AnswerText}",
                        FontSize = 20,
                        Margin = new Thickness(0, 5, 0, 20)
                    };

                    ResultsStackPanel.Children.Add(questionTextBlock);
                    ResultsStackPanel.Children.Add(answerTextBlock);
                }
            }
            else
            {
                ResultsStackPanel.Children.Add(new TextBlock
                {
                    Text = "Результаты не найдены.",
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 0)
                });
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private string GetResultDescription(string resultType)
        {
            switch (resultType)
            {
                case "Событийное волонтерство":
                    return "Подходит для тех, кто любит планировать и проводить мероприятия, работать с людьми и находиться в центре внимания. Событийное волонтерство это про тех, кто не готов посвящать все свое свободное время, но готов сделать свой вклад время от времени.";
                case "Экологическое волонтерство":
                    return "Подходит для тех, кто увлечен защитой окружающей среды, любит работать на открытом воздухе и хочет внести свой вклад в устойчивое развитие.";
                case "Спортивное волонтерство":
                    return "Подходит для тех, кто любит спорт, физическую активность и хочет поддержать свои любимые команды или мероприятия.";
                case "Патриотическое волонтерство":
                    return "Подходит для тех, кто увлечен своей страной, хочет отдать должное ветеранам и военнослужащим и внести свой вклад в свое сообщество.";
                case "Медиаволонтерство":
                    return "Подходит для тех, кто любит создавать контент, работать с социальными сетями и хочет использовать свои цифровые навыки для поддержки различных организаций.";
                default:
                    return "Тип результата по умолчанию";
            }
        }
    }
}
