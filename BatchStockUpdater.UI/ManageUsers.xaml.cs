using System;
using System.Windows;
using System.Windows.Media;
using BatchStockUpdater.Users;
using BatchStockUpdater.Core;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace BatchStockUpdater.UI
{
    /// <summary>
    /// Interaction logic for ManageUsers.xaml
    /// </summary>
    public partial class ManageUsersWindow : Window
    {

        private IUsersRepository _userRepository;
        private int _currentUserID;
        private IFormatProvider _provider;

        private AppUser _newUser;

        private bool _isAddingNewUser = false;

        // Initialize Window and assign variables
        public ManageUsersWindow(IUsersRepository userRepository)
        {
            InitializeComponent();
            _userRepository = userRepository;
            InitializeUI();
        }

        #region Buttons and checkboxes
        // Following methods are button and checkbox callbacks 
        private void NewUserButton_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            _currentUserID = _userRepository.ReturnLastUserID();
            IDTextBox.Text = _currentUserID.ToString();
            _isAddingNewUser = true;

            ProtectedUserCheckBox.IsEnabled = true;
            
            EnableNewUserButtons(false);
            ResetBackgroundColours();

            _newUser = new AppUser()
            {
                ID = _currentUserID
            };
        }

        private void NextUserButton_Click(object sender, RoutedEventArgs e)
        {
            var lastUserID = _userRepository.ReturnLastUserID();

            if (_currentUserID < lastUserID - 1)
            {
                _currentUserID += 1;
                UpdateUI(_currentUserID);
                EnableProtectedUserCheckBox();
            }
        }

        private void PreviousUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUserID > 0)
            {
                _currentUserID -= 1;
                UpdateUI(_currentUserID);
                EnableProtectedUserCheckBox();
            }
        }

        private void RemoveUserButton_Click(object sender, RoutedEventArgs e)
        {

            if (_currentUserID != 0)
            {
                Logging.GetInstance().LogDeleteUser(_userRepository.ReturnUser(_currentUserID).UserName);

                _userRepository.DeleteUser(_currentUserID);
                
                _currentUserID -= 1;
                UpdateUI(_currentUserID);

                EnableProtectedUserCheckBox();
            }
            
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {

            var updateUser = AssignPropsToUserClass();

            // Check following fields are correct
            if (CheckUserFields(updateUser))
            {
                _userRepository.UpdateUser(updateUser);
                ResetBackgroundColours();
                Logging.GetInstance().LogAddUser(UserNameTextBox.Text);
                MessageBox.Show("User details updated.");
            }
            else
            {
                MessageBox.Show("User details have not been updated.");
            }
            
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isAddingNewUser)
            {
                if (CheckUserFields(_newUser))
                {
                    if (_userRepository.DoesUserExist(UserNameTextBox.Text))
                    {
                        MessageBox.Show("User name already exists, please try a different user name");
                        return;
                    }

                    var newAppUser = AssignPropsToUserClass();

                    if (newAppUser == null)
                    {
                        return;
                    }

                    _userRepository.AddUser(newAppUser);
                    Logging.GetInstance().LogAddUser(newAppUser.UserName);

                    _currentUserID = _userRepository.ReturnLastUserID() - 1;
                    _isAddingNewUser = false;
                    EnableNewUserButtons(true);
                    ResetBackgroundColours();
                    EnableProtectedUserCheckBox();
                }
                /*
                if (_isAddingNewUser && _isUserNameOK && _isPasswordOK && _isFirstNameOK && _isLastNameOK && _isEmailOK)
                {
                    if (_userRepository.DoesUserExist(UserNameTextBox.Text))
                    {
                        MessageBox.Show("User name already exists, please try a different user name");
                        return;
                    }

                    var newAppUser = AssignPropsToUserClass();

                    if (newAppUser == null)
                    {
                        return;
                    }

                    _userRepository.AddUser(newAppUser);
                    Logging.GetInstance().LogAddUser(newAppUser.UserName);

                    _currentUserID = _userRepository.ReturnLastUserID()-1;
                    _isAddingNewUser = false;
                    EnableNewUserButtons(true);
                    ResetBackgroundColours();
                    EnableProtectedUserCheckBox();
                }*/
            }
        }

        private void ProtectedUserCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RemoveUserButton.IsEnabled = false;
        }

        private void ProtectedUserCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RemoveUserButton.IsEnabled = true;
        }

        private void EnableNewUserButtons(bool isEnabled)
        {
            NextUserButton.IsEnabled = isEnabled;
            PreviousUserButton.IsEnabled = isEnabled;
            RemoveUserButton.IsEnabled = isEnabled;
            UpdateUserButton.IsEnabled = isEnabled;
            AddUserButton.IsEnabled = !isEnabled;
        }

        #endregion

        #region LostFocus Character Check
        // Checks the textbox input field matches the string requirements for the text fields.
        private void UserNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NewUserTextBoxLostFocus(UserNameTextBox);
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NewUserTextBoxLostFocus(PasswordTextBox);
        }

        private void FirstNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NewUserTextBoxLostFocus(FirstNameTextBox);
        }

        private void LastNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NewUserTextBoxLostFocus(LastNameTextBox);
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NewUserTextBoxLostFocus(EmailTextBox);
        }

        #endregion


        // Updates TextBoxes when adding new users
        private void NewUserTextBoxLostFocus(TextBox inputBox)
        {

            if (inputBox.Text == String.Empty)
            {
                inputBox.Background = new SolidColorBrush(Colors.White);
                return;
            }

            if (_isAddingNewUser)
            {

                var returnField = String.Empty;

                switch (inputBox.Name)
                {

                    case "UserNameTextBox":
                        _newUser.UserName = inputBox.Text;
                        returnField = _newUser.UserName;
                        break;
                    case "PasswordTextBox":
                        _newUser.Password = inputBox.Text;
                        returnField = _newUser.Password;
                        break;
                    case "FirstNameTextBox":
                        _newUser.FirstName = inputBox.Text;
                        returnField = _newUser.FirstName;
                        break;
                    case "LastNameTextBox":
                        _newUser.LastName = inputBox.Text;
                        returnField = _newUser.LastName;
                        break;
                    case "EmailTextBox":
                        _newUser.Email = inputBox.Text;
                        returnField = _newUser.Email;
                        break;
                }

                ColourTextCells(inputBox, returnField);
            }
        }

        // Colour TextBox bacgrounds based on complient text input
        private void ColourTextCells(TextBox textBox, string textBoxString)
        {
            if (textBoxString != null)
            {
                textBox.Background = new SolidColorBrush(Color.FromArgb(255, 127, 255, 127));
            }
            else
            {
                textBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
            }
        }

        private bool CheckUserFields(AppUser appUser)
        {
            var isValid = true; 

            if (appUser.UserName == null || appUser.UserName == String.Empty)
            {
                UserNameTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
                isValid = false;
            }
            if (appUser.Password == null || appUser.Password == String.Empty)
            {
                PasswordTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
                isValid = false;
            }
            if (appUser.FirstName == null || appUser.FirstName == String.Empty)
            {
                FirstNameTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
                isValid = false;
            }
            if (appUser.LastName == null || appUser.LastName == String.Empty)
            {
                LastNameTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
                isValid = false;
            }
            if (appUser.Email == null)
            {
                EmailTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
                isValid = false;
            }

            return isValid;
        }

        // Hide instead of close the window - allows the window to be reopened again
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }

        // Reset colours when adding, modifying or sequencing through users
        private void ResetBackgroundColours()
        {
            UserNameTextBox.Background = new SolidColorBrush(Colors.White);
            PasswordTextBox.Background = new SolidColorBrush(Colors.White);
            FirstNameTextBox.Background = new SolidColorBrush(Colors.White);
            LastNameTextBox.Background = new SolidColorBrush(Colors.White);
            EmailTextBox.Background = new SolidColorBrush(Colors.White);
        }

        // Enable the use of the ProtectedUser checkbox when the user is not the administrator at id 0
        private void EnableProtectedUserCheckBox()
        {
            if (_currentUserID == 0)
            {
                ProtectedUserCheckBox.IsEnabled = false;
                UserInactiveCheckBox.IsEnabled = false;
            }
            else
            {
                ProtectedUserCheckBox.IsEnabled = true;
                UserInactiveCheckBox.IsEnabled = true;
            }
        }

        // Load user types into combobox and fill populate UI input fields.
        private void InitializeUI()
        {
            UserTypeComboBox.Items.Add("Staff");
            UserTypeComboBox.Items.Add("Manager");
            UserTypeComboBox.Items.Add("Administrator");

            UpdateUI(0);
        }

        // Clear UI infput fields
        private void ClearUI()
        {
            IDTextBox.Clear();
            UserNameTextBox.Clear();
            PasswordTextBox.Clear();
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            EmailTextBox.Clear();
            var date = new DateTime();
            date = DateTime.Today;
            StartDateDatePickerTextBox.Text = date.ToShortDateString();
            UserTypeComboBox.Text = "Staff";
            ProtectedUserCheckBox.IsChecked = false;
            UserInactiveCheckBox.IsChecked = false;
        }

        // Update UI input fields from the AppUser class
        private void UpdateUI(int userID)
        {
            var currentUser = _userRepository.ReturnUser(userID);

            _currentUserID = userID;

            IDTextBox.Text = currentUser.ID.ToString();
            UserNameTextBox.Text = currentUser.UserName;
            PasswordTextBox.Text = currentUser.Password;
            FirstNameTextBox.Text = currentUser.FirstName;
            LastNameTextBox.Text = currentUser.LastName;
            EmailTextBox.Text = currentUser.Email;
            StartDateDatePickerTextBox.Text = currentUser.StartDate.ToShortDateString();
            UserTypeComboBox.Text = currentUser.UserType.ToString();
            ProtectedUserCheckBox.IsChecked = currentUser.ProtectedUser;
            UserInactiveCheckBox.IsChecked = currentUser.Inactive;
        }

        // Checks the UI input fields making sure they match the required length and symbols
        // of the requested field. eg. email address requres the user to enter a valid email address
        // not just any text.
        private bool CheckFields(string prop, string matchChars, string mustContainMessage, string fieldName)
        {

            var isValid = Regex.IsMatch(prop, matchChars);
            Console.WriteLine(isValid);
            if (!isValid)
            {
                MessageBox.Show("The " + fieldName + mustContainMessage + ".");

                return false;
            }

            return true; 
        }

        // Assign properties from the UI input fields to the AppUser class and return the class.
        private AppUser AssignPropsToUserClass()
        {
            var user = new AppUser();

            user.ID = _currentUserID;
            user.UserName = UserNameTextBox.Text;
            user.Password = PasswordTextBox.Text;
            user.FirstName = FirstNameTextBox.Text;
            user.LastName = LastNameTextBox.Text;
            user.Email = EmailTextBox.Text;

            var date = new DateTime();
            date = DateTime.ParseExact(StartDateDatePickerTextBox.Text, "d", _provider);
            user.StartDate = date;
            user.UserType = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), UserTypeComboBox.Text);
            user.ProtectedUser = (bool)ProtectedUserCheckBox.IsChecked;
            user.Inactive = (bool)UserInactiveCheckBox.IsChecked;
            return user;
        }

    }

}
