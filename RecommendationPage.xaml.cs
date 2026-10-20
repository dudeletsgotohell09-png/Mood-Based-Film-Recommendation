using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MoodFlick.Models;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class RecommendationPage : UserControl
    {
        public RecommendationPage()
        {
            InitializeComponent();
        }

        private void SubmitQuiz_Click(object sender, RoutedEventArgs e)
        {
            string mood = (CboMood.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string genre = (CboGenre.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string language = (CboLanguage.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string era = (CboEra.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";

            if (string.IsNullOrEmpty(mood) || string.IsNullOrEmpty(genre) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(era))
            {
                MessageBox.Show("Please answer all pop quiz questions.", "MoodFlick", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Run priority-weighted recommendations search and log quiz history
            var recommendations = Repo.GetRecommendations(mood, genre, language, era, Auth.Current!.Username);

            // Display results
            TxtMatchPreferences.Text = $"Selected vibe: {mood} | {genre} | {language} | {era}";
            LstRecommendations.ItemsSource = recommendations;

            QuizGrid.Visibility = Visibility.Collapsed;
            ResultsGrid.Visibility = Visibility.Visible;
            DetailsPane.Visibility = Visibility.Collapsed; // Reset details pane

            if (recommendations.Count > 0)
            {
                LstRecommendations.SelectedIndex = 0; // Auto-select first item
            }
            else
            {
                MessageBox.Show("No films found matching your preferences. Please try a different combination!", "MoodFlick", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RetakeQuiz_Click(object sender, RoutedEventArgs e)
        {
            ResultsGrid.Visibility = Visibility.Collapsed;
            QuizGrid.Visibility = Visibility.Visible;
        }

        private void LstRecommendations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstRecommendations.SelectedItem is Film film)
            {
                TxtMovieTitle.Text = film.Title;
                TxtMovieYearGenre.Text = $"{film.Year} • {film.Genre}";
                TxtMovieRating.Text = $"{film.Rating:0.1}/10";
                TxtMovieLanguage.Text = film.Language;
                TxtMovieDirector.Text = string.IsNullOrEmpty(film.Director) ? "Unknown" : film.Director;
                TxtMovieCast.Text = string.IsNullOrEmpty(film.Cast) ? "N/A" : film.Cast;
                TxtMovieDescription.Text = string.IsNullOrEmpty(film.Description) ? "No description available." : film.Description;

                // Load moods badge list
                PnlMovieMoods.Children.Clear();
                foreach (var mood in film.MoodTags)
                {
                    PnlMovieMoods.Children.Add(BuildMoodBadge(mood));
                }

                DetailsPane.Visibility = Visibility.Visible;
            }
            else
            {
                DetailsPane.Visibility = Visibility.Collapsed;
            }
        }

        private FrameworkElement BuildMoodBadge(string mood)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xFB, 0xF1)), // Teal-100
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(10, 4, 10, 4),
                Margin = new Thickness(0, 0, 6, 6)
            };

            border.Child = new TextBlock
            {
                Text = mood,
                FontSize = 11,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(0x0F, 0x76, 0x6E)) // Teal-700
            };

            return border;
        }
    }
}
