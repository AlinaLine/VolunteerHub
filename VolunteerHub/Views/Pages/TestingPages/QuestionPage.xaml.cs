using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages.TestingPages
{
    public partial class QuestionPage : Page
    {
        private dbVolunteerHubEntities _context;
        private List<TestQuestions> _questions;
        private int _currentQuestionIndex = 0;
        private int? _currentUserId = null;
        private Dictionary<int, int> _selectedAnswers;

        public QuestionPage(int? userId = null)
        {
            InitializeComponent();
            _context = new dbVolunteerHubEntities();
            _currentUserId = userId;
            _selectedAnswers = new Dictionary<int, int>();
            LoadQuestions();
            DisplayCurrentQuestion();
        }

        private void LoadQuestions()
        {
            try
            {
                _questions = _context.TestQuestions.Include(q => q.TestAnswers).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при загрузке вопросов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayCurrentQuestion()
        {
            try
            {
                if (_currentQuestionIndex < _questions.Count)
                {
                    var question = _questions[_currentQuestionIndex];
                    QuestionNumberTextBlock.Text = $"Вопрос {_currentQuestionIndex + 1}";
                    QuestionTextBlock.Text = question.QuestionText;

                    AnswerOptionsPanel.Children.Clear();
                    foreach (var answer in question.TestAnswers)
                    {
                        var radioButton = new RadioButton
                        {
                            Content = answer.AnswerText,
                            Tag = answer.AnswerID,
                            GroupName = "Answers",
                            FontSize = 24,
                            Margin = new Thickness(0, 5, 0, 5)
                        };
                        AnswerOptionsPanel.Children.Add(radioButton);
                    }

                    AnswerOptionsPanel.Children.OfType<RadioButton>().ToList().ForEach(rb => rb.IsChecked = false);
                }
                else
                {
                    ShowResultsOrSave();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при отображении вопроса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedRadioButton = AnswerOptionsPanel.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);

                if (selectedRadioButton == null)
                {
                    MessageBox.Show("Пожалуйста, выберите один из вариантов ответа перед переходом к следующему вопросу.");
                    return;
                }

                int selectedAnswerId = (int)selectedRadioButton.Tag;
                if (_currentQuestionIndex < _questions.Count)
                {
                    _selectedAnswers[_questions[_currentQuestionIndex].QuestionID] = selectedAnswerId;
                }

                _currentQuestionIndex++;
                DisplayCurrentQuestion();
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show($"Ошибка при выборе ответа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowResultsOrSave()
        {
            try
            {
                string resultType = DetermineResultType();
                if (_currentUserId.HasValue)
                {
                    SaveResult(resultType);
                }
                ShowResults(resultType);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowResults(string resultType)
        {
            try
            {
                string resultDescription = GetResultDescription(resultType);
                ResultsPage resultsPage = new ResultsPage(resultType, resultDescription);
                NavigationService.Navigate(resultsPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при отображении результатов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveResult(string resultType)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var result = new TestResults
                    {
                        UserID = _currentUserId,
                        ResultType = resultType,
                        TestDate = DateTime.Now
                    };

                    _context.TestResults.Add(result);
                    _context.SaveChanges();

                    foreach (var answer in _selectedAnswers)
                    {
                        var resultAnswer = new TestResultAnswers
                        {
                            ResultID = result.ResultID,
                            QuestionID = answer.Key,
                            AnswerID = answer.Value
                        };
                        _context.TestResultAnswers.Add(resultAnswer);
                    }
                    _context.SaveChanges();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении результата: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetResultDescription(string resultType)
        {
            switch (resultType)
            {
                case "Событийное волонтерство":
                    return "подходит для тех, кто любит планировать и проводить мероприятия, работать с людьми и находиться в центре внимания. Событийное волонтерство это про тех, кто не готов посвящать все свое свободное время, но готов сделать свой вклад время от времени.";
                case "Экологическое волонтерство":
                    return "подходит для тех, кто увлечен защитой окружающей среды, любит работать на открытом воздухе и хочет внести свой вклад в устойчивое развитие.";
                case "Спортивное волонтерство":
                    return "подходит для тех, кто любит спорт, физическую активность и хочет поддержать свои любимые команды или мероприятия.";
                case "Патриотическое волонтерство":
                    return "подходит для тех, кто увлечен своей страной, хочет отдать должное ветеранам и военнослужащим и внести свой вклад в свое сообщество.";
                case "Медиаволонтерство":
                    return "подходит для тех, кто любит создавать контент, работать с социальными сетями и хочет использовать свои цифровые навыки для поддержки различных организаций.";
                default:
                    return "Тип результата по умолчанию";
            }
        }


        private string DetermineResultType()
        {
            Dictionary<string, int> volunteerTypes = new Dictionary<string, int>
    {
        { "Событийное волонтерство", 0 },
        { "Экологическое волонтерство", 0 },
        { "Спортивное волонтерство", 0 },
        { "Патриотическое волонтерство", 0 },
        { "Медиаволонтерство", 0 }
    };

            foreach (var answer in _selectedAnswers)
            {
                int questionId = answer.Key;

                if (questionId == 1 || questionId == 2 || questionId == 3)
                {
                    volunteerTypes["Событийное волонтерство"]++;
                    volunteerTypes["Экологическое волонтерство"]++;
                    volunteerTypes["Спортивное волонтерство"]++;
                    volunteerTypes["Медиаволонтерство"]++;
                }

                if (questionId == 7)
                {
                    volunteerTypes["Событийное волонтерство"]++;
                }

                if (questionId == 8)
                {
                    volunteerTypes["Экологическое волонтерство"]++;
                }

                if (questionId == 9)
                {
                    volunteerTypes["Спортивное волонтерство"]++;
                    volunteerTypes["Патриотическое волонтерство"]++;
                }

                if (questionId == 10)
                {
                    volunteerTypes["Патриотическое волонтерство"]++;
                }

                if (questionId == 5)
                {
                    volunteerTypes["Медиаволонтерство"]++;
                }
            }

            int maxScore = volunteerTypes.Values.Max();

            var topVolunteerTypes = volunteerTypes.Where(v => v.Value == maxScore).Select(v => v.Key).ToList();

            Random random = new Random();
            string resultType = topVolunteerTypes[random.Next(topVolunteerTypes.Count)];

            return resultType;
        }
    }
}

