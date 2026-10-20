using System;
using System.Windows;
using System.Windows.Controls;
using MoodFlick.Models;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class CatalogPage : UserControl
    {
        public CatalogPage()
        {
            InitializeComponent();

            // Set role visibility
            if (Auth.IsAdmin)
            {
                BtnAddFilm.Visibility = Visibility.Visible;
                AdminActionsPanel.Visibility = Visibility.Visible;
            }

            LoadFilms();
        }

        private void LoadFilms()
        {
            if (GridFilms == null) return;
            try
            {
                string search = TxtSearch?.Text?.Trim() ?? "";
                string genre = (CboGenre?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All";
                string language = (CboLanguage?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All";
                string era = (CboEra?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All";

                var list = Repo.GetFilms(string.IsNullOrEmpty(search) ? null : search);

                if (genre != "All")
                {
                    list = list.FindAll(f => string.Equals(f.Genre, genre, StringComparison.OrdinalIgnoreCase));
                }
                if (language != "All")
                {
                    list = list.FindAll(f => string.Equals(f.Language, language, StringComparison.OrdinalIgnoreCase));
                }
                if (era != "All")
                {
                    list = list.FindAll(f => string.Equals(f.Era, era, StringComparison.OrdinalIgnoreCase));
                }

                list.Sort((x, y) => string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase));

                GridFilms.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load films: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            LoadFilms();
        }

        private void AddFilm_Click(object sender, RoutedEventArgs e)
        {
            var editWin = new FilmEditWindow();
            if (editWin.ShowDialog() == true)
            {
                LoadFilms();
            }
        }

        private void EditFilm_Click(object sender, RoutedEventArgs e)
        {
            if (GridFilms.SelectedItem is Film selectedFilm)
            {
                var editWin = new FilmEditWindow(selectedFilm);
                if (editWin.ShowDialog() == true)
                {
                    LoadFilms();
                }
            }
            else
            {
                MessageBox.Show("Please select a film from the table to edit.", "MoodFlick", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteFilm_Click(object sender, RoutedEventArgs e)
        {
            if (GridFilms.SelectedItem is Film selectedFilm)
            {
                if (MessageBox.Show($"Are you sure you want to delete '{selectedFilm.Title}'? This action cannot be undone.",
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Repo.DeleteFilm(selectedFilm.FilmId);
                        LoadFilms();
                        MessageBox.Show("Film deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to delete film: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a film from the table to delete.", "MoodFlick", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
