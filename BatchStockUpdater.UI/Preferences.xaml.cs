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
using BatchStockUpdater.Core;
//using System.Windows.Forms;

namespace BatchStockUpdater.UI
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class PreferencesWindow : Window
    {
        // Variables
        Prefs _prefs;
        string _daysBeforeText;
        string _timeText;

        // Initialize Window and assign variables
        public PreferencesWindow()
        {
            InitializeComponent();

            _prefs = Prefs.GetInstance();

            InitializeUI();
        }

        #region UIElements
        // Sets CheckFileDateAndTime bool and saves preferences to file.
        private void EnableDateTimeCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (_prefs != null)
            {
                _prefs.CheckFileDateAndTime = true;
                _prefs.SavePrefs();

                EnableDisableUIItems(true);
            }
        }

        // Sets CheckFileDateAndTime bool and saves preferences to file.
        private void EnableDateTimeCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_prefs != null)
            {
                _prefs.CheckFileDateAndTime = false;
                _prefs.SavePrefs();

                EnableDisableUIItems(false);
            }
        }

        // Sets stocklist.csv import folder and saves preferenfes to file.
        private void ImportFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            var folderPath = String.Empty;

            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _prefs.FolderName = folderBrowser.SelectedPath;
                FolderLabel.Content = "FOLDER : " + _prefs.FolderName;
                _prefs.SavePrefs();
            }
            
        }

        // Updates the _timeText variable and gets processed when the textBox focus is lost (TimeText_LostFocus)
        private void TimeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            _timeText = TimeTextBox.Text;
            SavePrefsButton.IsEnabled = true;
        }

        // Updates _daysBefore DaysBeforeTextBox and gets processed when the textBox focus is lost (DaysBeforeText_LostFocus)
        private void DaysBeforeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            _daysBeforeText = DaysBeforeTextBox.Text;
            SavePrefsButton.IsEnabled = true;
        }

        #endregion

        // Checks time entered into the TimeTextBox complies with the 24 hours standard
        private void ParseTextToTime()
        {
            var timeArray = CheckTimeFormat(_timeText);
            if (timeArray != null)
            {
                var timeSpan = new TimeSpan(timeArray[0], timeArray[1], 00);
                _prefs.TimeCheck = timeSpan;
            }
            else
            {
                TimeTextBox.Text = _prefs.TimeCheck.ToString(@"hh\:mm");
                MessageBox.Show("Only Accepts 24 hour time format eg. 15:30");
            }
        }

        // Checks text can be parsed to int, adds to variable in the Prefs class and saves to file.
        private void ParseDaysBeforeToInt()
        {
            var isInt = int.TryParse(_daysBeforeText, out var days);
            if (isInt)
            {

                _prefs.NDaysBefore = days;
            }
            else
            {
                DaysBeforeTextBox.Text = _prefs.NDaysBefore.ToString();
                MessageBox.Show("Only Accepts whole numbers, 0, 1, 2, 3..etc");
            }
        }

        // Initializes the Preferences UI. Removes the callback on EnableDateTimeCheckBox.Checked and *.Unchecked,
        // applies the bool then assigns the callbacks. This prevents the callbacks being triggerend when the checkbox
        // is initialized.
        public void InitializeUI()
        {
            // Remove EnableDateTimeCheckBox.Checked event handler to set the value then enable again.
            EnableDateTimeCheckBox.Checked -= EnableDateTimeCheck_Checked;
            EnableDateTimeCheckBox.Unchecked -= EnableDateTimeCheck_Unchecked;

            EnableDateTimeCheckBox.IsChecked = _prefs.CheckFileDateAndTime;

            EnableDateTimeCheckBox.Checked += EnableDateTimeCheck_Checked;
            EnableDateTimeCheckBox.Unchecked += EnableDateTimeCheck_Unchecked;

            EnableDisableUIItems(_prefs.CheckFileDateAndTime);


            TimeTextBox.Text = _prefs.TimeCheck.ToString(@"hh\:mm");
            DaysBeforeTextBox.Text = _prefs.NDaysBefore.ToString();
            FolderLabel.Content = "FOLDER : "+_prefs.FolderName;
        }

        // Checks the time format before apply the the variable in Prefs.
        private int[] CheckTimeFormat(string timeString)
        {

            string[] charArray = null;
            charArray = timeString.Split(':');

            // Check that a : makes to string items in array
            if (charArray.Length != 2)
            {
                return null;
            }
            // Check first two digits
            if (!CheckTimeDigits(charArray[0], "hours"))
            {
                return null;
            }
            if (!CheckTimeDigits(charArray[1], "mins"))
            {
                return null;
            }

            // Return in array (hh:mm)
            var timeArray = new int[2] { int.Parse(charArray[0]), int.Parse(charArray[1])};
            return timeArray;
        }

        // Checks the time numbers can be parsed into numbers and the hours are not greater than 23
        // and the minutes are not greater than 59.
        private bool CheckTimeDigits(string timeDigits, string hoursMins)
        {
            
            // Check parsted string is an int
            var isInt = int.TryParse(timeDigits, out var hour);
            if (!isInt)
            {
                return false;
            }

            if (hoursMins == "hours")
            {
                // Check string is 1 or 2 characters long
                if (timeDigits.Length < 1 || timeDigits.Length > 2)
                {
                    return false;
                }

                if (int.Parse(timeDigits) > 23)
                {
                    return false;
                }
            }

            if (hoursMins == "mins")
            {
                // Check string is 2 characters long
                if (timeDigits.Length != 2)
                {
                    return false;
                }
                if (int.Parse(timeDigits) > 59)
                {
                    return false;
                }

            }

            return true;
        }

        // Enable UI input elements based on EnableDateTimeCheck bool.
        private void EnableDisableUIItems(bool isEnabled)
        {
            TimeTextBox.IsEnabled = isEnabled;
            TimeTextBlock.IsEnabled = isEnabled;
            DaysBeforeTextBox.IsEnabled = isEnabled;
            DaysBeforeTextBlock.IsEnabled = isEnabled;
            if (isEnabled)
            {
                TimeTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                DaysBeforeTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                TimeTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 127, 127, 127));
                DaysBeforeTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 127, 127, 127));
                
            }
        }

        // Save Preferences
        private void SavePrefsButton_Click(object sender, RoutedEventArgs e)
        {

            ParseDaysBeforeToInt();

            ParseTextToTime();

            _prefs.SavePrefs();

            SavePrefsButton.IsEnabled = false;
        }

        // Hide instead of close the window - allows the window to be reopened again
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }

        private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ParseTextToTime();
        }
    }
}
