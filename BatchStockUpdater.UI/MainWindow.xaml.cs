using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BatchStockUpdater.Core;
using System.Data;
using BatchStockUpdater.Users;
using System.Diagnostics;

namespace BatchStockUpdater.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Class References
        FileIO _csvFileIO;
        Prefs _prefs;
        DataMethods _dataMethods;
        Logging _logging;

        IUsersRepository _userRepository;

        // XAML Window References
        LoginWindow _loginWindow;
        OverwriteQueryWindow _overwriteQueryWindow;
        PreferencesWindow _preferncesWindow;
        ManageUsersWindow _manageUserWindow;

        // Variables
        DataTable _csvDataTable;
        public List <int> _csvCurrentCountsImport;
        public string CurrentUserName;

        DataGrid _csvDataGrid;
        bool _isDataTableImported;

        string[] _csvHeader = new string[]
        {
            "Item Code",
            "Item Description",
            "Current Count",
            "On Order"
        };

        // Initialize classes, DataTable, DataGrid objects and hide buttons 
        public MainWindow()
        {

            this.Title = "Batch Stock Updater";

            _logging = Logging.GetInstance();
            _logging.OpenApplication();

            // Load Users or create administrator user and save user files
            _userRepository = CSVUserRepository.GetInstance();
            _userRepository.InitializeUsers();

            _prefs = Prefs.GetInstance();
            _prefs.LoadPrefs();

            _dataMethods = DataMethods.GetInstance();
            _manageUserWindow = new ManageUsersWindow(_userRepository);

            _loginWindow = new LoginWindow(this, _userRepository);
            _preferncesWindow = new PreferencesWindow();
            
            _csvFileIO = new FileIO();
            _csvFileIO.CSVHeaders = _csvHeader;
            
            InitializeComponent();

            PreferencesButton.Visibility = Visibility.Hidden;
            ManageUsersButton.Visibility = Visibility.Hidden;
            LogsButton.Visibility = Visibility.Hidden;

            // Default DataTable view on boot
            var csvDataTable = new DataTable();
            var defaultViewDataTable = _dataMethods.SetupDataTable(csvDataTable, _csvHeader);
            SetupDataGrid(defaultViewDataTable);
        }

        #region buttons
        // Buttons and objects related to the XAML MainWindow
        private void CSVDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("Source" + e.Source.ToString());
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _loginWindow.Show();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            _isDataTableImported = false;
            EnableButtons(false);
            ClearDataTable();
            LoginButton.IsEnabled = true;
            _logging.LogLogOut(CurrentUserName);

            CurrentUserName = String.Empty;
        }

        private void ImportCSVButton_Click(object sender, RoutedEventArgs e)
        {
            // Prevent multiple populations of DataTable
            if (!_isDataTableImported)
            {
                var csvData = _csvFileIO.LoadCSV(_prefs.FilePath, true, _prefs.CheckFileDateAndTime, true);
                if (csvData != null)
                {
                    _csvDataTable = _dataMethods.PopulateDataTable(csvData, _csvHeader);

                    if (_csvDataTable != null)
                    {
                        _csvDataGrid.ItemsSource = _csvDataTable.DefaultView;
                        SetDataGridWidths();

                        //Make a new collection of the 'Current Count' to check on export of csv
                        _csvCurrentCountsImport = _dataMethods.ReturnColumnCollection<int>(_csvDataTable, _csvHeader[2]).ToList();

                        _isDataTableImported = true;
                        ExportCSVButton.IsEnabled = true;
                        _logging.LogImportCSV(FailSuccessStatus.Success);
                    }
                }
                else
                {
                    _logging.LogImportCSV(FailSuccessStatus.Failure);
                }
            }

        }

        private void ExportCSVButton_Click(object sender, RoutedEventArgs e)
        {
            // Check the DataTable has been imported by verifying the row count is greater than 1.
            if (_csvDataTable.Rows.Count == 0)
            {
                MessageBox.Show("Datafile has not been imported. Please Import and update before exporting");

                return;
            }

            // Check if there have been changes on the 'Current Count' column
            var currentCountList = _dataMethods.ReturnColumnCollection<int>(_csvDataTable, _csvHeader[2]).ToList();

            var isCurrentCountDifferent = _dataMethods.CompareCurrentCountsTables<int>(_csvCurrentCountsImport, currentCountList);

            if (isCurrentCountDifferent)
            {
                MessageBox.Show("'Current Counts' items have not changed since last save. \nThe CSV file will not be saved");
                return;
            }

            if (_csvFileIO.CheckFilePath(_prefs.FilePath))
            {
                if (_overwriteQueryWindow == null)
                {
                    _overwriteQueryWindow = new OverwriteQueryWindow(this, _prefs.FilePath);
                }
                _overwriteQueryWindow.Show();
            }
            else
            {
                _csvFileIO.SaveCSV(_prefs.FilePath, _csvDataTable);
            }  
        }

        private void PreferencesButton_Click(object sender, RoutedEventArgs e)
        {
            _preferncesWindow.Show();
            _prefs.LoadPrefs();
            _preferncesWindow.InitializeUI();
        }

        private void ManageUsersButton_Click(object sender, RoutedEventArgs e)
        {
            _manageUserWindow.Show();
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            var folder = Logging.GetInstance().LogFolder;
            Process.Start(folder);
        }

        #endregion

        public void LogInApproved(UserTypeEnum userType)
        {
            EnableButtons(true, userType);
            LoginButton.IsEnabled = false;
        }

        // Hide instead of close the window - allows the window to be reopened again
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _logging.CloseApplication();

            //e.Cancel = true;
            //this.Hide();
        }

        // Export data to csv via _csvFileIO
        public void ExportCSVFile(string filepath)
        {
            _csvFileIO.SaveCSV(filepath, _csvDataTable);

            //Make a new collection of the 'Current Count' to check on export of csv
            _csvCurrentCountsImport = _dataMethods.ReturnColumnCollection<int>(_csvDataTable, _csvHeader[2]).ToList();

        }

        // Setup DataGrid and connect _csvDataTable to DataGrid
        private void SetupDataGrid(DataTable dataTable)
        {

            _csvDataGrid = CSVDataGrid;
            _csvDataGrid.ItemsSource = dataTable.DefaultView;
            _csvDataGrid.CanUserAddRows = false;
            _csvDataGrid.CanUserDeleteRows = false;

        }

        // Set DataGrid column Widths
        private void SetDataGridWidths()
        {
            _csvDataGrid.Columns[0].Width = 90;
            _csvDataGrid.Columns[1].Width = 300;
            _csvDataGrid.Columns[2].Width = 120;
            _csvDataGrid.Columns[3].Width = 80;
        }

        //@TODO Update colour of modified cell
        private void UpdateCellColour(int row, int column)
        {
            //_csvDataGrid.RowBackground = new SolidColorBrush(Color.FromArgb(255, 127, 255, 127));
        }

        // Make function buttons visible based on user type accessability
        private void EnableButtons(bool isLoggedIn, UserTypeEnum userType = UserTypeEnum.Staff)
        {
            if (isLoggedIn)
            {
                switch (userType)
                {
                    case UserTypeEnum.Administrator:
                        PreferencesButton.Visibility = Visibility.Visible;
                        ManageUsersButton.Visibility = Visibility.Visible;
                        LogsButton.Visibility = Visibility.Visible;
                        break;

                    case UserTypeEnum.Manager:
                        PreferencesButton.Visibility = Visibility.Visible;
                        ManageUsersButton.Visibility = Visibility.Visible;
                        break;

                    case UserTypeEnum.Staff:

                        break;
                    default:
                        break;
                }
                LogOutButton.IsEnabled = true;
                ImportCSVButton.IsEnabled = true;
            }
            else
            {
                PreferencesButton.Visibility = Visibility.Hidden;
                ManageUsersButton.Visibility = Visibility.Hidden;
                LogsButton.Visibility = Visibility.Hidden;
                LogOutButton.IsEnabled = false;
                ImportCSVButton.IsEnabled = false;
                ExportCSVButton.IsEnabled = false;
            }
        }

        // Clear Datatable and Datagrid
        private void ClearDataTable()
        {
            if (_csvDataTable != null)
            {
                _csvDataTable.Clear();
                _csvDataGrid.ItemsSource = _csvDataTable.DefaultView;
            }

        }

        //@TODO Update colour of modified cell
        private void CSVDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            
            var cell = new DataGridCell();

            var row = e.Row;
            var column = e.Column;

          

            //row.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 127));

            Console.WriteLine("row"+row.GetIndex());
            Console.WriteLine("column" + column.DisplayIndex);
            Console.WriteLine("Int List " + _csvCurrentCountsImport[row.GetIndex()]);
        }

    }

}
