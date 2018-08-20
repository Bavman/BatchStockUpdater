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

namespace BatchStockUpdater.UI
{
    /// <summary>
    /// Interaction logic for FileOverrideQuery.xaml
    /// </summary>
    public partial class OverwriteQueryWindow : Window
    {
        // Variables
        MainWindow _mainWindow;
        string _filePath;

        // Initialize Window and assign variables
        public OverwriteQueryWindow(MainWindow mainWindow, string filePath)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _filePath = filePath;
        }

        // Hide instead of close the window - allows the window to be reopened again
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        // Closes UI and does not overwrite stocklist.csv
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Overwrites stocklist.csv and closes window
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ExportCSVFile(_filePath);
            this.Close();
        }
    }
}
