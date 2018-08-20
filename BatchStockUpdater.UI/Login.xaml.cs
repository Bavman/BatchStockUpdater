using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BatchStockUpdater.Users;
using BatchStockUpdater.Core;

namespace BatchStockUpdater.UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        IUsersRepository _usersRepository;
        MainWindow _mainWindow;
        Logging _logging;
        // Initialize Window and assign variables
        public LoginWindow(MainWindow mainWindow, IUsersRepository usersRepository)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _usersRepository = usersRepository;
            UserNameTextBox.Focus();
            _logging = Logging.GetInstance();

            // Auto Login
            UserNameTextBox.Text = "Admin";
            PasswordTextBox.Text = "Passw0rd";
        }

        // Check user credentials and enable login if correct credentials
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginCheck(UserNameTextBox.Text, PasswordTextBox.Text);
        }

        // Check the user credentials match and fire LogInApproved in the MainWinow
        private void LoginCheck(string userNameCheck, string passwordCheck)
        {
            var userList = _usersRepository.UserList;
            var userListQuery = userList.Where(u => u.UserName.Equals(userNameCheck)).ToArray();

            if (userListQuery.Length > 0)
            {
                if (userListQuery[0].Inactive)
                {
                    return;
                }
                if (userListQuery[0].Password == passwordCheck)
                {
                    _mainWindow.CurrentUserName = userNameCheck;

                    _mainWindow.LogInApproved(userListQuery[0].UserType);

                    _logging.LogLogin(userNameCheck, FailSuccessStatus.Success);

                    ClearUserNameAndPasswordTextBoxes();

                    this.Close();
                }
                else
                {
                    FailedLoginMessage();
                    _logging.LogLogin(userNameCheck, FailSuccessStatus.Failure);
                }
            }
            else
            {
                FailedLoginMessage();
                _logging.LogLogin(userNameCheck, FailSuccessStatus.Failure);
            }
        }

        // Reset Username and Password TextBoxes
        private void ClearUserNameAndPasswordTextBoxes()
        {
            UserNameTextBox.Text = String.Empty;
            PasswordTextBox.Text = String.Empty;
        }

        // User credentials messagebox
        private void FailedLoginMessage()
        {
            MessageBox.Show("Sorry, user credentials do not match.");
        }

        // Hide instead of close the window - allows the window to be reopened again
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

    }
}
