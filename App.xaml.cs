using System;
using System.Windows;
using System.Windows.Threading;
using MoodFlick.Data;

namespace MoodFlick
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += OnUnhandledException;

            try
            {
                Db.Initialize();
                Db.TestConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not connect to the MySQL database.\n\n" +
                    ex.Message + "\n\n" +
                    "Checklist:\n" +
                    " • MySQL server is running\n" +
                    " • Password in appsettings.json is correct\n" +
                    " • Database\\schema.sql script exists to auto-seed",
                    "MoodFlick — Database Connection Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    
                Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unexpected error occurred:\n\n" + e.Exception.Message, "MoodFlick — Error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}
