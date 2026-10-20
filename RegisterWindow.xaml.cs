using System.Windows;
using MoodFlick.Services;

namespace MoodFlick.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            TxtUsername.Focus();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";
            string username = TxtUsername.Text.Trim();
            string fullName = TxtFullName.Text.Trim();
            string pwd = ChkShowPassword.IsChecked == true ? TxtPasswordReveal.Text : TxtPassword.Password;
            string confPwd = ChkShowPassword.IsChecked == true ? TxtConfirmPasswordReveal.Text : TxtConfirmPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(pwd))
            {
                TxtError.Text = "All fields are required.";
                return;
            }

            if (pwd.Length < 4)
            {
                TxtError.Text = "Password must be at least 4 characters.";
                return;
            }

            if (pwd != confPwd)
            {
                TxtError.Text = "Passwords do not match.";
                return;
            }

            bool success = Auth.Register(username, fullName, pwd);
            if (!success)
            {
                TxtError.Text = "Username is already taken.";
                return;
            }

            MessageBox.Show("Registration successful! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            
            var login = new LoginWindow();
            login.Show();
            Close();
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            Close();
        }

        private void ChkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            if (TxtPasswordReveal == null || TxtPassword == null || TxtConfirmPasswordReveal == null || TxtConfirmPassword == null) return;
            
            TxtPasswordReveal.Text = TxtPassword.Password;
            TxtPassword.Visibility = Visibility.Collapsed;
            TxtPasswordReveal.Visibility = Visibility.Visible;

            TxtConfirmPasswordReveal.Text = TxtConfirmPassword.Password;
            TxtConfirmPassword.Visibility = Visibility.Collapsed;
            TxtConfirmPasswordReveal.Visibility = Visibility.Visible;
        }

        private void ChkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            if (TxtPasswordReveal == null || TxtPassword == null || TxtConfirmPasswordReveal == null || TxtConfirmPassword == null) return;

            TxtPassword.Password = TxtPasswordReveal.Text;
            TxtPasswordReveal.Visibility = Visibility.Collapsed;
            TxtPassword.Visibility = Visibility.Visible;

            TxtConfirmPassword.Password = TxtConfirmPasswordReveal.Text;
            TxtConfirmPasswordReveal.Visibility = Visibility.Collapsed;
            TxtConfirmPassword.Visibility = Visibility.Visible;
        }
    }
}
