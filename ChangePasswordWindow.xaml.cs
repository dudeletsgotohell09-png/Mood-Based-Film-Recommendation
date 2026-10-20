using System;
using System.Windows;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
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
            TxtCurrentPassword.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";

            string currentPwd = ChkShowPassword.IsChecked == true ? TxtCurrentPasswordReveal.Text : TxtCurrentPassword.Password;
            string newPwd = ChkShowPassword.IsChecked == true ? TxtNewPasswordReveal.Text : TxtNewPassword.Password;
            string confirmNewPwd = ChkShowPassword.IsChecked == true ? TxtConfirmNewPasswordReveal.Text : TxtConfirmNewPassword.Password;

            if (string.IsNullOrEmpty(currentPwd) || string.IsNullOrEmpty(newPwd) || string.IsNullOrEmpty(confirmNewPwd))
            {
                TxtError.Text = "All fields are required.";
                return;
            }

            if (newPwd.Length < 4)
            {
                TxtError.Text = "New password must be at least 4 characters.";
                return;
            }

            if (newPwd != confirmNewPwd)
            {
                TxtError.Text = "Passwords do not match.";
                return;
            }

            try
            {
                Repo.ChangeMyPassword(currentPwd, newPwd);
                MessageBox.Show("Password changed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                TxtError.Text = ex.Message;
            }
        }

        private void ChkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            if (TxtCurrentPasswordReveal == null || TxtCurrentPassword == null ||
                TxtNewPasswordReveal == null || TxtNewPassword == null ||
                TxtConfirmNewPasswordReveal == null || TxtConfirmNewPassword == null) return;

            TxtCurrentPasswordReveal.Text = TxtCurrentPassword.Password;
            TxtCurrentPassword.Visibility = Visibility.Collapsed;
            TxtCurrentPasswordReveal.Visibility = Visibility.Visible;

            TxtNewPasswordReveal.Text = TxtNewPassword.Password;
            TxtNewPassword.Visibility = Visibility.Collapsed;
            TxtNewPasswordReveal.Visibility = Visibility.Visible;

            TxtConfirmNewPasswordReveal.Text = TxtConfirmNewPassword.Password;
            TxtConfirmNewPassword.Visibility = Visibility.Collapsed;
            TxtConfirmNewPasswordReveal.Visibility = Visibility.Visible;
        }

        private void ChkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            if (TxtCurrentPasswordReveal == null || TxtCurrentPassword == null ||
                TxtNewPasswordReveal == null || TxtNewPassword == null ||
                TxtConfirmNewPasswordReveal == null || TxtConfirmNewPassword == null) return;

            TxtCurrentPassword.Password = TxtCurrentPasswordReveal.Text;
            TxtCurrentPasswordReveal.Visibility = Visibility.Collapsed;
            TxtCurrentPassword.Visibility = Visibility.Visible;

            TxtNewPassword.Password = TxtNewPasswordReveal.Text;
            TxtNewPasswordReveal.Visibility = Visibility.Collapsed;
            TxtNewPassword.Visibility = Visibility.Visible;

            TxtConfirmNewPassword.Password = TxtConfirmNewPasswordReveal.Text;
            TxtConfirmNewPasswordReveal.Visibility = Visibility.Collapsed;
            TxtConfirmNewPassword.Visibility = Visibility.Visible;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
