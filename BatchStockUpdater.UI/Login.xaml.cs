using System;
using System.Linq;
using System.Windows;
using BatchStockUpdater.Users;
using BatchStockUpdater.Core;

namespace BatchStockUpdater.UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private static bool _useAdminCreds;

        MainWindow _mainWindow;

        // Initialize Window and assign variables
        public LoginWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            UserNameTextBox.Focus();

            // Auto Login
            _useAdminCreds = true;

            if (_useAdminCreds)
            {
                UserNameTextBox.Text = "Admin";
                PasswordTextBox.Text = "Passw0rd";
            }
        }

        // Check user credentials and enable login if correct credentials
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            //var isCorrectCreds = CheckUserCredentials(UserNameTextBox.Text, PasswordTextBox.Text);
            var validAppUser = UserLogin.CheckUserCredentials(UserNameTextBox.Text, PasswordTextBox.Text);

            if (validAppUser != null)
            {
                _mainWindow.CurrentUserName = validAppUser.UserName;

                _mainWindow.LogInApproved(validAppUser.UserType);

                if (!_useAdminCreds)
                {
                    ClearUserNameAndPasswordTextBoxes();
                }

                this.Close();
            }
        }

        // Reset Username and Password TextBoxes
        private void ClearUserNameAndPasswordTextBoxes()
        {
            UserNameTextBox.Text = String.Empty;
            PasswordTextBox.Text = String.Empty;
        }

        

        // Hide instead of close the window - allows the window to be reopened again
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

    }
}
