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
    public partial class NewsDisplayPage : Page
    {
        private int _newsId;

        public NewsDisplayPage(int newsId)
        {
            InitializeComponent();
            _newsId = newsId;
            LoadNewsContent();
        }

        private void LoadNewsContent()
        {
            using (var context = new dbVolunteerHubEntities())
            {
                var news = context.News.Find(_newsId);
                if (news == null)
                {
                    MessageBox.Show("Новость не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var contentBlocks = context.ContentBlocks
                    .Where(cb => cb.NewsID == _newsId)
                    .OrderBy(cb => cb.OrderIndex)
                    .ToList();

                foreach (var block in contentBlocks)
                {
                    if (block.ContentType == "Title")
                    {
                        AddTextBlock(block.Content, 20, FontWeights.Bold);
                    }
                    else if (block.ContentType == "Subtitle")
                    {
                        AddTextBlock(block.Content, 18, FontWeights.Bold);
                    }
                    else if (block.ContentType == "Text")
                    {
                        AddTextBlock(block.Content, 16, FontWeights.Normal);
                    }
                    else if (block.ContentType == "Image")
                    {
                        var newsImage = context.NewsImages.FirstOrDefault(ni => ni.ContentBlockID == block.ContentBlockID);
                        if (newsImage != null)
                        {
                            var image = context.Images.Find(newsImage.ImageID);
                            if (image != null)
                            {
                                AddImageBlock(image.ImageData);
                            }
                        }
                    }
                }
            }
        }

        private void AddTextBlock(string text, int fontSize, FontWeight fontWeight)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                FontSize = fontSize,
                FontWeight = fontWeight,
                Margin = new Thickness(0, 10, 0, 10),
                TextWrapping = TextWrapping.Wrap
            };
            ContentStackPanel.Children.Add(textBlock);
        }

        private void AddImageBlock(byte[] imageData)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(imageData);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            var image = new Image
            {
                Source = bitmap,
                Width = 400,
                Height = 300,
                Stretch = Stretch.Uniform,
                Margin = new Thickness(0, 0, 0, 10)
            };
            ContentStackPanel.Children.Add(image);
        }
    }
}
