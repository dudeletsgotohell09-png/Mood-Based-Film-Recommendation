using System;
using System.Windows;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class ResetPasswordWindow : Window
    {
        private readonly string _targetUsername;

        public ResetPasswordWindow(string username)
        {
            InitializeComponent();
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    Owner = win;
                    break;
                }
            }
            _targetUsername = username;
            TxtTargetUsername.Text = _targetUsername;
            TxtNewPassword.Focus();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";

            string newPwd = TxtNewPassword.Password;
            string confirmPwd = TxtConfirmNewPassword.Password;

            if (string.IsNullOrEmpty(newPwd) || string.IsNullOrEmpty(confirmPwd))
            {
                TxtError.Text = "All fields are required.";
                return;
            }

            if (newPwd.Length < 4)
            {
                TxtError.Text = "Password must be at least 4 characters.";
                return;
            }

            if (newPwd != confirmPwd)
            {
                TxtError.Text = "Passwords do not match.";
                return;
            }

            try
            {
                Repo.AdminResetPassword(_targetUsername, newPwd);
                MessageBox.Show($"Password for '{_targetUsername}' has been reset successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                TxtError.Text = ex.Message;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
