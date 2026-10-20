using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MoodFlick.Models;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class UsersPage : UserControl
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            string search = TxtSearch.Text.Trim();
            string role = (CboRole.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "All";
            string sort = (CboSort.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Name (A-Z)";

            try
            {
                var list = Repo.GetUsers(search, role);

                // Apply Sorting
                if (sort == "Name (A-Z)")
                {
                    list = list.OrderBy(u => u.FullName).ToList();
                }
                else if (sort == "Name (Z-A)")
                {
                    list = list.OrderByDescending(u => u.FullName).ToList();
                }
                else if (sort == "Username")
                {
                    list = list.OrderBy(u => u.Username).ToList();
                }

                GridUsers.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load users: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (GridUsers == null) return;
            LoadUsers();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new UserAddWindow();
            if (addWin.ShowDialog() == true)
            {
                LoadUsers();
            }
        }

        private void ResetPwd_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string username)
            {
                var resetWin = new ResetPasswordWindow(username);
                resetWin.ShowDialog();
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is User user)
            {
                var editWin = new UserEditWindow(user);
                if (editWin.ShowDialog() == true)
                {
                    LoadUsers();
                    if (string.Equals(Auth.Current?.Username, user.Username, StringComparison.OrdinalIgnoreCase))
                    {
                        var parentWindow = Window.GetWindow(this) as MainWindow;
                        parentWindow?.RefreshUserChip();
                    }
                }
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is User user)
            {
                if (string.Equals(Auth.Current?.Username, user.Username, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("You cannot delete your own logged-in admin account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to delete user '{user.Username}' ({user.FullName})? This action cannot be undone.",
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Repo.DeleteUser(user.Username);
                        LoadUsers();
                        MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to delete user: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
