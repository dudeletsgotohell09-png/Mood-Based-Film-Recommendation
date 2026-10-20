using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class ProfilePage : UserControl
    {
        public ProfilePage()
        {
            InitializeComponent();
            PopulateProfileData();
        }

        private void PopulateProfileData()
        {
            if (Auth.Current == null) return;

            TxtUsername.Text = Auth.Current.Username;
            TxtRole.Text = Auth.Current.Role.ToUpper();
            TxtFullName.Text = Auth.Current.FullName;
            TxtCreated.Text = Auth.Current.CreatedAt.ToString("MMMM dd, yyyy");

            UpdateAvatar(Auth.Current.FullName);
        }

        private void UpdateAvatar(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                TxtAvatar.Text = "U";
                return;
            }

            var parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            for (int i = 0; i < Math.Min(parts.Length, 2); i++)
            {
                if (parts[i].Length > 0)
                {
                    sb.Append(char.ToUpper(parts[i][0]));
                }
            }

            TxtAvatar.Text = sb.Length > 0 ? sb.ToString() : "U";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";
            string fullName = TxtFullName.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                TxtError.Text = "Full Name is required.";
                return;
            }

            try
            {
                Repo.UpdateMyProfile(fullName);
                UpdateAvatar(fullName);
                
                // Refresh sidebar in MainWindow
                var parentWindow = Window.GetWindow(this) as MainWindow;
                parentWindow?.RefreshUserChip();

                MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                TxtError.Text = "Failed to update profile: " + ex.Message;
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var changePwdWin = new ChangePasswordWindow();
            changePwdWin.ShowDialog();
        }
    }
}
