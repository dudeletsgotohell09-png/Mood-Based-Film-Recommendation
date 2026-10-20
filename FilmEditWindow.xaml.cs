using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MoodFlick.Models;
using MoodFlick.Services;

namespace MoodFlick.Views.Pages
{
    public partial class FilmEditWindow : Window
    {
        private readonly Film? _editingFilm;
        private readonly List<CheckBox> _moodCheckBoxes = new();

        public FilmEditWindow(Film? film = null)
        {
            InitializeComponent();
            _editingFilm = film;
            
            foreach (Window win in Application.Current.Windows)
            {
                if (win.GetType().Name == "MainWindow")
                {
                    Owner = win;
                    break;
                }
            }

            LoadMoodTags();

            if (_editingFilm != null)
            {
                TxtHeader.Text = "Edit Film";
                Title = "MoodFlick — Edit Film";
                PopulateForm();
            }
            else
            {
                TxtHeader.Text = "Add New Film";
                Title = "MoodFlick — Add New Film";
            }
        }

        private void LoadMoodTags()
        {
            var moods = Repo.GetMoodTags();
            if (moods.Count == 0)
            {
                // Fallback standard moods
                moods = new List<string> { "Happy", "Adventurous", "Romantic", "Sad", "Relaxed", "Tense" };
            }

            foreach (var mood in moods)
            {
                var cb = new CheckBox
                {
                    Content = mood,
                    Margin = new Thickness(0, 0, 16, 8),
                    FontSize = 13
                };
                _moodCheckBoxes.Add(cb);
                PnlMoods.Children.Add(cb);
            }
        }

        private void PopulateForm()
        {
            if (_editingFilm == null) return;

            TxtTitle.Text = _editingFilm.Title;
            TxtYear.Text = _editingFilm.Year.ToString();
            TxtRating.Text = _editingFilm.Rating.ToString("0.0");
            TxtDirector.Text = _editingFilm.Director ?? "";
            TxtCast.Text = _editingFilm.Cast ?? "";
            TxtDescription.Text = _editingFilm.Description ?? "";

            // Select Genre
            SelectComboItem(CboGenre, _editingFilm.Genre);
            
            // Select Language
            SelectComboItem(CboLanguage, _editingFilm.Language);

            // Select Era
            SelectComboItem(CboEra, _editingFilm.Era);

            // Select Mood CheckBoxes
            foreach (var cb in _moodCheckBoxes)
            {
                cb.IsChecked = _editingFilm.MoodTags.Contains(cb.Content.ToString() ?? "");
            }
        }

        private void SelectComboItem(ComboBox combo, string contentValue)
        {
            foreach (ComboBoxItem item in combo.Items)
            {
                if (string.Equals(item.Content.ToString(), contentValue, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedItem = item;
                    break;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";

            string title = TxtTitle.Text.Trim();
            string yearStr = TxtYear.Text.Trim();
            string ratingStr = TxtRating.Text.Trim();
            string genre = (CboGenre.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string language = (CboLanguage.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string era = (CboEra.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string director = TxtDirector.Text.Trim();
            string cast = TxtCast.Text.Trim();
            string desc = TxtDescription.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(yearStr) || string.IsNullOrEmpty(genre) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(era))
            {
                TxtError.Text = "Please fill in all required (*) fields.";
                return;
            }

            if (!int.TryParse(yearStr, out int year) || year < 1888 || year > DateTime.Now.Year + 2)
            {
                TxtError.Text = "Please enter a valid release year.";
                return;
            }

            decimal rating = 0;
            if (!string.IsNullOrEmpty(ratingStr) && (!decimal.TryParse(ratingStr, out rating) || rating < 0 || rating > 10))
            {
                TxtError.Text = "Rating must be between 0.0 and 10.0.";
                return;
            }

            // Read selected moods
            var selectedMoods = new List<string>();
            foreach (var cb in _moodCheckBoxes)
            {
                if (cb.IsChecked == true)
                {
                    selectedMoods.Add(cb.Content.ToString()!);
                }
            }

            if (selectedMoods.Count == 0)
            {
                TxtError.Text = "Please select at least one mood tag.";
                return;
            }

            try
            {
                var film = new Film
                {
                    FilmId = _editingFilm?.FilmId ?? 0,
                    Title = title,
                    Year = year,
                    Rating = rating,
                    Genre = genre,
                    Language = language,
                    Era = era,
                    Director = string.IsNullOrEmpty(director) ? null : director,
                    Cast = string.IsNullOrEmpty(cast) ? null : cast,
                    Description = string.IsNullOrEmpty(desc) ? null : desc,
                    MoodTags = selectedMoods
                };

                if (_editingFilm == null)
                {
                    Repo.AddFilm(film);
                }
                else
                {
                    Repo.UpdateFilm(film);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                TxtError.Text = "Save failed: " + ex.Message;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
