using System;
using System.Data;
using System.Windows.Controls;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class ActivityLogPage : UserControl
    {
        public ActivityLogPage()
        {
            InitializeComponent();
            LoadLogs();
        }

        private void LoadLogs()
        {
            try
            {
                string search = TxtSearch.Text.Trim();
                DataTable dt = Repo.GetActivityLogs(string.IsNullOrEmpty(search) ? null : search);
                GridLogs.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading activity logs: " + ex.Message);
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadLogs();
        }

        private void Refresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadLogs();
        }
    }
}
