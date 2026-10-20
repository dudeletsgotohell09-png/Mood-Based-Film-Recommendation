using System.Windows;
using MoodFlick.Services;

namespace MoodFlick.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            TxtUsername.Focus();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";
            string username = TxtUsername.Text.Trim();
            string pwd = ChkShowPassword.IsChecked == true ? TxtPasswordReveal.Text : TxtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(pwd))
            {
                TxtError.Text = "Please enter your username and password.";
                return;
            }

            var user = Auth.Login(username, pwd);
            if (user == null)
            {
                TxtError.Text = "Invalid username or password.";
                TxtPassword.Clear();
                TxtPasswordReveal.Clear();
                return;
            }

            var main = new MainWindow();
            main.Show();
            Close();
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var signup = new RegisterWindow();
            signup.Show();
            Close();
        }

        private void ChkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            if (TxtPasswordReveal == null || TxtPassword == null) return;
            TxtPasswordReveal.Text = TxtPassword.Password;
            TxtPassword.Visibility = Visibility.Collapsed;
            TxtPasswordReveal.Visibility = Visibility.Visible;
            TxtPasswordReveal.Focus();
        }

        private void ChkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            if (TxtPasswordReveal == null || TxtPassword == null) return;
            TxtPassword.Password = TxtPasswordReveal.Text;
            TxtPasswordReveal.Visibility = Visibility.Collapsed;
            TxtPassword.Visibility = Visibility.Visible;
            TxtPassword.Focus();
        }
    }
}
