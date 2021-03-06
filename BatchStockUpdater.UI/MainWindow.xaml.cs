﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BatchStockUpdater.Core;
using System.Data;
using BatchStockUpdater.Users;
using System.Diagnostics;
using System.Text;

namespace BatchStockUpdater.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Class References
        FileIO _csvFileIO = new FileIO();
        Prefs _prefs = Prefs.GetInstance();
        DataMethods _dataMethods = DataMethods.GetInstance();
        XMLExporter _xmlExporter = new XMLExporter();


        IUsersRepository _userRepository = CSVUserRepository.GetInstance();

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

            Logging.GetInstance().OpenApplication();

            // Load Users or create administrator user and save user files

            _userRepository.InitializeUsers();

            _prefs.LoadPrefs();

            _manageUserWindow = new ManageUsersWindow(_userRepository);

            _loginWindow = new LoginWindow(this);
            _preferncesWindow = new PreferencesWindow();

            UserLogin.UsersRepository = _userRepository;

            InitializeComponent();

            HideUIEmementsOnBoot();

            // Default DataTable view on boot
            var csvDataTable = new DataTable();
            var defaultViewDataTable = _dataMethods.SetupDataTable(csvDataTable, _csvHeader);
            SetupDataGrid(defaultViewDataTable);
        }

        private void StatusMethod(string status, int intVar)
        {
            Console.WriteLine();
        }

        #region buttons
        // Buttons and objects related to the XAML MainWindow
        private void CSVDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Console.WriteLine("Source" + e.Source);

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

            Logging.GetInstance().LogLogOut(CurrentUserName);

            CurrentUserName = String.Empty;
        }

        private void ImportCSVButton_Click(object sender, RoutedEventArgs e)
        {
            // Prevent multiple populations of DataTable
            if (!_isDataTableImported)
            {
                var csvData = _csvFileIO.LoadCSV(_prefs.CSVFilePath, _csvHeader, _prefs.CheckFileDateAndTime, true);
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
                        ExportXMLButton1.IsEnabled = true;
                        ExportXMLButton2.IsEnabled = true;
                        OpenFolderButton.IsEnabled = true;
                        Logging.GetInstance().LogImportCSV(LogStatus.Success);
                    }
                }
                else
                {
                    Logging.GetInstance().LogImportCSV(LogStatus.Failure);
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

            // Open Overwrite Query Window or save if file does not exist in path
            if (_csvFileIO.CheckFilePath(_prefs.CSVFilePath))
            {
                if (_overwriteQueryWindow == null)
                {
                    _overwriteQueryWindow = new OverwriteQueryWindow(this, _prefs.CSVFilePath);
                }
                _overwriteQueryWindow.Show();
            }
            else
            {
                _csvFileIO.SaveCSV(_prefs.CSVFilePath, _csvDataTable);
            }  
        }

        private static string AllOperationMessages(OperationResult result)
        {
            var builtMessage = new StringBuilder();
            builtMessage.AppendLine(string.Join(",\n", result.MessageList.ToArray()));
            return builtMessage.ToString();
        }

        private void ExportXMLButton1_Click(object sender, RoutedEventArgs e)
        {
            var result = _xmlExporter.ExportXML(_prefs.XMLFilePathStyle1, _csvDataTable, 1);
            if (result.Success)
            {
                MessageBox.Show(AllOperationMessages(result));
            }
            else
            {;
                MessageBox.Show(AllOperationMessages(result));
            }

        }

        private void ExportXMLButton2_Click(object sender, RoutedEventArgs e)
        {
            var result = _xmlExporter.ExportXML(_prefs.XMLFilePathStyle2, _csvDataTable, 2);
            if (result.Success)
            {
                MessageBox.Show(AllOperationMessages(result));
            }
            else
            {
                ;
                MessageBox.Show(AllOperationMessages(result));
            }
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(_prefs.FolderName);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("Could not find the 'import / export' folder. Please check preferences or contact the Systems Administrator.");
            }
            catch (Exception)
            {
                MessageBox.Show("Please contact the Systems Administrator.");
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


        // Hide MainWindow UI Buttons - defaults for lowest user accessability type
        private void HideUIEmementsOnBoot()
        {
            PreferencesButton.Visibility = Visibility.Hidden;
            ManageUsersButton.Visibility = Visibility.Hidden;
            LogsButton.Visibility = Visibility.Hidden;
        }

        // Enables MainWindow UI Buttons based on user types accessability
        public void LogInApproved(UserTypeEnum userType)
        {
            EnableButtons(true, userType);
            LoginButton.IsEnabled = false;
        }

        // Hide instead of close the window - allows the window to be reopened again
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Logging.GetInstance().CloseApplication();

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
                ExportXMLButton1.IsEnabled = false;
                ExportXMLButton2.IsEnabled = false;
                OpenFolderButton.IsEnabled = false;
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
