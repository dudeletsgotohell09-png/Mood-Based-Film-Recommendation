using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MoodFlick.Services;
using MoodFlick.Views.Pages;

namespace MoodFlick.Views
{
    public partial class MainWindow : Window
    {
        private readonly List<Button> _navButtons = new();

        public MainWindow()
        {
            InitializeComponent();

            Application.Current.MainWindow = this;

            TxtUserName.Text = Auth.Current!.FullName;
            TxtUserRole.Text = Auth.Current.Role.ToUpper();

            BuildNav();
        }

        private void BuildNav()
        {
            var items = new List<(string label, string key, bool show)>
            {
                ("📊  Dashboard",      "dashboard", true),
                ("🎯  Recommender",     "quiz",      true),
                ("📂  Film Catalog",   "catalog",   true),
                ("👤  My Profile",     "profile",   true),
                ("👥  Users",          "users",     Auth.IsAdmin),
                ("🛡️  Activity Log",   "activity",  Auth.IsAdmin)
            };

            foreach (var (label, key, show) in items)
            {
                if (!show) continue;
                var btn = new Button
                {
                    Content = label,
                    Tag = key,
                    Style = (Style)Application.Current.Resources["NavButton"]
                };
                btn.Click += Nav_Click;
                _navButtons.Add(btn);
                NavPanel.Children.Add(btn);
            }

            // Default landing page
            Navigate("dashboard");
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.Tag is string key)
                Navigate(key);
        }

        public void Navigate(string key)
        {
            // Highlight active button
            foreach (var b in _navButtons)
            {
                bool isActive = (b.Tag as string) == key;
                b.Background = isActive
                    ? new SolidColorBrush(Color.FromArgb(26, 255, 255, 255))
                    : Brushes.Transparent;
                
                b.Foreground = isActive
                    ? Brushes.White
                    : new SolidColorBrush(Color.FromRgb(0x94, 0xA3, 0xB8)); // slate-400
            }

            UserControl page = key switch
            {
                "dashboard" => new DashboardPage(),
                "quiz"      => new RecommendationPage(),
                "catalog"   => new CatalogPage(),
                "profile"   => new ProfilePage(),
                "users"     => new UsersPage(),
                "activity"  => new ActivityLogPage(),
                _           => new DashboardPage()
            };
            ContentArea.Content = page;
        }

        public void RefreshUserChip()
        {
            TxtUserName.Text = Auth.Current!.FullName;
            TxtUserRole.Text = Auth.Current.Role.ToUpper();
        }

        private void UserPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Navigate("profile");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (MessageBox.Show("Are you sure you want to sign out?", "MoodFlick", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            Auth.Logout();
            var login = new LoginWindow();
            login.Show();
            Close();
        }
    }
}
