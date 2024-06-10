using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VolunteerHub.Model;

namespace VolunteerHub.Views.Pages
{
    public partial class NewsPage : Page
    {
        private List<ContentBlock> contentBlocks = new List<ContentBlock>();

        public NewsPage()
        {
            InitializeComponent();
        }

        private void AddTitleBlock_Click(object sender, RoutedEventArgs e)
        {
            AddTextBlock("Заголовок", 20, FontWeights.Bold, "Title");
        }

        private void AddSubtitleBlock_Click(object sender, RoutedEventArgs e)
        {
            AddTextBlock("Подзаголовок", 18, FontWeights.Bold, "Subtitle");
        }

        private void AddTextBlock_Click(object sender, RoutedEventArgs e)
        {
            AddTextBlock("Контент", 16, FontWeights.Normal, "Text");
        }

        private void AddTextBlock(string placeholder, int fontSize, FontWeight fontWeight, string contentType)
        {
            var textBlock = new TextBlock
            {
                Text = placeholder,
                FontSize = fontSize,
                FontWeight = fontWeight,
                Margin = new Thickness(0, 10, 0, 5)
            };
            var richTextBox = new RichTextBox
            {
                FontSize = fontSize,
                FontWeight = fontWeight,
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(5),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C4DFF")),
                BorderThickness = new Thickness(2)
            };
            richTextBox.Tag = contentType;
            ContentStackPanel.Children.Add(textBlock);
            ContentStackPanel.Children.Add(richTextBox);

            contentBlocks.Add(new ContentBlock
            {
                ContentType = contentType,
                UIElement = richTextBox
            });
        }

        private void AddImageBlock_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    byte[] imageData = File.ReadAllBytes(filename);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(imageData);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    var image = new Image
                    {
                        Source = bitmap,
                        Width = 400,
                        Height = 400,
                        Stretch = Stretch.Uniform,
                        Margin = new Thickness(0, 0, 0, 10)
                    };
                    ContentStackPanel.Children.Add(image);

                    contentBlocks.Add(new ContentBlock
                    {
                        ContentType = "Image",
                        ImageData = imageData,
                        UIElement = image
                    });
                }
            }
        }

        private void RemoveLastBlock_Click(object sender, RoutedEventArgs e)
        {
            if (contentBlocks.Count > 0)
            {
                var lastBlock = contentBlocks.Last();
                ContentStackPanel.Children.Remove(lastBlock.UIElement);

                if (lastBlock.ContentType != "Image")
                {
                    ContentStackPanel.Children.Remove(ContentStackPanel.Children[ContentStackPanel.Children.Count - 1]);
                }

                contentBlocks.Remove(lastBlock);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;

            int orderIndex = 0;

            foreach (var contentBlock in contentBlocks)
            {
                if (contentBlock.ContentType == "Text" || contentBlock.ContentType == "Title" || contentBlock.ContentType == "Subtitle")
                {
                    var richTextBox = contentBlock.UIElement as RichTextBox;
                    string content = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
                    contentBlock.Content = content;
                }
                contentBlock.OrderIndex = orderIndex++;
            }

            if (string.IsNullOrWhiteSpace(title) || contentBlocks.Count == 0)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveNewsToDatabase(title, contentBlocks);

            MessageBox.Show("Новость успешно сохранена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            TitleTextBox.Clear();
            ContentStackPanel.Children.Clear();
            contentBlocks.Clear();
        }

        private void SaveNewsToDatabase(string title, List<ContentBlock> contentBlocks)
        {
            using (var context = new dbVolunteerHubEntities())
            {
                var news = new News
                {
                    Title = title,
                    PublicationDate = DateTime.Now
                };

                context.News.Add(news);
                context.SaveChanges(); 

                foreach (var contentBlock in contentBlocks)
                {
                    var contentBlockEntity = new ContentBlocks
                    {
                        NewsID = news.NewsID,
                        ContentType = contentBlock.ContentType,
                        Content = contentBlock.ContentType == "Image" ? null : contentBlock.Content,
                        OrderIndex = contentBlock.OrderIndex
                    };

                    context.ContentBlocks.Add(contentBlockEntity);
                    context.SaveChanges(); 

                    if (contentBlock.ContentType == "Image")
                    {
                        var image = new Images
                        {
                            ImageData = contentBlock.ImageData,
                            UploadDate = DateTime.Now
                        };

                        context.Images.Add(image);
                        context.SaveChanges();

                        var newsImage = new NewsImages
                        {
                            ContentBlockID = contentBlockEntity.ContentBlockID,
                            ImageID = image.ImageID
                        };

                        context.NewsImages.Add(newsImage);
                        context.SaveChanges();
                    }
                }
            }
        }
    }

    public class ContentBlock
    {
        public string ContentType { get; set; }
        public string Content { get; set; }
        public byte[] ImageData { get; set; }
        public int OrderIndex { get; set; }
        public UIElement UIElement { get; set; } 
    }
}
