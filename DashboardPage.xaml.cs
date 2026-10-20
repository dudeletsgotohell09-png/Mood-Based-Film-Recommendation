using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class DashboardPage : UserControl
    {
        public DashboardPage()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            // Set welcome message
            TxtWelcome.Text = $"Welcome back, {Auth.Current!.FullName}!";

            // Fetch statistics
            var (totalFilms, activeUsers, avgRating) = Repo.GetGeneralStats();

            AddStatCard("Total Films", totalFilms.ToString(), PrimaryBrush());
            AddStatCard("Active Users (30d)", activeUsers.ToString(), AccentBrush());
            AddStatCard("Average Rating", Math.Round(avgRating, 1).ToString("0.0") + "/10", SuccessBrush());

            // Build Mood Popularity Chart
            var popularity = Repo.GetMoodPopularity();
            int maxCount = popularity.Count > 0 ? popularity.Max(x => x.Value) : 1;

            MoodChartItems.Items.Clear();
            if (popularity.Count == 0)
            {
                MoodChartItems.Items.Add(new TextBlock
                {
                    Text = "No recommendation history recorded yet. Take the quiz to see metrics!",
                    Foreground = MutedBrush(),
                    FontSize = 13,
                    Margin = new Thickness(0, 10, 0, 0)
                });
            }
            else
            {
                foreach (var kvp in popularity)
                {
                    double pct = maxCount > 0 ? (double)kvp.Value / maxCount * 100 : 0;
                    MoodChartItems.Items.Add(BuildBarRow(kvp.Key, $"{kvp.Value} quiz submission{(kvp.Value > 1 ? "s" : "")}", pct, PrimaryBrush()));
                }
            }
        }

        private void AddStatCard(string label, string value, Brush accent)
        {
            var border = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(18, 16, 18, 16),
                Margin = new Thickness(0, 0, 16, 16),
                Width = 230,
                BorderThickness = new Thickness(4, 0, 0, 0),
                BorderBrush = accent,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Black,
                    Opacity = 0.05,
                    BlurRadius = 8,
                    ShadowDepth = 1
                }
            };

            var sp = new StackPanel();
            sp.Children.Add(new TextBlock
            {
                Text = label.ToUpper(),
                FontSize = 10,
                FontWeight = FontWeights.SemiBold,
                Foreground = MutedBrush()
            });
            sp.Children.Add(new TextBlock
            {
                Text = value,
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Foreground = TextDarkBrush(),
                Margin = new Thickness(0, 4, 0, 0)
            });

            border.Child = sp;
            StatsItems.Items.Add(border);
        }

        private FrameworkElement BuildBarRow(string moodName, string subtext, double percentage, Brush color)
        {
            var grid = new Grid { Margin = new Thickness(0, 8, 0, 8) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

            // Mood Name & Emoji Tag
            string emoji = moodName switch
            {
                "Happy" => "😊",
                "Adventurous" => "🧗",
                "Romantic" => "💖",
                "Sad" => "😢",
                "Relaxed" => "🧘",
                "Tense" => "😰",
                _ => "🎬"
            };

            var nameLabel = new TextBlock
            {
                Text = $"{emoji}  {moodName}",
                FontWeight = FontWeights.SemiBold,
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(nameLabel, 0);
            grid.Children.Add(nameLabel);

            // Progress bar filler container
            var track = new Border
            {
                Height = 8,
                Background = new SolidColorBrush(Color.FromRgb(0xF1, 0xF5, 0xF9)), // slate-100
                CornerRadius = new CornerRadius(4),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0)
            };
            var fill = new Border
            {
                Height = 8,
                Background = color,
                CornerRadius = new CornerRadius(4),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0),
                Width = Math.Max(4, percentage / 100.0 * 200) // Base length for chart width
            };
            
            var progressHolder = new Grid();
            progressHolder.Children.Add(track);
            progressHolder.Children.Add(fill);
            Grid.SetColumn(progressHolder, 1);
            grid.Children.Add(progressHolder);

            // Subtext/Count label
            var countLabel = new TextBlock
            {
                Text = subtext,
                FontSize = 12,
                Foreground = MutedBrush(),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            Grid.SetColumn(countLabel, 2);
            grid.Children.Add(countLabel);

            return grid;
        }

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow?.Navigate("quiz");
        }

        private void BrowseCatalog_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow?.Navigate("catalog");
        }

        private Brush PrimaryBrush() => (Brush)Application.Current.Resources["Primary"];
        private Brush AccentBrush()  => (Brush)Application.Current.Resources["Accent"];
        private Brush SuccessBrush() => (Brush)Application.Current.Resources["Success"];
        private Brush MutedBrush()   => (Brush)Application.Current.Resources["Muted"];
        private Brush TextDarkBrush() => (Brush)Application.Current.Resources["TextDark"];
    }
}
