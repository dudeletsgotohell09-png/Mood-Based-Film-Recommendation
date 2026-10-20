using System;
using System.Windows;
using System.Windows.Controls;
using MoodFlick.Models;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class UserEditWindow : Window
    {
        private readonly string _username;

        public UserEditWindow(User user)
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

            _username = user.Username;
            TxtUsername.Text = user.Username;
            TxtFullName.Text = user.FullName;

            foreach (ComboBoxItem item in CboRole.Items)
            {
                if (item.Content.ToString() == user.Role)
                {
                    CboRole.SelectedItem = item;
                    break;
                }
            }

            if (string.Equals(Auth.Current?.Username, user.Username, StringComparison.OrdinalIgnoreCase))
            {
                CboRole.IsEnabled = false; // Cannot demote yourself
            }

            TxtFullName.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";

            string fullName = TxtFullName.Text.Trim();
            string role = (CboRole.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "user";

            if (string.IsNullOrEmpty(fullName))
            {
                TxtError.Text = "Full Name is required.";
                return;
            }

            try
            {
                Repo.UpdateUser(_username, fullName, role);
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
