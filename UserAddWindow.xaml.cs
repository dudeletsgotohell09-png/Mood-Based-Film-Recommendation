using System;
using System.Windows;
using System.Windows.Controls;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class UserAddWindow : Window
    {
        public UserAddWindow()
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
            TxtUsername.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";

            string username = TxtUsername.Text.Trim();
            string fullName = TxtFullName.Text.Trim();
            string role = (CboRole.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "user";
            string password = TxtPassword.Password;
            string confirmPwd = TxtConfirmPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(password))
            {
                TxtError.Text = "All fields are required.";
                return;
            }

            if (password.Length < 4)
            {
                TxtError.Text = "Password must be at least 4 characters.";
                return;
            }

            if (password != confirmPwd)
            {
                TxtError.Text = "Passwords do not match.";
                return;
            }

            try
            {
                Repo.AddUser(username, fullName, role, password);
                MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                TxtError.Text = ex.Message;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
