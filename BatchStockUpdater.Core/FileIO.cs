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
        public TextFieldParser LoadCSV(string filePath, bool checkHeader, bool isCheckDateTime = false, bool displayFileExistsMessage = false)
        {
            var doesFileExist = CheckFilePath(filePath);
            

            if (!doesFileExist)
            {
                if (displayFileExistsMessage)
                {
                    MessageBox.Show("Coul not find " + filePath + " Please check the file location and try again.");
                }
                return null;
            }

            // Check that imported CSV meets the date time requirements in preferences
            if (isCheckDateTime)
            {
                if (!DoesFileDateCheckPass(filePath, CheckFileDateTime()))
                {
                    MessageBox.Show("CSV file to be imported is older than expected. \nPlease check the file " +
                        "is recent or adjust the time and days settings in preferences");
                    return null;
                }
            }
            
            // Retrieve CSV TextFieldParser object
            var csvData = ReturnCSVData(filePath);

            var returnCSVData = false;

            if (checkHeader)
            {
                returnCSVData = VerifyHearderRow(csvData, CSVHeaders);
            }
            else
            {
                returnCSVData = true;
            }

            // Return CSV string or fail and return null
            if (returnCSVData)
            {
                return ReturnCSVData(filePath);
            }
            else
            {
                MessageBox.Show("CSV header row did not pass the comparison check.");
                return null;
            }
               
        }

        // Write CSV DataTable to csv text file
        public void SaveCSV(string filePath, DataTable dataTable)
        {
            var folder = Path.GetDirectoryName(filePath);
            Console.WriteLine(folder);

            if (!Directory.Exists(folder))
            {
                MessageBox.Show("Destination folder does not exist. Please check the preferences and try again.");
                return;
            }

            try
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
            catch (Exception)
            {

                throw new ArgumentException(@"Cannot write to c:\. Try running program as Administrator");
            }


        }

        // Checks if the file modified date and time if before the date and time in preferences
        public bool DoesFileDateCheckPass(string filePath, DateTime checkDateTime)
        {
            var fileInfo = new FileInfo(filePath);
            var fileDateModified = fileInfo.LastWriteTime;

            var dateTimeCompare = DateTime.Compare(fileDateModified, checkDateTime);

            if (dateTimeCompare < 0)
            {
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
        private bool VerifyHearderRow(TextFieldParser csvData, string[] compareHeader)
        {
            var hasPassed = true;

            if (csvData != null)
            {
                var header = csvData.ReadLine();
                var headerArray = header.Split(',');

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
