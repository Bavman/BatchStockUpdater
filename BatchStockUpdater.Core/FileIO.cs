using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Data;

namespace BatchStockUpdater.Core
{
    public class FileIO
    {

        Prefs _prefs;

        public FileIO()
        {
            if (_prefs == null)
            {
                _prefs = Prefs.GetInstance();
            }
        }

        public string[] CSVHeaders;

        // Load CSV and return data as a TextFieldParser object
        public TextFieldParser LoadCSV(string filePath, bool checkHeader, bool isCheckDateTime = false, bool showFileExistsMessage = false)
        {
            var doesFileExist = CheckFilePath(filePath);

            if (!doesFileExist)
            {
                if (showFileExistsMessage)
                {
                    MessageBox.Show("Coul not find " + filePath + " Please check the file location and try again.");
                }
                return null;
            }

            // Check that imported CSV meets the date time requirements in preferences
            if (isCheckDateTime)
            {
                if (!IsFileDateModifiedCurrent(filePath, CheckFileDateTime()))
                {
                    return null;
                }
            }
            
            // Retrieve CSV TextFieldParser object
            var csvData = ReturnCSVData(filePath);

            // If 'checkHeader' bool is requested the header of the CSV is checked.
            // Either the TextFieldParser object is returned if successful or a null if not.
            if (checkHeader)
            {
                var isCSVHeaderOK = ChechHearderRow(csvData, CSVHeaders);

                if (isCSVHeaderOK)
                {
                    return csvData;
                }
                else
                {
                    MessageBox.Show("CSV header row did not pass the comparison check.");
                    return null;
                }
            }
            else
            {
                return csvData;
            }
 
        }

        // Write CSV DataTable to csv text file
        public bool SaveCSV(string filePath, DataTable dataTable)
        {

            var isSaved = false;

            var folder = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(folder))
            {
                MessageBox.Show("Destination folder does not exist. Please check the preferences and try again.");
                return false;
            }

            try
            {
                WriteDataTableToFile(filePath, dataTable);

                isSaved = true;

                Logging.GetInstance().LogExportCSV();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(@"Cannot save to C:\. Will have to run the application as Administrator");
                //throw new ArgumentException(@"Cannot write to c:\. Try running program as Administrator");
            }

            return isSaved;
        }

        // Write DataTable to csv file with StreamWriter
        private void WriteDataTableToFile(string filePath, DataTable dataTable)
        {
            var streamWriter = new StreamWriter(filePath);

            var rows = dataTable.Rows;

            var stringBuilder = new StringBuilder();

            // Collect column names and add to string builder
            var colNames = dataTable.Columns.Cast<DataColumn>().
                Select(column => column.ColumnName).
                ToArray();

            stringBuilder.AppendLine(string.Join(",", colNames));


            // Loop through DataTable rows
            for (var i = 0; i < rows.Count; i++)
            {
                var rowFields = rows[i].ItemArray.
                    Select(field => field.ToString()).ToArray();

                stringBuilder.AppendLine(string.Join(",", rowFields));
            }

            streamWriter.WriteLine(stringBuilder);
            streamWriter.Close();
        }

        // Checks if the file modified date and time if before the date and time in preferences
        public bool IsFileDateModifiedCurrent(string filePath, DateTime checkDateTime)
        {
            var fileInfo = new FileInfo(filePath);
            var fileDateModified = fileInfo.LastWriteTime;

            var dateTimeCompare = DateTime.Compare(fileDateModified, checkDateTime);

            if (dateTimeCompare < 0)
            {
                MessageBox.Show("CSV file to be imported is older than expected. \nPlease check the file " +
                        "is recent or adjust the time and days settings in preferences");
                return false;
            }
            return true;
        }

        // Check File Date and Time
        private DateTime CheckFileDateTime()
        {

            var checkDateTime = DateTime.Today;

            checkDateTime = checkDateTime.AddDays((double)-_prefs.NDaysBefore);

            checkDateTime = checkDateTime.Add(_prefs.TimeCheck);

            return checkDateTime;

        }

        // Check if file exists at file path string
        public bool CheckFilePath(string filePath)
        {
            var doesFileExist = false;

            if (File.Exists(filePath))
            {
                doesFileExist = true;
            }

            return doesFileExist;
        }
        
        // Returns parsed CSV data from file
        public TextFieldParser ReturnCSVData(string path)
        {
            var fileContents = String.Empty;

            var csvParser = new TextFieldParser(path);

            csvParser.TextFieldType = FieldType.Delimited;
            csvParser.SetDelimiters(",");

            return csvParser;
        }

        // Checks header row to see if it matches the header row details set in preferences.
        public bool ChechHearderRow(TextFieldParser csvData, string[] compareHeader)
        {
            var hasPassed = true;

            if (csvData != null)
            {
                var header = csvData.ReadLine();

                var headerArray = new string[0];

                // Check file contains characters
                if (header != null)
                {
                    headerArray = header.Split(',');
                }
                else
                {
                    return false;
                }

                // Fail if too many header counts
                if (headerArray.Length != compareHeader.Length)
                {
                    return false;
                }

                for (var i = 0; i < headerArray.Length; i++)
                {
                    if (headerArray[i] != compareHeader[i])
                    {
                        hasPassed = false;
                    }
                }
            }

            return hasPassed;
        }
    }
}
