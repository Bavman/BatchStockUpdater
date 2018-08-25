using System;
using System.Windows;
using System.Windows.Media;
using BatchStockUpdater.Users;
using BatchStockUpdater.Core;
using System.Text.RegularExpressions;

namespace BatchStockUpdater.UI
{
    /// <summary>
    /// Interaction logic for ManageUsers.xaml
    /// </summary>
    public partial class ManageUsersWindow : Window
    {

        private IUsersRepository _userRepository;
        private Logging _logging;
        private int _currentUserID;
        private IFormatProvider provider;

        private bool _isAddingNewUser = true;
        private bool _isUserNameOK = true;
        private bool _isPasswordOK = true;
        private bool _isFirstNameOK = true;
        private bool _isLastNameOK = true;
        private bool _isEmailOK = true;

        // Initialize Window and assign variables
        public ManageUsersWindow(IUsersRepository userRepository)
        {
            InitializeComponent();
            _userRepository = userRepository;
            _logging = Logging.GetInstance();
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
                _logging.LogDeleteUser(_userRepository.ReturnUser(_currentUserID).UserName);

                _userRepository.DeleteUser(_currentUserID);
                
                _currentUserID -= 1;
                UpdateUI(_currentUserID);

                EnableProtectedUserCheckBox();
            }
            
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            // Check following fields are correct
            if (_isAddingNewUser && _isUserNameOK &&_isPasswordOK && _isFirstNameOK && _isLastNameOK &&_isEmailOK)
            {
                _userRepository.UpdateUser(AssignPropsToUserClass());
                ResetBackgroundColours();
                _logging.LogAddUser(UserNameTextBox.Text);
            }
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isAddingNewUser)
            {
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
                    _logging.LogAddUser(newAppUser.UserName);

                    _currentUserID = _userRepository.ReturnLastUserID()-1;
                    _isAddingNewUser = false;
                    EnableNewUserButtons(true);
                    ResetBackgroundColours();
                    EnableProtectedUserCheckBox();
                }
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
            var inputBox = UserNameTextBox;

            if (inputBox.Text == String.Empty)
            {
                return;
            }

            var isValid = CheckFields(inputBox.Text, @"^[a-zA-Z0-9_]+$", @"can contain upper and lower case letters, numbers and _", "User Name");
            if (isValid)
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 127, 255, 127));
            }
            else
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
            }
            _isUserNameOK = isValid;
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var inputBox = PasswordTextBox;

            if (inputBox.Text == String.Empty)
            {
                return;
            }

            var isValid = CheckFields(PasswordTextBox.Text, @"^(?=.*\d)(?=.*[a-zA-Z]).{4,12}$", @" must contain at least one letter and one number and has to be between 4-12 characters long", "Password");
            if (isValid)
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 127, 255, 127));
            }
            else
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
            }
            _isPasswordOK = isValid;
        }

        private void FirstNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var inputBox = FirstNameTextBox;

            if (inputBox.Text == String.Empty)
            {
                return;
            }

            var isValid = CheckFields(inputBox.Text, @"^[a-zA-Z]+$", @"can contain upper and lower case letters, numbers and _", "First Name");
            if (isValid)
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 127, 255, 127));
            }
            else
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
            }
            _isFirstNameOK = isValid;
        }

        private void LastNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var inputBox = LastNameTextBox;

            if (inputBox.Text == String.Empty)
            {
                return;
            }

            var isValid = CheckFields(inputBox.Text, @"^[a-zA-Z]+$", @"can contain upper and lower case letters, numbers and _", "Last Name");
            if (isValid)
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 127, 255, 127));
            }
            else
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127)); 
            }
            _isLastNameOK = isValid;
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var inputBox = EmailTextBox;

            if (inputBox.Text == String.Empty)
            {
                return;
            }

            var isValid = CheckFields(inputBox.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", @"must be a valid email address", "Email");
            if (isValid)
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 127, 255, 127));
            }
            else
            {
                inputBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));
            }
            _isEmailOK = isValid;
        }
        #endregion

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
            
            var userName = UserNameTextBox.Text;
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
            date = DateTime.ParseExact(StartDateDatePickerTextBox.Text, "d", provider);
            user.StartDate = date;
            user.UserType = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), UserTypeComboBox.Text);
            user.ProtectedUser = (bool)ProtectedUserCheckBox.IsChecked;
            user.Inactive = (bool)UserInactiveCheckBox.IsChecked;
            return user;
        }

    }

}
